using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace build_and_expand.UI
{
    public class AnimatedTexture
    {
        public Texture2D AnimTexture { get; set; }
        public float AnimTime { get; set; } = 0.0f;
        public float AnimFrameTime = 0.1f;
        public int AnimFrameIndex = 0;
        public Point BaseTextureDim { get; set; } = C.TILETEXTURESIZE;

        private int _animFrameColumns;
        private int _animFrameCount;
        private int _animFrameRowIdx = 0;
        private int _animFrameColIdx = 0;

        public Color DrawColor { get; set; } = Color.White;
        public Point Position { get; set; } = Point.Zero;

        public bool Loop { get; set; } = false;
        public bool Finished { get; set; } = false;

        public AnimatedTexture(Texture2D anim_Texture, Point position)
        {
            _animFrameColumns = anim_Texture.Width / BaseTextureDim.X;
            _animFrameCount = (anim_Texture.Width / BaseTextureDim.X) 
                                        * (anim_Texture.Height / BaseTextureDim.Y);
            AnimTexture = anim_Texture;
            Position = position;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if(Finished.Equals(false))
            {
                AnimTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if(AnimTime >= AnimFrameTime)
                {
                    AnimTime -= AnimFrameTime;
                    AnimFrameIndex = (AnimFrameIndex + 1) % _animFrameCount;
                    _animFrameColIdx = (AnimFrameIndex % _animFrameColumns);
                    _animFrameRowIdx = (AnimFrameIndex / _animFrameColumns);
                }

                spriteBatch.Draw(
                            AnimTexture,
                            sourceRectangle:
                                new Rectangle(_animFrameColIdx * BaseTextureDim.X,
                                                _animFrameRowIdx * BaseTextureDim.Y,
                                                BaseTextureDim.X,
                                                BaseTextureDim.Y),
                            position: Position.ToVector2(),
                            color: DrawColor);
            }

            if((AnimFrameIndex == _animFrameCount - 1) && Loop.Equals(false))
            {
                Finished = true;
                AnimFrameIndex = 0;
                AnimFrameTime = 0;
            }
        }
    }
}
