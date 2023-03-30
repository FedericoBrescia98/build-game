using build_and_expand.Content;
using build_and_expand.Objects;
using build_and_expand.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml.Linq;

namespace build_and_expand.UI
{
    public class HUD : Component
    {
        public GameState GameState { get; set; }
        protected GameContent _content { get; set; }
        protected SpriteFont _font { get; set; }
        protected List<Component> _components = new List<Component>();

        private Texture2D _bottomNavBar;
        private Rectangle _bottomNavBarRect = new Rectangle(new Point(0, (int)C.DISPLAYDIM.Y - 192),
                                                            new Point((int)C.DISPLAYDIM.X,192));
        private Texture2D _topNavBar;
        private Rectangle _topNavBarRect = new Rectangle(new Point(0, 0),
                                                            new Point((int)C.DISPLAYDIM.X, 64));
        private Button _menu;
        private Button _houseBuild;
        private Button _logCabinBuild;
        private Button _windmillBuild;
        private Button _farmBuild;
        private Button _quarryBuild;
        private Button _roadBuild;
        private Button _demolish;

        private Inventory _inventory;
        private Calendar _calendar;

        public HUD(GameContent content, Action onMenu)
        {
            Texture2D houseButton = content.GetUiTexture(6);
            Texture2D logCabinButton = content.GetUiTexture(7);
            Texture2D windmillButton = content.GetUiTexture(8);
            Texture2D farmButton = content.GetUiTexture(16);
            Texture2D quarryButton = content.GetUiTexture(17);
            Texture2D roadButton = content.GetUiTexture(18);
            Texture2D menuButton = content.GetUiTexture(10);
            Texture2D demolishButton = content.GetUiTexture(11);
            Texture2D textBubble = content.GetUiTexture(14);
            _bottomNavBar = content.GetUiTexture(5);
            _topNavBar = content.GetUiTexture(5);

            SpriteFont font = content.GetFont(1);

            _menu = new Button(menuButton, font)
            {
                Scale = new Vector2(2f,2f),
                Text = "MENU",
                Position = new Point(16, 16)
            };
            _menu.Click += (object sender, EventArgs e) => onMenu();


            _houseBuild = new Button(houseButton, font)
            {
                ObjectId = 100,
                Position = new Point(16, _bottomNavBarRect.Y + 16),
                TextBubble = new TextBubble(textBubble, "Basic House\r\nCost: 25 Wood\r\nOutputs: 4 Workers", font)
                {
                    Position = new Point(16, _bottomNavBarRect.Y + -96),
                }
            };
            _houseBuild.Click += (object sender, EventArgs e) => { GameState.SelectedObject = Building.BasicHouse(); };

            _logCabinBuild = new Button(logCabinButton, font)
            {
                ObjectId = 200,
                Position = new Point(16 + 96, _bottomNavBarRect.Y + 16),
                TextBubble = new TextBubble(textBubble, "Log Cabin\r\nCost: 20 Wood, 1 Worker\r\nOutputs per day: 10 Wood", font)
                {
                    Position = new Point(16 + 96, _bottomNavBarRect.Y + -96),
                }
            };
            _logCabinBuild.Click += (object sender, EventArgs e) => { GameState.SelectedObject = Building.LogCabin(); };

            _farmBuild = new Button(farmButton, font)
            {
                ObjectId = 300,
                Position = new Point(16 + 192, _bottomNavBarRect.Y + 16),
                TextBubble = new TextBubble(textBubble, "Farm\r\nCost: 25 Wood, 1 Worker\r\nOutputs per day: 10 Food", font)
                {
                    Position = new Point(16 + 192, _bottomNavBarRect.Y + - 96),
                }
            };
            _farmBuild.Click += (object sender, EventArgs e) => { GameState.SelectedObject = Building.Farm(); };

            _quarryBuild = new Button(quarryButton, font)
            {
                ObjectId = 400,
                Position = new Point(16 + 288, _bottomNavBarRect.Y + 16),
                TextBubble = new TextBubble(textBubble, "Quarry\r\nCost: 50 Wood, 2 Worker\r\nOutputs per day: 10 Stone", font)
                {
                    Position = new Point(16 + 288, _bottomNavBarRect.Y + -96),
                }
            };
            _quarryBuild.Click += (object sender, EventArgs e) => { GameState.SelectedObject = Building.Quarry(); };

            _windmillBuild = new Button(windmillButton, font)
            {
                ObjectId = 500,
                Position = new Point(16 + 384, _bottomNavBarRect.Y + 16),
                TextBubble = new TextBubble(textBubble, "Wind Mill\r\nCost: 40 Wood, 1 Worker\r\nOutputs per day: 10 Food", font)
                {
                    Position = new Point(16 + 384, _bottomNavBarRect.Y + -96),
                }
            };
            _windmillBuild.Click += (object sender, EventArgs e) => { GameState.SelectedObject = Building.Windmill(); };

            _roadBuild = new Button(roadButton, font)
            {
                ObjectId = 1000,
                Position = new Point(16 + 480, _bottomNavBarRect.Y + 16),
                TextBubble = new TextBubble(textBubble, "Road\r\nCost: 10 Stone\r\n", font)
                {
                    Position = new Point(16 + 480, _bottomNavBarRect.Y + -96),
                }
            };
            _roadBuild.Click += (object sender, EventArgs e) => { GameState.SelectedObject = Building.Road(); };

            _demolish = new Button(demolishButton, font)
            {
                Position = new Point(_bottomNavBarRect.Width - 96, _bottomNavBarRect.Y + 16),
                TextBubble = new TextBubble(textBubble, "Destroy buildings", font)
                {
                    Position = new Point(_bottomNavBarRect.Width - 156, _bottomNavBarRect.Y + -96),
                }
            };
            _demolish.Click += (object sender, EventArgs e) => { GameState.DeleteBuildingButtonClick(); };

            _inventory = new Inventory(content);
            _calendar = new Calendar(content);

            // add buttons to list of components
            _components = new List<Component>()
            {
                _menu,
                _houseBuild,
                _logCabinBuild,
                _farmBuild,
                _quarryBuild,
                _windmillBuild,
                _roadBuild,
                _inventory,
                _demolish,
                _calendar
            };
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // draw background
            spriteBatch.Draw(_bottomNavBar, _bottomNavBarRect, Color.White);
            spriteBatch.Draw(_topNavBar, _topNavBarRect, Color.White);

            // draw each component
            foreach(Component component in _components)
                component.Draw(gameTime, spriteBatch);
        }

        public override void Update(GameTime gameTime, GameState gameState)
        {
            // update each component
            foreach(Component component in _components)
                component.Update(gameTime, gameState);

            // save gamestate
            GameState = gameState;
        }

        public bool Intersects(Rectangle rectangle)
        {
            if(_bottomNavBarRect.Intersects(rectangle))
            {
                return true;
            }
            if(_topNavBarRect.Intersects(rectangle))
            {
                return true;
            }
            return false;
        }
    }
}
