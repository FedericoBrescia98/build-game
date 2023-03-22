using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace build_and_expand.UI
{
    public class Camera
    {
        private Matrix _transform;
        private Vector2 _pos;
        private float _zoom;
        private float _maxZoom = 2f;
        private float _minZoom = 0.8f;
        private MouseState _currentMouse;
        private KeyboardState _keyboardState;
        private int _previousScrollValue;
        private GraphicsDevice _graphicsDevice;

        public Camera(GraphicsDevice graphicsDevice)
        {
            _zoom = 1.0f;
            _pos = new Vector2(C.DISPLAYDIM.X / 2, C.DISPLAYDIM.Y / 2);
            _graphicsDevice = graphicsDevice;
        }

        public Vector2 Pos => _pos;
        public float Zoom => _zoom;

        public void Update()
        {
            _currentMouse = Mouse.GetState();
            _keyboardState = Keyboard.GetState();

            // keyboard moving map logic
            if(_keyboardState.IsKeyDown(Keys.A))
            {
                _pos += new Vector2(-5, 0);
                if(_pos.X < 0)
                {
                    _pos = new Vector2(0, _pos.Y);
                }
            }
            if(_keyboardState.IsKeyDown(Keys.D))
            {
                _pos += new Vector2(5, 0);
                if(_pos.X > C.DISPLAYDIM.X)
                {
                    _pos = new Vector2(C.DISPLAYDIM.X, _pos.Y);
                }
            }
            if(_keyboardState.IsKeyDown(Keys.W))
            {
                _pos += new Vector2(0, -5);
                if(_pos.Y < 0)
                {
                    _pos = new Vector2(_pos.X, 0);
                }
            }
            if(_keyboardState.IsKeyDown(Keys.S))
            {
                _pos += new Vector2(0, 5);
                if(_pos.Y > C.DISPLAYDIM.Y)
                {
                    _pos = new Vector2(_pos.X, C.DISPLAYDIM.Y);
                }
            }

            // // mouse edge moving map logic
            //if (_currentMouse.X < 10)
            //{
            //    _pos += new Vector2(-5, 0);
            //    if (_pos.X < 0)
            //    {
            //        _pos = new Vector2(0, _pos.Y);
            //    }
            //}
            //if (_currentMouse.X > C.DISPLAYDIM.X - 10)
            //{
            //    _pos += new Vector2(5, 0);
            //    if (_pos.X > C.DISPLAYDIM.X)
            //    {
            //        _pos = new Vector2(C.DISPLAYDIM.X, _pos.Y);
            //    }
            //}
            //if (_currentMouse.Y < 10)
            //{
            //    _pos += new Vector2(0, -5);
            //    if (_pos.Y < 0)
            //    {
            //        _pos = new Vector2(_pos.X, 0);
            //    }
            //}
            //if (_currentMouse.Y > C.DISPLAYDIM.Y - 10)
            //{
            //    _pos += new Vector2(0, 5);
            //    if (_pos.Y > C.DISPLAYDIM.Y)
            //    {
            //        _pos = new Vector2(_pos.X, C.DISPLAYDIM.Y);
            //    }
            //}

            // zoom logic
            if(_currentMouse.ScrollWheelValue > _previousScrollValue)
            {
                _zoom += 0.04f;
                if(_zoom > _maxZoom)
                {
                    _zoom = _maxZoom;
                }
            }
            if(_currentMouse.ScrollWheelValue < _previousScrollValue)
            {
                _zoom += -0.04f;
                if(_zoom < _minZoom)
                {
                    _zoom = _minZoom;
                }
            }
            _previousScrollValue = _currentMouse.ScrollWheelValue;
        }

        public Matrix getTransformation()
        {
            _transform =
              Matrix.CreateTranslation(new Vector3(-_pos.X, -_pos.Y, 0)) *
                         Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
                         Matrix.CreateTranslation(new Vector3(_graphicsDevice.Viewport.Width * 0.5f, _graphicsDevice.Viewport.Height * 0.5f, 0));
            return _transform;
        }
    }
}
