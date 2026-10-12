namespace SpaceShooter
{
    /// <summary>
    /// Represents the main game class and manages the game's graphics,
    /// input, audio, screen transitions, and update and draw cycles.
    /// </summary>
    public class Game1 : Game
    {
        /// <summary>
        /// Manages the game's graphics device and display settings.
        /// </summary>
        private GraphicsDeviceManager _graphics;

        /// <summary>
        /// Handles drawing sprites to the screen.
        /// </summary>
        private SpriteBatch _spriteBatch;

        /// <summary>
        /// Manages the different screens of the game.
        /// </summary>
        public ScreenManager ScreenManager { get; private set; }

        /// <summary>
        /// Texture used to display the custom mouse cursor.
        /// </summary>
        private Texture2D _cursorTexture;

        /// <summary>
        /// Current position of the custom mouse cursor.
        /// </summary>
        private Vector2 _cursorPosition;

        /// <summary>
        /// Stores whether the left mouse button was pressed during the previous update.
        /// </summary>
        private bool _previousLeftMouseButton;

        /// <summary>
        /// Gets or sets the player's pseudo.
        /// </summary>
        public string PlayerPseudo { get; set; }

        /// <summary>
        /// Gets or sets the selected game difficulty.
        /// </summary>
        public string Difficulty { get; set; }

        /// <summary>
        /// Gets or sets the multiplier applied to the selected difficulty.
        /// </summary>
        public double DifficultyMultiplier { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Game1"/> class.
        /// Configures the graphics settings, game window, default difficulty,
        /// and screen manager.
        /// </summary>
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = false;
            _graphics.PreferredBackBufferWidth = WINDOW_WIDTH;
            _graphics.PreferredBackBufferHeight = WINDOW_HEIGHT;
            Window.Title = WINDOW_TITLE;
            PlayerPseudo = "";
            Difficulty = "Medium";
            DifficultyMultiplier = 1.0;
            ScreenManager = new ScreenManager(this);
        }

        /// <summary>
        /// Changes the size of the game window.
        /// </summary>
        /// <param name="width">The new width of the game window.</param>
        /// <param name="height">The new height of the game window.</param>
        public void ChangeScreenSize(int width, int height)
        {
            _graphics.PreferredBackBufferWidth = width;
            _graphics.PreferredBackBufferHeight = height;
            _graphics.ApplyChanges();
        }

        /// <summary>
        /// Initializes the game and displays the pseudo selection screen.
        /// </summary>
        protected override void Initialize()
        {
            ScreenManager.ChangeScreen(new PseudoScreen(this));
            IsMouseVisible = false;
            base.Initialize();
        }

        /// <summary>
        /// Sets the window icon using the provided texture.
        /// </summary>
        /// <param name="texture">The texture to use as the window icon.</param>
        private void SetWindowIcon(Texture2D texture)
        {
            XnaColor[] pixels = new XnaColor[texture.Width * texture.Height];
            texture.GetData(pixels);

            using var bitmap = new System.Drawing.Bitmap(
                texture.Width,
                texture.Height,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb
            );

            for (int y = 0; y < texture.Height; y++)
            {
                for (int x = 0; x < texture.Width; x++)
                {
                    XnaColor c = pixels[y * texture.Width + x];

                    bitmap.SetPixel(
                        x, y,
                        System.Drawing.Color.FromArgb(c.A, c.R, c.G, c.B)
                    );
                }
            }

            IntPtr handle = bitmap.GetHicon();
            var icon = System.Drawing.Icon.FromHandle(handle);

            var form = (System.Windows.Forms.Form)
                System.Windows.Forms.Form.FromHandle(Window.Handle);

            if (form != null)
                form.Icon = (System.Drawing.Icon)icon.Clone();

            DestroyIcon(handle);
        }

        /// <summary>
        /// Destroys an icon handle to free up system resources.
        /// </summary>
        /// <param name="hIcon">The handle of the icon to destroy.</param>
        /// <returns></returns>
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool DestroyIcon(IntPtr hIcon);

        /// <summary>
        /// Loads the game's graphical and audio content and initializes
        /// the custom cursor and audio systems.
        /// </summary>
        protected override void LoadContent()
        {
            Texture2D iconTexture = Content.Load<Texture2D>("Icons/spaceinvaders_128_icon");
            SetWindowIcon(iconTexture);

            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _cursorTexture = Content.Load<Texture2D>("Textures/PNG/UI/cursor");
            IsMouseVisible = false;

            _cursorPosition = new Vector2(_graphics.PreferredBackBufferWidth / 2f, _graphics.PreferredBackBufferHeight / 2f);

            SoundEffectPlayer.Instance.Initialize(this);
            MusicPlayer.Instance.Initialize(this);
            MusicPlayer.Instance.Play("Menu");

            Mouse.SetPosition((int)_cursorPosition.X, (int)_cursorPosition.Y);
        }

        /// <summary>
        /// Updates the game state, processes mouse input, updates the active screen,
        /// and handles mouse button clicks.
        /// </summary>
        /// <param name="gameTime">Provides timing information for the update.</param>
        protected override void Update(GameTime gameTime)
        {
            MusicPlayer.Instance.Update();

            MouseState mouseState = Mouse.GetState();

            int maxX = _graphics.PreferredBackBufferWidth - _cursorTexture.Width;
            int maxY = _graphics.PreferredBackBufferHeight - _cursorTexture.Height;

            int mouseX = mouseState.X;
            int mouseY = mouseState.Y;

            if (mouseX >= 0 && mouseX <= maxX && mouseY >= 0 && mouseY <= maxY)
            {
                _cursorPosition = new Vector2(mouseX, mouseY);
            }
            else
            {
                _cursorPosition.X = Math.Clamp(_cursorPosition.X, 0, maxX);
                _cursorPosition.Y = Math.Clamp(_cursorPosition.Y, 0, maxY);

                Mouse.SetPosition((int)_cursorPosition.X, (int)_cursorPosition.Y);
            }

            bool mouseClicked = mouseState.LeftButton == XnaButtonState.Pressed && !_previousLeftMouseButton;

            _previousLeftMouseButton = mouseState.LeftButton == XnaButtonState.Pressed;

            ScreenManager.Update(gameTime, Keyboard.GetState(), _cursorPosition, mouseClicked);

            base.Update(gameTime);
        }

        /// <summary>
        /// Draws the current game screen and the custom mouse cursor.
        /// </summary>
        /// <param name="gameTime">Provides timing information for the draw operation.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(XnaColor.Black);

            ScreenManager.Draw(gameTime, _spriteBatch);

            _spriteBatch.Begin();
            _spriteBatch.Draw(_cursorTexture, _cursorPosition, XnaColor.White);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}