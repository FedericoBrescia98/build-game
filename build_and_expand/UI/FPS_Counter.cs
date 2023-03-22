using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace build_and_expand.UI
{
    public class FPS_Counter
    {
        private SpriteFont _font;
        private float _fps = 0;
        private float _totalTime;
        private float _displayFPS;

        public FPS_Counter()
        {
            this._totalTime = 0f;
            this._displayFPS = 0f;
        }

        public void LoadContent(ContentManager content)
        {
            this._font = content.Load<SpriteFont>("Fonts/slkscr");
        }

        public void Draw(GameTime gameTime, SpriteBatch batch)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            _totalTime += elapsed;

            if(_totalTime >= 1000)
            {
                _displayFPS = _fps;
                _fps = 0;
                _totalTime = 0;
            }

            _fps++;

            batch.Begin();
            batch.DrawString(this._font, this._displayFPS.ToString() + " FPS", new Vector2(10, 10), Color.White);
            batch.End();
        }
    }
}
