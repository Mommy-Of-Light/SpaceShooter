namespace SpaceShooter
{
    public class ScreenManager
    {
        private Game1 _game;
        private GameScreen _currentScreen;
        public GameScreen CurrentScreen => _currentScreen;

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
        }

        public void ReturnToScreen(GameScreen screen)
        {
            if (_currentScreen != null)
                _currentScreen.OnExit();

            _currentScreen = screen;

            if (_currentScreen != null)
                _currentScreen.OnEnter();
        }

        public void Update(GameTime gameTime, KeyboardState keyboard, Vector2 mousePosition, bool mouseClicked)
        {
            _currentScreen?.Update(gameTime, keyboard, mousePosition, mouseClicked);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            _currentScreen?.Draw(gameTime, spriteBatch);
        }
    }
}