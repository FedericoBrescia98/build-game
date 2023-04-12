using build_and_expand.States;
using build_and_expand.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Threading.Tasks;
using Color = Microsoft.Xna.Framework.Color;
using Point = Microsoft.Xna.Framework.Point;

namespace build_and_expand
{
    public partial class Game : Microsoft.Xna.Framework.Game
    {
        private FPS_Counter fpsCounter;
        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private State _currentState;
        private State _nextState;
        private MouseState _currentMouseState;
        private MouseState _previousMouseState;
        private SoundEffect ClickSound;

        public Game()
        {
            _graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = (int)C.DISPLAYDIM.X,
                PreferredBackBufferHeight = (int)C.DISPLAYDIM.Y
            };
            Content.RootDirectory = "Content";
            IsMouseVisible = false;
        }

        protected override void Initialize()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _currentState = new SplashScreenState(this, GraphicsDevice, Content);
            fpsCounter = new FPS_Counter();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            fpsCounter.LoadContent(Content);
            ClickSound = Content.Load<SoundEffect>("Sounds/FX/Click");
        }

        protected override void Update(GameTime gameTime)
        {
            // manage inputs only if game has focus
            if (IsActive)
            {
                _previousMouseState = _currentMouseState;
                _currentMouseState = Mouse.GetState();

                // make click sound
                if (_currentMouseState.LeftButton == ButtonState.Released &&
                    _previousMouseState.LeftButton == ButtonState.Pressed)
                {
                    if (IsMouseInsideWindow())
                    {
                        bool makeSound = true;
                        if (_currentState is GameState s)
                        {
                            if (!s.IsLoaded)
                            {
                                makeSound = false;
                            }
                        }

                        if (makeSound.Equals(true))
                        {
                            ClickSound.Play(0.2f, -0.3f, 0.0f);
                        }
                    }
                }

                // ESC will open the menu
                if (_currentState is not MenuState)
                {
                    if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                    {
                        if (_currentState is GameState state)
                        {
                            Task.Run(() => state.SaveGame());
                        }

                        _nextState = new MenuState(this, GraphicsDevice, Content);
                    }
                }
            }


            // next state logic
            if (_nextState != null)
            {
                _currentState = _nextState;
                _nextState = null;
            }

            _currentState.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            // draw only if game has focus
            if (IsActive)
            {
                GraphicsDevice.Clear(Color.CornflowerBlue);
                _currentState.Draw(gameTime, _spriteBatch);
                fpsCounter.Draw(gameTime, _spriteBatch);
            }

            base.Draw(gameTime);
        }

        private bool IsMouseInsideWindow()
        {
            MouseState mouseState = Mouse.GetState();
            Point mousePos = new Point(mouseState.X, mouseState.Y);
            return GraphicsDevice.Viewport.Bounds.Contains(mousePos);
        }

        public void ChangeState(State state)
        {
            _nextState = state;
        }
    }
}
