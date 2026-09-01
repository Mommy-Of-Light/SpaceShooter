using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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

        public void Update(GameTime gameTime)
        {
            MouseState mouse = Mouse.GetState();

            if (_waitForMouseRelease)
            {
                if (mouse.LeftButton == ButtonState.Released)
                {
                    _waitForMouseRelease = false;
                }

                return;
            }

            _currentScreen?.Update(gameTime);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            _currentScreen?.Draw(gameTime, spriteBatch);
        }
    }
}