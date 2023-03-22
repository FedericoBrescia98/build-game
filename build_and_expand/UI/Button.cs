using System;
using build_and_expand.Objects;
using build_and_expand.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace build_and_expand.UI
{
    public class Button : Component
    {
        private MouseState _previousMouse;
        private MouseState _currentMouse;
        private readonly SpriteFont _font;
        private readonly Texture2D _texture;
        public bool ResourceLocked = false;
        public bool Locked = false;
        public int TextPadding = 4;
        public bool IsHovering { get; private set; }
        public Color PenColor { get; set; }
        public Color HoverColor { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 Scale { get; set; } = new Vector2(1f, 1f);
        public event EventHandler Click;
        public bool IsSelected { get; set; } = false;
        public string Text { get; set; }
        public int ObjectId { get; set; } = 0;
        public Rectangle Rectangle =>
                new Rectangle((int)Position.X,
                (int)Position.Y,
                (!string.IsNullOrEmpty(Text) ?
                    (((int)_font.MeasureString(Text).X + TextPadding) > _texture.Width ?
                        (int)_font.MeasureString(Text).X + TextPadding
                    : _texture.Width)
                : _texture.Width),
                _texture.Height);

        public Button(Texture2D texture, SpriteFont font = null)
        {
            _texture = texture;
            _font = font;
            PenColor = Color.Black;
            HoverColor = Color.DarkGray;
            if(ObjectId >= 100)
            {
                ResourceLocked = true;
            }
            if(font != null)
            {
                _font = font;
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Color color = Color.White;
            if(IsHovering)
            {
                color = HoverColor;
            }
            if(Locked)
            {
                color = Color.DarkGray;
            }
            if(IsSelected)
            {
                color = Color.DarkGray;
            }
            spriteBatch.Draw(_texture, Rectangle, color);
            if(!string.IsNullOrEmpty(Text))
            {
                float x = _font.MeasureString(Text).X / 2;
                float y = _font.MeasureString(Text).Y / 2;
                Vector2 origin = new Vector2(x, y);
                spriteBatch.DrawString(_font,
                    Text,
                    new Vector2(Rectangle.X + (Rectangle.Width - Rectangle.Width / 2),
                    Rectangle.Y + (Rectangle.Height - Rectangle.Height / 2)), PenColor,
                    0,
                    origin,
                    Scale,
                    SpriteEffects.None,
                    1);
            }
        }

        public override void Update(GameTime gameTime, GameState gameState)
        {
            _previousMouse = _currentMouse;
            _currentMouse = Mouse.GetState();
            Rectangle mouseRectangle = new Rectangle(_currentMouse.X, _currentMouse.Y, 1, 1);
            if(ResourceLocked)
            {
                if(ObjectId != 0)
                {
                    Building b = BuildingData.Dict_BuildingFromObjectID[ObjectId];
                    Locked = gameState.GameStateData.PlayerInventory.CanBuildObject(b);
                }
                else
                {
                    Locked = false;
                }
            }
            IsHovering = false;
            if(mouseRectangle.Intersects(Rectangle))
            {
                IsHovering = true;
                if(_currentMouse.LeftButton == ButtonState.Released && _previousMouse.LeftButton == ButtonState.Pressed)
                {
                    if(Locked == false)
                    {
                        Click?.Invoke(this, new EventArgs());
                    }
                }
            }
        }
    }
}
