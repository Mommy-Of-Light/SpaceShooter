namespace SpaceShooter
{
    /// <summary>
    /// Represents the base class for all game screens.
    /// Provides common lifecycle methods for initialization, updating, drawing,
    /// and entering or exiting a screen.
    /// </summary>
    public abstract class GameScreen
    {
        /// <summary>
        /// Reference to the main game instance.
        /// </summary>
        protected Game1 Game;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameScreen"/> class.
        /// </summary>
        /// <param name="game">The main game instance associated with this screen.</param>
        public GameScreen(Game1 game)
        {
            Game = game;
        }

        /// <summary>
        /// Initializes the game screen.
        /// </summary>
        public virtual void Initialize() { }

        /// <summary>
        /// Updates the game screen based on the current input and game state.
        /// </summary>
        /// <param name="gameTime">Provides timing information for the update.</param>
        /// <param name="keyboard">The current keyboard state.</param>
        /// <param name="mousePosition">The current position of the mouse.</param>
        /// <param name="mouseClicked">Indicates whether the mouse was clicked during this update.</param>
        public virtual void Update(GameTime gameTime, KeyboardState keyboard, Vector2 mousePosition, bool mouseClicked) { }

        /// <summary>
        /// Draws the contents of the game screen.
        /// </summary>
        /// <param name="gameTime">Provides timing information for the draw operation.</param>
        /// <param name="spriteBatch">The sprite batch used to draw the screen.</param>
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch) { }

        /// <summary>
        /// Called when the game screen becomes the active screen.
        /// </summary>
        public virtual void OnEnter() { }

        /// <summary>
        /// Called when the game screen is no longer the active screen.
        /// </summary>
        public virtual void OnExit() { }
    }
}