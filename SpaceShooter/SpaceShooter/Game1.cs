namespace SpaceShooter
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public ScreenManager ScreenManager { get; private set; }

        private Texture2D _cursorTexture;
        private Vector2 _cursorPosition;
        private XnaPoint _windowCenter;

        private float _mouseSensitivity = 3.0f;

        private bool _previousLeftMouseButton;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _graphics.PreferredBackBufferWidth = WINDOW_WIDTH;
            _graphics.PreferredBackBufferHeight = WINDOW_HEIGHT;
            Window.Title = WINDOW_TITLE;

            ScreenManager = new ScreenManager(this);
        }

        public void ChangeScreenSize(int width, int height)
        {
            _graphics.PreferredBackBufferWidth = width;
            _graphics.PreferredBackBufferHeight = height;
            _graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            ScreenManager.ChangeScreen(new MenuScreen(this));

            IsMouseVisible = false;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _cursorTexture = Content.Load<Texture2D>(
                "Textures/PNG/UI/cursor"
            );

            IsMouseVisible = false;

            _cursorPosition = new Vector2(
                _graphics.PreferredBackBufferWidth / 2f,
                _graphics.PreferredBackBufferHeight / 2f
            );

            CenterMouse();
        }

        private void CenterMouse()
        {
            _windowCenter = new XnaPoint(
                _graphics.PreferredBackBufferWidth / 2,
                _graphics.PreferredBackBufferHeight / 2
            );

            Mouse.SetPosition(
                _windowCenter.X,
                _windowCenter.Y
            );
        }

        protected override void Update(GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState();

            int deltaX = mouseState.X - _windowCenter.X;
            int deltaY = mouseState.Y - _windowCenter.Y;

            // Move virtual cursor
            _cursorPosition.X += deltaX * _mouseSensitivity;
            _cursorPosition.Y += deltaY * _mouseSensitivity;

            _cursorPosition.X = Math.Clamp(
                _cursorPosition.X,
                0,
                _graphics.PreferredBackBufferWidth - _cursorTexture.Width
            );

            _cursorPosition.Y = Math.Clamp(
                _cursorPosition.Y,
                0,
                _graphics.PreferredBackBufferHeight - _cursorTexture.Height
            );

            bool mouseClicked =
                mouseState.LeftButton == XnaButtonState.Pressed &&
                !_previousLeftMouseButton;

            _previousLeftMouseButton =
                mouseState.LeftButton == XnaButtonState.Pressed;

            ScreenManager.Update(
                gameTime,
                Keyboard.GetState(),
                _cursorPosition,
                mouseClicked
            );

            Mouse.SetPosition(
                _windowCenter.X,
                _windowCenter.Y
            );

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(XnaColor.Black);

            ScreenManager.Draw(gameTime, _spriteBatch);

            _spriteBatch.Begin();

            _spriteBatch.Draw(
                _cursorTexture,
                _cursorPosition,
                XnaColor.White
            );

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}