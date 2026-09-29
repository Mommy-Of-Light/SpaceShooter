namespace SpaceShooter
{
    /// <summary>
    /// Manages the current game screen and handles switching, updating, and drawing screens.
    /// </summary>
    public class ScreenManager
    {
        /// <summary>
        /// Reference to the main game instance.
        /// </summary>
        private Game1 _game;

        /// <summary>
        /// Screen currently displayed by the game.
        /// </summary>
        private GameScreen _currentScreen;

        /// <summary>
        /// Gets the screen currently managed and displayed by the game.
        /// </summary>
        public GameScreen CurrentScreen => _currentScreen;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScreenManager"/> class.
        /// </summary>
        /// <param name="game">Reference to the main game instance.</param>
        public ScreenManager(Game1 game)
        {
            _game = game;
        }

        /// <summary>
        /// Changes the current screen and initializes the new screen.
        /// </summary>
        /// <param name="newScreen">Screen that should become the current screen.</param>
        public void ChangeScreen(GameScreen newScreen)
        {
            if (_currentScreen != null)
                _currentScreen.OnExit();

            _currentScreen = newScreen;

            if (_currentScreen != null)
            {
                _currentScreen.Initialize();
                _currentScreen.OnEnter();
            }
        }

        /// <summary>
        /// Returns to an already initialized screen without initializing it again.
        /// </summary>
        /// <param name="screen">Screen to return to.</param>
        public void ReturnToScreen(GameScreen screen)
        {
            if (_currentScreen != null)
                _currentScreen.OnExit();

            _currentScreen = screen;

            if (_currentScreen != null)
                _currentScreen.OnEnter();
        }

        /// <summary>
        /// Updates the current screen.
        /// </summary>
        /// <param name="gameTime">Provides timing information for the current update.</param>
        /// <param name="keyboard">Current keyboard state.</param>
        /// <param name="mousePosition">Current position of the mouse cursor.</param>
        /// <param name="mouseClicked">Indicates whether the mouse button was clicked during this update.</param>
        public void Update(GameTime gameTime, KeyboardState keyboard, Vector2 mousePosition, bool mouseClicked)
        {
            _currentScreen?.Update(gameTime, keyboard, mousePosition, mouseClicked);
        }

        /// <summary>
        /// Draws the current screen.
        /// </summary>
        /// <param name="gameTime">Provides timing information for the current game frame.</param>
        /// <param name="spriteBatch">The sprite batch used to draw the current screen.</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            _currentScreen?.Draw(gameTime, spriteBatch);
        }
    }
}