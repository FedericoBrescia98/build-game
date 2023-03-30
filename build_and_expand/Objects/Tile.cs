using build_and_expand.Content;
using build_and_expand.States;
using build_and_expand.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;
using System;

namespace build_and_expand.Objects
{
    public class Tile
    {
        // object (building or resource) belong to this tile
        public TileObject Object { get; set; }
        public bool IsHovered { get; set; } = false;
        public Color DrawColor { get; set; } = Color.White;
        [JsonIgnore] public GameContent Content { get; set; }
        [JsonIgnore] private GameState _gameState { get; set; }
        private MouseState PreviousMouseState { get; set; }
        [JsonIgnore] private Texture2D Texture { get; set; }

        // hitbox for mouse touch
        public Rectangle TouchHitbox
        {
            get { return new Rectangle((int)Position.X, (int)Position.Y, C.TILETEXTURESIZE.X, C.TILETEXTURESIZE.Y); }
        }

        public Point Position { get; set; } = Point.Zero;
        public Point PositionToMap { get; set; } = Point.Zero;
        public Point CenterPoint => Position + new Point(16, 12);

        // used to determine when clicked
        public event EventHandler Click;
        public event EventHandler Pressed;
        public event EventHandler Pressing;
        public event EventHandler RightClick;

        // animated tile properties
        [JsonIgnore] public AnimatedTexture AnimTexture { get; set; }
        [JsonIgnore] public AnimatedTexture DestructionFX { get; set; }

        public bool HasAnimatedTexture => AnimTexture != null;

        // destruction properties
        public bool ObjectDestroyed { get; set; } = false;

        public Tile(GameContent content, TileObject tileObject, Point position)
        {
            Content = content;
            Object = tileObject ?? new TileObject();
            Position = position;
            PositionToMap = new Point(position.X / C.TILETEXTURESIZE.X, position.Y / C.TILETEXTURESIZE.Y);
            PreviousMouseState = Mouse.GetState();
            if(Object.ObjectId >= 1 && content != null)
            {
                DestructionFX = new AnimatedTexture(Content.GetTileTexture(-2), Position);
            }
        }

        // update
        // - check for mouse hovering and click (select)
        public void Update(GameTime gameTime, GameState gameState)
        {

            MouseState currentMouse = Mouse.GetState();

            // convert mouse screen position to world position
            Point mScreenPosition = currentMouse.Position;
            Vector2 mWorldPosition = Vector2.Transform(mScreenPosition.ToVector2(),
                                                        Matrix.Invert(gameState.CurrentCamera.getTransformation()));

            // get bounds for mouse world position
            Rectangle mouseWorldRectangle = new Rectangle((int)mWorldPosition.X, (int)mWorldPosition.Y, 1, 1);

            // get bounds for mouse screen position
            Rectangle mouseScreenRectangle = new Rectangle((int)mScreenPosition.X, (int)mScreenPosition.Y, 1, 1);

            // check if mouse bounds intersects with tile touchbox bounds
            if(mouseWorldRectangle.Intersects(TouchHitbox) && gameState.GameHUD.Intersects(mouseScreenRectangle).Equals(false))
            {
                gameState.CurrentlyHoveredTile = this;
                IsHovered = true;

                switch(currentMouse.LeftButton)
                {
                    case ButtonState.Pressed when PreviousMouseState.LeftButton == ButtonState.Pressed:
                        if(!(gameState.CurrentlyPressedTile.Equals(this)))
                        {
                            Pressing?.Invoke(this, new EventArgs());
                        }

                        break;
                    case ButtonState.Pressed when PreviousMouseState.LeftButton == ButtonState.Released:
                        Pressed?.Invoke(this, new EventArgs());
                        break;
                    case ButtonState.Released when PreviousMouseState.LeftButton == ButtonState.Pressed:
                        Click?.Invoke(this, new EventArgs());
                        break;
                }

                if(currentMouse.LeftButton == ButtonState.Released && PreviousMouseState.LeftButton == ButtonState.Pressed)
                {
                    RightClick?.Invoke(this, new EventArgs());
                }
            }
            else
            {
                IsHovered = false;
            }

            if(DestructionFX != null)
            {
                // if destroing animation finished
                if(DestructionFX.Finished)
                {
                    ObjectDestroyed = false;
                }
            }

            PreviousMouseState = currentMouse;
            _gameState = gameState;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // set draw color to orange red if hovered by mouse, otherwise draw normal color
            if(IsHovered)
            {
                DrawColor = Color.OrangeRed;
            }
            else
            {
                DrawColor = Color.White;
            }

            if(Object.ObjectId == 1000)
            {
                Texture = Content.GetTileTexture(1);
                spriteBatch.Draw(Texture, destinationRectangle:
                                            new Rectangle(new Point(Position.X, Position.Y),
                                            C.TILETEXTURESIZE), color: DrawColor);
                Texture = Content.GetTileTexture(Object.TerrainId);
                spriteBatch.Draw(Texture, destinationRectangle:
                                            new Rectangle(new Point(Position.X, Position.Y),
                                            C.TILETEXTURESIZE), color: DrawColor);
            }
            else
            {
                if(Object.ObjectId > 1)
                {
                    // draw base texture
                    Texture = Content.GetTileTexture(Object.TerrainId);
                    spriteBatch.Draw(Texture, destinationRectangle:
                                                new Rectangle(new Point(Position.X, Position.Y),
                                                C.TILETEXTURESIZE), color: DrawColor);
                }
                Texture = Content.GetTileTexture(Object.ObjectId);
                spriteBatch.Draw(Texture, destinationRectangle:
                                             new Rectangle(new Point(Position.X, Position.Y),
                                             C.TILETEXTURESIZE), color: DrawColor);
            }
            // draw animation
            if(HasAnimatedTexture)
            {
                AnimTexture.Draw(gameTime, spriteBatch);
            }

            // draw destruction fx
            if(ObjectDestroyed.Equals(true))
            {
                Object = new TileObject();
                DestructionFX.Draw(gameTime, spriteBatch);
            }
        }
    }
}
