namespace SpaceShooter
{
    public class RankingScreen : GameScreen
    {
        private SpriteFont _font;
        private SpriteFont _font_title;

        private Button _returnButton;
        private Button _easyButton;
        private Button _normalButton;
        private Button _hardButton;
        private Button _allButton;

        private KeyboardState _previousKeyboard;
        private List<ScoreData> _scores;
        private List<ScoreData> _filteredScores;

        private int _scrollIndex;
        private int _visibleScores = 7;

        public RankingScreen(Game1 game) : base(game)
        {
            _previousKeyboard = Keyboard.GetState();
            Game.ChangeScreenSize(500, 500);
        }

        public override void Initialize()
        {
            _font = Game.Content.Load<SpriteFont>("Fonts/SpaceInvader_12");
            _font_title = Game.Content.Load<SpriteFont>("Fonts/SpaceInvader_16");

            _returnButton = new Button("Return", new XnaRectangle(100, 430, 300, 45));
            _allButton = new Button("All", new XnaRectangle(20, 75, 100, 35));
            _easyButton = new Button("Easy", new XnaRectangle(140, 75, 100, 35));
            _normalButton = new Button("Medium", new XnaRectangle(260, 75, 100, 35));
            _hardButton = new Button("Hard", new XnaRectangle(380, 75, 100, 35));

            _scores = MariaDbManager.GetScores();
            _filteredScores = _scores.ToList();

            _scrollIndex = 0;

            _allButton.SetSelected(true);
            _easyButton.SetSelected(false);
            _normalButton.SetSelected(false);
            _hardButton.SetSelected(false);
        }

        private void SortByDifficulty(string difficulty)
        {
            _filteredScores = _scores
                .Where(score => string.Equals(score.Difficulty, difficulty, StringComparison.OrdinalIgnoreCase))
                .ToList();

            _scrollIndex = 0;
        }

        private void ScrollUp()
        {
            if (_scrollIndex <= 0)
                return;

            _scrollIndex--;
        }

        private void ScrollDown()
        {
            if (_scrollIndex + _visibleScores >= _filteredScores.Count)
                return;

            _scrollIndex++;
        }

