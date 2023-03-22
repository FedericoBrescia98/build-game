using build_and_expand.Content;
using build_and_expand.States;
using build_and_expand.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace build_and_expand.Objects
{
    public class Inventory : Component
    {
        protected GameContent _content { get; set; }
        private Texture2D Texture;
        private SpriteFont Fonts;
        private InventoryData _inventoryData;

        public Inventory(GameContent content)
        {
            _content = content;
            _inventoryData = new InventoryData();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Dictionary<int, int> invIndx = new Dictionary<int, int>()
            {
                {1, _inventoryData.Food},
                {2, _inventoryData.Wood},
                {3, _inventoryData.Workers},
            };
            for(int i = 1; i < 4; i++)
            {
                Point position = new Point(i * 156 + 16, 16);
                Texture = _content.GetUiTexture(i);
                spriteBatch.Draw(Texture, new Rectangle(new Point(position.X, position.Y), C.TILETEXTURESIZE), color: Color.White);
                Fonts = _content.GetFont(1);
                spriteBatch.DrawString(Fonts, invIndx[i].ToString(), new Vector2(i * 156 + 64, 24), Color.White);
            }
        }

        public override void Update(GameTime gameTime, GameState gameState)
        {
            // update inventory hud only if we are playing
            if(gameState is GameState)
            {
                _inventoryData = gameState.GameStateData.PlayerInventory;
            }
        }
    }
}
