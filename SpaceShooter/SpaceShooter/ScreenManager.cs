namespace SpaceShooter
{
    public class ScreenManager
    {
        private Game1 _game;
        private GameScreen _currentScreen;

        public GameScreen CurrentScreen => _currentScreen;

        private bool _waitForMouseRelease;

        public ScreenManager(Game1 game)
        {
            _game = game;
        }

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

            _waitForMouseRelease = true;
        }

        public void Update(
            GameTime gameTime,
            KeyboardState keyboard,
            Vector2 mousePosition,
            bool mouseClicked)
        {
            _currentScreen.Update(
                gameTime,
                keyboard,
                mousePosition,
                mouseClicked
            );
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            _currentScreen?.Draw(gameTime, spriteBatch);
        }
    }
}