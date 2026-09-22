namespace SpaceShooter
{
    public class DeathScreen : GameScreen
    {
        private SpriteFont _font;
        private SpriteFont _titleFont;
        private Button _restartButton;
        private Button _exitButton;
        private KeyboardState _previousKeyboard;
        private int _wave;
        private int _score;
        private bool _scoreSaved;

        public DeathScreen(Game1 game, int wave, int score) : base(game)
        {
            _wave = wave;
            _score = score;
            _previousKeyboard = Keyboard.GetState();
            _scoreSaved = false;
            Game.ChangeScreenSize(500, 500);
        }

        public override void Initialize()
        {
            _font = Game.Content.Load<SpriteFont>("Fonts/SpaceInvader_12");
            _titleFont = Game.Content.Load<SpriteFont>("Fonts/SpaceInvader_16");

            _restartButton = new Button("Restart", new XnaRectangle((Game.GraphicsDevice.Viewport.Width - 200) / 2, 300, 200, 50));
            _exitButton = new Button("Exit", new XnaRectangle((Game.GraphicsDevice.Viewport.Width - 200) / 2, 380, 200, 50));

            SaveScore();
        }

        private void SaveScore()
        {
            if (_scoreSaved)
                return;

            _scoreSaved = true;

            MariaDbManager.SaveScore(Game.PlayerPseudo, _score, Game.Difficulty);
        }

        public override void Update(GameTime gameTime, KeyboardState keyboard, Vector2 mousePosition, bool mouseClicked)
        {
            _restartButton.Update(mousePosition);
            _exitButton.Update(mousePosition);

            if (_restartButton.IsClicked(mousePosition, mouseClicked))
            {
                Game.ScreenManager.ChangeScreen(new PlayScreen(Game));
                return;
            }

            if (_exitButton.IsClicked(mousePosition, mouseClicked))
            {
                Game.ScreenManager.ChangeScreen(new NewGameScreen(Game));
                return;
            }

            if (keyboard.IsKeyDown(XnaKeys.Escape) && _previousKeyboard.IsKeyUp(XnaKeys.Escape))
            {
                Game.ScreenManager.ChangeScreen(new NewGameScreen(Game));
                return;
            }

            _previousKeyboard = keyboard;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            Texture2D background = Game.Content.Load<Texture2D>("Textures/Background/black");
            spriteBatch.Draw(background, Vector2.Zero, XnaColor.White);

            string title = "GAME OVER";
            Vector2 titleSize = _titleFont.MeasureString(title);

            spriteBatch.DrawString(_titleFont, title, new Vector2((Game.GraphicsDevice.Viewport.Width - titleSize.X) / 2f, 70), XnaColor.White);

            string pseudoText = "Player: " + Game.PlayerPseudo;
            Vector2 pseudoSize = _font.MeasureString(pseudoText);

            spriteBatch.DrawString(_font, pseudoText, new Vector2((Game.GraphicsDevice.Viewport.Width - pseudoSize.X) / 2f, 140), XnaColor.White);

            string waveText = "Wave " + _wave;
            Vector2 waveSize = _font.MeasureString(waveText);

            spriteBatch.DrawString(_font, waveText, new Vector2((Game.GraphicsDevice.Viewport.Width - waveSize.X) / 2f, 180), XnaColor.White);

            string scoreText = "Score " + _score;
            Vector2 scoreSize = _font.MeasureString(scoreText);

            spriteBatch.DrawString(_font, scoreText, new Vector2((Game.GraphicsDevice.Viewport.Width - scoreSize.X) / 2f, 220), XnaColor.White);

            _restartButton.Draw(spriteBatch, _font);
            _exitButton.Draw(spriteBatch, _font);

            spriteBatch.End();
        }
    }
}