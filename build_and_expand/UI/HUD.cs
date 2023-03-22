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
        private Rectangle _bottomNavBarRect = new Rectangle(new Point(0, (int)C.DISPLAYDIM.Y - 192), new Point((int)C.DISPLAYDIM.X, 192));
        private Texture2D _topNavBar;
        private Rectangle _topNavBarRect = new Rectangle(new Point(0, 0), new Point((int)C.DISPLAYDIM.X, 64));
        private Button _menu;
        private Button _houseBuild;
        private Button _logCabinBuild;
        private Button _windmillBuild;
        private Button _demolish;
        private Inventory _inventory;
        private Calendar _calendar;

        public HUD(GameContent content, Action onMenu)
        {
            Texture2D houseButton = content.GetUiTexture(5);
            Texture2D logCabinButton = content.GetUiTexture(6);
            Texture2D windmillButton = content.GetUiTexture(7);
            Texture2D menuButton = content.GetUiTexture(9);
            Texture2D demolishButton = content.GetUiTexture(10);
            _bottomNavBar = content.GetUiTexture(4);
            _topNavBar = content.GetUiTexture(4);

            SpriteFont font = content.GetFont(1);

            _menu = new Button(menuButton, font)
            {
                Scale = new Vector2(2f,2f),
                Text = "MENU",
                Position = new Vector2(16, 16)
            };
            _menu.Click += (object sender, EventArgs e) => onMenu();


            _houseBuild = new Button(houseButton, font)
            {
                ObjectId = 100,
                Position = new Vector2(16, _bottomNavBarRect.Y + 16)
            };
            _houseBuild.Click += (object sender, EventArgs e) => { GameState.SelectedObject = Building.BasicHouse(); };

            _logCabinBuild = new Button(logCabinButton, font)
            {
                ObjectId = 200,
                Position = new Vector2(16 + 96, _bottomNavBarRect.Y + 16)
            };
            _logCabinBuild.Click += (object sender, EventArgs e) => { GameState.SelectedObject = Building.LogCabin(); };

            _windmillBuild = new Button(windmillButton, font)
            {
                ObjectId = 500,
                Position = new Vector2(16 + 192, _bottomNavBarRect.Y + 16)
            };
            _windmillBuild.Click += (object sender, EventArgs e) => { GameState.SelectedObject = Building.Windmill(); };

            _demolish = new Button(demolishButton, font)
            {
                Position = new Vector2(_bottomNavBarRect.Width - 64, _bottomNavBarRect.Y + 16)
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
                _windmillBuild,
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
