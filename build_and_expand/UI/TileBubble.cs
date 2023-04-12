using build_and_expand.Content;
using build_and_expand.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace build_and_expand.UI
{
    public class TileBubble : Component
    {
        public Texture2D IconTexture { get; set; }
        private Texture2D _bubbleTexture;
        private readonly SpriteFont _font;
        private string _text = "";
        public float Showtime = 2.0f;
        private float _bubbleTime = 0f;
        public Color PenColor { get; set; } = Color.Black;
        public Point Position { get; set; }
        public Vector2 Scale { get; set; } = new Vector2(1f, 1f);

        public Rectangle Rectangle =>
            new Rectangle(Position.X + (C.TILETEXTURESIZE.X / 2) - (_bubbleTexture.Width / 2), Position.Y,
                _bubbleTexture.Width, _bubbleTexture.Height);

        public TileBubble(GameContent content, string text, SpriteFont font)
        {
            _text = text;
            _font = font;
            _bubbleTexture = content.GetUiTexture(15);
        }

        public override void Update(GameTime gameTime, GameState gameState)
        {
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            _bubbleTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_bubbleTime > Showtime)
            {
                return;
            }

            Rectangle bubbleRect = new Rectangle(Rectangle.X, Rectangle.Y + (2 * (int)_bubbleTime), Rectangle.Width,
                Rectangle.Height);
            spriteBatch.Draw(_bubbleTexture, bubbleRect, Color.White);

            float x = _font.MeasureString(_text).X / 2;
            float y = _font.MeasureString(_text).Y / 2;
            Vector2 origin = new Vector2(x, y);
            spriteBatch.DrawString(_font,
                _text,
                new Vector2(Rectangle.X + (Rectangle.Width / 2),
                    Rectangle.Y + (Rectangle.Height / 2) + _bubbleTime), PenColor,
                0,
                origin,
                Scale,
                SpriteEffects.None,
                1);
        }
    }
}
