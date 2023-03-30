using build_and_expand.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace build_and_expand.UI
{
    public class TextBubble : Component
    {
        private Texture2D _texture;
        private readonly SpriteFont _font;
        public Color PenColor { get; set; } = Color.Black;

        public Point Position { get; set; }
        public Vector2 Scale { get; set; } = new Vector2(1f, 1f);
        public int TextPadding = 4;

        private List<char> _char;
        private string _text = "";
        private string _displayText = "";

        private float Field => Rectangle.Width * 0.9f;
        public Rectangle Rectangle =>
            new Rectangle(Position.X, Position.Y,
                        _texture.Width, _texture.Height);

        public TextBubble(Texture2D texture, string text, SpriteFont font)
        {
            _text = text;
            _font = font;
            _texture = texture;
        }

        public override void Update(GameTime gameTime, GameState gameState)
        {
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if(_displayText == "")
            {
                string temp = "";
                foreach(char c in _text)
                {
                    temp += c;
                    Vector2 textVector = _font.MeasureString(temp);
                    if(textVector.X >= Field)
                    {
                        temp += Environment.NewLine;
                        _displayText += temp;
                        temp = "";
                    }
                }
                if(temp != "")
                {
                    _displayText += temp;
                }
            }

            spriteBatch.Draw(_texture, Rectangle, Color.White);

            float x = _font.MeasureString(_displayText).X / 2;
            float y = _font.MeasureString(_displayText).Y / 2;
            Vector2 origin = new Vector2(x, y);
            spriteBatch.DrawString(_font,
                _displayText,
                new Vector2(Rectangle.X + (Rectangle.Width / 2),
                Rectangle.Y + (Rectangle.Height / 2) - (2*TextPadding)), PenColor,
                0,
                origin,
                Scale,
                SpriteEffects.None,
                1);
        }

    }
}