        public override void Update(GameTime gameTime, KeyboardState keyboard, Vector2 mousePosition, bool mouseClicked)
        {
            _returnButton.Update(mousePosition);
            _easyButton.Update(mousePosition);
            _normalButton.Update(mousePosition);
            _hardButton.Update(mousePosition);
            _allButton.Update(mousePosition);

            if (_allButton.IsClicked(mousePosition, mouseClicked) || (keyboard.IsKeyDown(XnaKeys.D1) && _previousKeyboard.IsKeyUp(XnaKeys.D1)))
            {
                _filteredScores = _scores.ToList();
                _scrollIndex = 0;

                _allButton.SetSelected(true);
                _easyButton.SetSelected(false);
                _normalButton.SetSelected(false);
                _hardButton.SetSelected(false);
            }

            if (_returnButton.IsClicked(mousePosition, mouseClicked))
            {
                Game.ScreenManager.ChangeScreen(new MenuScreen(Game));
                return;
            }

            if (_easyButton.IsClicked(mousePosition, mouseClicked) || (keyboard.IsKeyDown(XnaKeys.D2) && _previousKeyboard.IsKeyUp(XnaKeys.D2)))
            {
                SortByDifficulty("Easy");

                _allButton.SetSelected(false);
                _easyButton.SetSelected(true);
                _normalButton.SetSelected(false);
                _hardButton.SetSelected(false);
            }

            if (_normalButton.IsClicked(mousePosition, mouseClicked) || (keyboard.IsKeyDown(XnaKeys.D3) && _previousKeyboard.IsKeyUp(XnaKeys.D3)))
            {
                SortByDifficulty("Medium");

                _allButton.SetSelected(false);
                _easyButton.SetSelected(false);
                _normalButton.SetSelected(true);
                _hardButton.SetSelected(false);
            }

            if (_hardButton.IsClicked(mousePosition, mouseClicked) || (keyboard.IsKeyDown(XnaKeys.D4) && _previousKeyboard.IsKeyUp(XnaKeys.D4)))
            {
                SortByDifficulty("Hard");

                _allButton.SetSelected(false);
                _easyButton.SetSelected(false);
                _normalButton.SetSelected(false);
                _hardButton.SetSelected(true);
            }

            if (keyboard.IsKeyDown(XnaKeys.Up) && _previousKeyboard.IsKeyUp(XnaKeys.Up))
            {
                ScrollUp();
            }

            if (keyboard.IsKeyDown(XnaKeys.Down) && _previousKeyboard.IsKeyUp(XnaKeys.Down))
            {
                ScrollDown();
            }

            if (keyboard.IsKeyDown(XnaKeys.Escape) && _previousKeyboard.IsKeyUp(XnaKeys.Escape))
            {
                Game.ScreenManager.ChangeScreen(new MenuScreen(Game));
                return;
            }

            _previousKeyboard = keyboard;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            Texture2D background = Game.Content.Load<Texture2D>("Textures/Background/black");
            spriteBatch.Draw(background, Vector2.Zero, XnaColor.White);

            string title = "RANKING";
            Vector2 titleSize = _font_title.MeasureString(title);

            spriteBatch.DrawString(_font_title, title, new Vector2((Game.GraphicsDevice.Viewport.Width - titleSize.X) / 2f, 30), XnaColor.White);

            _easyButton.Draw(spriteBatch, _font);
            _normalButton.Draw(spriteBatch, _font);
            _hardButton.Draw(spriteBatch, _font);
            _allButton.Draw(spriteBatch, _font);

            if (!string.IsNullOrEmpty(MariaDbManager.LastError))
            {
                string error = "DATABASE ERROR";
                Vector2 errorSize = _font.MeasureString(error);

                spriteBatch.DrawString(_font, error, new Vector2((Game.GraphicsDevice.Viewport.Width - errorSize.X) / 2f, 150), XnaColor.White);
            }
            else if (_filteredScores.Count == 0)
            {
                string noScores = "NO SCORES";
                Vector2 noScoresSize = _font.MeasureString(noScores);

                spriteBatch.DrawString(_font, noScores, new Vector2((Game.GraphicsDevice.Viewport.Width - noScoresSize.X) / 2f, 180), XnaColor.White);
            }
            else
            {
                int endIndex = Math.Min(_scrollIndex + _visibleScores, _filteredScores.Count);

                for (int i = _scrollIndex; i < endIndex; i++)
                {
                    ScoreData score = _filteredScores[i];

                    string pseudo = score.Pseudo;

                    if (pseudo.Length > 12)
                        pseudo = pseudo.Substring(0, 12);

                    int displayIndex = i - _scrollIndex;

                    string line = (i + 1) + ". " + pseudo + "    " + score.Score + "    " + score.Difficulty;

                    spriteBatch.DrawString(_font, line, new Vector2(40, 120 + displayIndex * 38), XnaColor.White);
                }

                if (_scrollIndex > 0)
                {
                    spriteBatch.DrawString(_font, "UP", new Vector2(10, 20), XnaColor.White);
                }

                if (_scrollIndex + _visibleScores < _filteredScores.Count)
                {
                    spriteBatch.DrawString(_font, "DOWN", new Vector2(10, 35), XnaColor.White);
                }

                string counter = (_scrollIndex + 1) + "-" + Math.Min(_scrollIndex + _visibleScores, _filteredScores.Count) + " / " + _filteredScores.Count;
                Vector2 counterSize = _font.MeasureString(counter);

                spriteBatch.DrawString(_font, counter, new Vector2((Game.GraphicsDevice.Viewport.Width - counterSize.X) / 2f, 405), XnaColor.White);
            }

            _returnButton.Draw(spriteBatch, _font);

            spriteBatch.End();
        }
    }
}