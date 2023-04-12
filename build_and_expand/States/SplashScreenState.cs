using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace build_and_expand.States
{
    public class SplashScreenState : State
    {
        // set splash-image sprite and sprite player
        private readonly Texture2D _splashTexture;
        private readonly Song _glimmerSound;
        private readonly Song _song;
        private readonly Texture2D _cursorTexture;

        // set animation-playback countdown (till end)
        private int _countdown = 220;

        // construct state
        public SplashScreenState(Game game, GraphicsDevice graphicsDevice, ContentManager content) : base(game,
            graphicsDevice, content)
        {
            _splashTexture = content.Load<Texture2D>("Sprites/SplashScreen/SplashScreen");
            _glimmerSound = content.Load<Song>("Sounds/FX/Glimmer");
            _cursorTexture = _content.Load<Texture2D>("Sprites/UI/cursor");
            _song = _content.Load<Song>("Sounds/Music/Bgm");
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Point msp = Mouse.GetState().Position;
            Vector2 mp = new Vector2(msp.X, msp.Y);

            // clear screen to black
            _graphicsDevice.Clear(Color.Wheat);

            // begin spriteBatch for state
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            spriteBatch.Draw(_splashTexture, new Vector2(0, 0), Color.White);
            spriteBatch.Draw(_cursorTexture, mp, Color.White);
            spriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            if (_countdown > 0)
            {
                // subtract countdown
                if (_countdown.Equals(100))
                {
                    MediaPlayer.Play(_glimmerSound);
                }

                _countdown--;
            }

            if (_countdown.Equals(0))
            {
                // change to main menu at end of countdown
                _game.ChangeState(new MenuState(_game, _graphicsDevice, _content));

                // game music
                MediaPlayer.Play(_song);
                MediaPlayer.Volume = 0.2f;
                MediaPlayer.IsRepeating = true;
            }
        }
    }
}
