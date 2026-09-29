namespace SpaceShooter
{
    /// <summary>
    /// Represents the screen used to display and filter the game's ranking scores.
    /// </summary>
    public class RankingScreen : GameScreen
    {
        /// <summary>
        /// Font used for regular text displayed on the ranking screen.
        /// </summary>
        private SpriteFont _font;

        /// <summary>
        /// Font used for the ranking screen title.
        /// </summary>
        private SpriteFont _font_title;

        /// <summary>
        /// Button used to return to the main menu.
        /// </summary>
        private Button _returnButton;

        /// <summary>
        /// Button used to display scores from the Easy difficulty.
        /// </summary>
        private Button _easyButton;

        /// <summary>
        /// Button used to display scores from the Medium difficulty.
        /// </summary>
        private Button _normalButton;

        /// <summary>
        /// Button used to display scores from the Hard difficulty.
        /// </summary>
        private Button _hardButton;

        /// <summary>
        /// Button used to display scores from all difficulties.
        /// </summary>
        private Button _allButton;

        /// <summary>
        /// Stores the keyboard state from the previous update.
        /// </summary>
        private KeyboardState _previousKeyboard;

        /// <summary>
        /// Contains all scores retrieved from the database.
        /// </summary>
        private List<ScoreData> _scores;

        /// <summary>
        /// Contains the scores currently displayed after applying the selected difficulty filter.
        /// </summary>
        private List<ScoreData> _filteredScores;

        /// <summary>
        /// Index of the first score currently visible in the ranking list.
        /// </summary>
        private int _scrollIndex;

        /// <summary>
        /// Maximum number of scores displayed at the same time.
        /// </summary>
        private int _visibleScores = 7;

        /// <summary>
        /// Initializes a new instance of the <see cref="RankingScreen"/> class.
        /// </summary>
        /// <param name="game">Reference to the main game instance.</param>
        public RankingScreen(Game1 game) : base(game)
        {
            _previousKeyboard = Keyboard.GetState();
            Game.ChangeScreenSize(500, 500);
        }

        /// <summary>
        /// Loads the fonts, creates the ranking buttons, retrieves the scores,
        /// and initializes the default ranking filter.
        /// </summary>
        public override void Initialize()
        {
            _font = Game.Content.Load<SpriteFont>("Fonts/SpaceInvader_12");
            _font_title = Game.Content.Load<SpriteFont>("Fonts/SpaceInvader_16");

            _returnButton = new Button("Return", new XnaRectangle(100, 430, 300, 45));
            _allButton = new Button("All 1", new XnaRectangle(10, 75, 100, 35));
            _easyButton = new Button("Easy 2", new XnaRectangle(130, 75, 100, 35));
            _normalButton = new Button("Medium 3", new XnaRectangle(240, 75, 130, 35));
            _hardButton = new Button("Hard 4", new XnaRectangle(390, 75, 100, 35));

            _scores = MariaDbManager.GetScores();
            _filteredScores = _scores.ToList();

            _scrollIndex = 0;

            _allButton.SetSelected(true);
            _easyButton.SetSelected(false);
            _normalButton.SetSelected(false);
            _hardButton.SetSelected(false);
        }

        /// <summary>
        /// Filters the ranking scores according to the selected difficulty.
        /// </summary>
        /// <param name="difficulty">Difficulty used to filter the scores.</param>
        private void SortByDifficulty(string difficulty)
        {
            _filteredScores = _scores
                .Where(score => string.Equals(score.Difficulty, difficulty, StringComparison.OrdinalIgnoreCase))
                .ToList();

            _scrollIndex = 0;
        }

        /// <summary>
        /// Scrolls the ranking list one position upward when possible.
        /// </summary>
        private void ScrollUp()
        {
            if (_scrollIndex <= 0)
                return;

            _scrollIndex--;
        }

        /// <summary>
        /// Scrolls the ranking list one position downward when more scores are available.
        /// </summary>
        private void ScrollDown()
        {
            if (_scrollIndex + _visibleScores >= _filteredScores.Count)
                return;

            _scrollIndex++;
        }

        /// <summary>
        /// Updates the ranking buttons, filters, scrolling controls, and keyboard input.
        /// </summary>
        /// <param name="gameTime">Provides timing information for the current update.</param>
        /// <param name="keyboard">Current keyboard state.</param>
        /// <param name="mousePosition">Current position of the mouse cursor.</param>
        /// <param name="mouseClicked">Indicates whether the mouse button was clicked during this update.</param>
        public override void Update(GameTime gameTime, KeyboardState keyboard, Vector2 mousePosition, bool mouseClicked)
        {
            _returnButton.Update(mousePosition);
            _easyButton.Update(mousePosition);
            _normalButton.Update(mousePosition);
            _hardButton.Update(mousePosition);
            _allButton.Update(mousePosition);

            if (_allButton.IsClicked(mousePosition, mouseClicked) || (keyboard.IsKeyDown(XnaKeys.D1) && _previousKeyboard.IsKeyUp(XnaKeys.D1)) || (keyboard.IsKeyDown(XnaKeys.NumPad1) && _previousKeyboard.IsKeyUp(XnaKeys.D0)))
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

            if (_easyButton.IsClicked(mousePosition, mouseClicked) || (keyboard.IsKeyDown(XnaKeys.D2) && _previousKeyboard.IsKeyUp(XnaKeys.D2)) || (keyboard.IsKeyDown(XnaKeys.NumPad2) && _previousKeyboard.IsKeyUp(XnaKeys.NumPad2)))
            {
                SortByDifficulty("Easy");

                _allButton.SetSelected(false);
                _easyButton.SetSelected(true);
                _normalButton.SetSelected(false);
                _hardButton.SetSelected(false);
            }

            if (_normalButton.IsClicked(mousePosition, mouseClicked) || (keyboard.IsKeyDown(XnaKeys.D3) && _previousKeyboard.IsKeyUp(XnaKeys.D3)) || (keyboard.IsKeyDown(XnaKeys.NumPad3) && _previousKeyboard.IsKeyUp(XnaKeys.NumPad3)))
            {
                SortByDifficulty("Medium");

                _allButton.SetSelected(false);
                _easyButton.SetSelected(false);
                _normalButton.SetSelected(true);
                _hardButton.SetSelected(false);
            }

            if (_hardButton.IsClicked(mousePosition, mouseClicked) || (keyboard.IsKeyDown(XnaKeys.D4) && _previousKeyboard.IsKeyUp(XnaKeys.D4)) || (keyboard.IsKeyDown(XnaKeys.NumPad4) && _previousKeyboard.IsKeyUp(XnaKeys.NumPad4)))
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

        /// <summary>
        /// Draws the ranking title, difficulty filters, scores, navigation indicators,
        /// database error messages, and return button.
        /// </summary>
        /// <param name="gameTime">Provides timing information for the current game frame.</param>
        /// <param name="spriteBatch">The sprite batch used to draw the screen.</param>
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