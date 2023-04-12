using System;
using System.Collections.Generic;
using build_and_expand.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace build_and_expand.States
{
    public class MenuState : State
    {
        // list to hold all components in menu
        private readonly List<Component> _components;
        private readonly Texture2D _cursorTexture;
        private readonly Texture2D _backgroundTexture;

        public MenuState(Game game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice,
            content)
        {
            _cursorTexture = _content.Load<Texture2D>("Sprites/UI/cursor");

            // variables to hold button texture and font
            Texture2D buttonTexture = _content.Load<Texture2D>("Sprites/UI/stretchButton");
            SpriteFont buttonFont = _content.Load<SpriteFont>("Fonts/slkscr");

            _backgroundTexture = _content.Load<Texture2D>("Sprites/Images/worldCapture");

            #region CREATE BUTTONS

            // create buttons and set properties, and click event functions
            Button newGameButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Point(_graphicsDevice.Viewport.Width / 2, _graphicsDevice.Viewport.Height / 2) +
                           new Point(0, -220),
                Text = "New Game",
                HoverColor = Color.Green,
                Scale = new Vector2(2.5f, 2.5f)
            };
            newGameButton.Click += NewGameButton_Click;
            newGameButton.Position = newGameButton.Position + new Point(-(newGameButton.Rectangle.Width / 2), 0);

            Button loadGameButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Point(_graphicsDevice.Viewport.Width / 2, _graphicsDevice.Viewport.Height / 2) +
                           new Point(0, -100),
                Text = "Load Game",
                HoverColor = Color.CornflowerBlue,
                Scale = new Vector2(2.5f, 2.5f)
            };
            loadGameButton.Click += LoadGameButton_Click;
            loadGameButton.Position = loadGameButton.Position + new Point(-(loadGameButton.Rectangle.Width / 2), 0);

            Button quitGameButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Point(_graphicsDevice.Viewport.Width / 2, _graphicsDevice.Viewport.Height / 2) +
                           new Point(0, 100),
                Text = "Quit Game",
                HoverColor = Color.Red,
                Scale = new Vector2(2.5f, 2.5f)
            };
            quitGameButton.Click += QuitGameButton_Click;
            quitGameButton.Position = quitGameButton.Position + new Point(-(quitGameButton.Rectangle.Width / 2), 0);

            #endregion

            // add buttons to list of components
            _components = new List<Component>()
            {
                newGameButton,
                loadGameButton,
                quitGameButton
            };

            // set mouse position
            Mouse.SetPosition(_graphicsDevice.Viewport.Width / 2, _graphicsDevice.Viewport.Height / 2);
        }

        #region BUTTON CLICK METHODS

        private void QuitGameButton_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Quitting game...");
            _game.Exit();
        }

        private void LoadGameButton_Click(object sender, EventArgs e)
        {
            // todo load game
            Console.WriteLine("Loading game...");
            _game.ChangeState(new GameState(_game, _graphicsDevice, _content, false));
            // load previous game
        }

        private void NewGameButton_Click(object sender, EventArgs e)
        {
            // todo new game
            Console.WriteLine("Starting new game...");
            _game.ChangeState(new GameState(_game, _graphicsDevice, _content, true));
            // load new game
        }

        #endregion

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Point msp = Mouse.GetState().Position;
            Vector2 mp = new Vector2(msp.X, msp.Y);

            spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            // draw background
            spriteBatch.Draw(_backgroundTexture, new Vector2(0, 0), Color.LightBlue);

            // draw each component
            foreach (Component component in _components)
                component.Draw(gameTime, spriteBatch);

            spriteBatch.Draw(_cursorTexture, mp, Color.White);

            spriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            // update each component
            foreach (Component component in _components)
                component.Update(gameTime, null);
        }
    }
}
