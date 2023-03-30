using build_and_expand.Content;
using build_and_expand.States;
using build_and_expand.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

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
                {3, _inventoryData.Stone},
                {4, _inventoryData.FreeWorkers},
                {5, _inventoryData.TotalWorkers},
            };
            for(int i = 1; i < 5; i++)
            {
                Point position = new Point(164 + (i - 1) * 90, 16);
                Texture = _content.GetUiTexture(i);
                spriteBatch.Draw(Texture, new Rectangle(new Point(position.X, position.Y), C.TILETEXTURESIZE), color: Color.White);
                Fonts = _content.GetFont(1);
                string text = i == 4 ? invIndx[i].ToString() + "/" + invIndx[5].ToString() : invIndx[i].ToString();
                spriteBatch.DrawString(Fonts, text, new Vector2(200 + (i - 1) * 90, 24), Color.White);
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
