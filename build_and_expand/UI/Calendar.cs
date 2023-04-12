using build_and_expand.Content;
using build_and_expand.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace build_and_expand.UI
{
    public class Calendar : Component
    {
        private GameStateData _gameStateData;
        Texture2D _icon;
        protected GameContent _content { get; set; }
        protected SpriteFont _font { get; set; }

        public Calendar(GameContent gameContent)
        {
            _content = gameContent;
            _icon = gameContent.GetUiTexture(9);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Rectangle iconRect = new Rectangle(C.DISPLAYDIM.X - 256, 16,
                C.TILETEXTURESIZE.X, C.TILETEXTURESIZE.Y);
            spriteBatch.Draw(_icon, iconRect, Color.White);

            _font = _content.GetFont(1);
            String text = "Days: " + _gameStateData.Day.ToString();
            spriteBatch.DrawString(_font, text, new Vector2(C.DISPLAYDIM.X - 208, 24), Color.White);

            text = "Year: " + _gameStateData.Year.ToString();
            spriteBatch.DrawString(_font, text, new Vector2(C.DISPLAYDIM.X - 96, 24), Color.White);
        }

        public override void Update(GameTime gameTime, GameState gameState)
        {
            _gameStateData = gameState.GameStateData;
        }
    }
}
