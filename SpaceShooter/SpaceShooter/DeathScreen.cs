namespace SpaceShooter
{
    /// <summary>
    /// Represents the screen displayed when the player loses the game.
    /// Provides options to restart the game or return to the new game screen.
    /// </summary>
    public class DeathScreen : GameScreen
    {
        /// <summary>
        /// Font used to display general screen text.
        /// </summary>
        private SpriteFont _font;

        /// <summary>
        /// Font used to display the game over title.
        /// </summary>
        private SpriteFont _titleFont;

        /// <summary>
        /// Button used to restart the game.
        /// </summary>
        private Button _restartButton;

        /// <summary>
        /// Button used to exit the current game.
        /// </summary>
        private Button _exitButton;

        /// <summary>
        /// Stores the previous keyboard state to detect individual key presses.
        /// </summary>
        private KeyboardState _previousKeyboard;

        /// <summary>
        /// Stores the wave reached by the player before the game ended.
        /// </summary>
        private int _wave;

        /// <summary>
        /// Stores the player's final score.
        /// </summary>
        private int _score;

        /// <summary>
        /// Indicates whether the final score has already been saved.
        /// </summary>
        private bool _scoreSaved;

        /// <summary>
        /// Contains the buttons available on the death screen.
        /// </summary>
        private List<Button> _buttons;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeathScreen"/> class.
        /// </summary>
        /// <param name="game">The main game instance associated with this screen.</param>
        /// <param name="wave">The wave reached by the player before the game ended.</param>
        /// <param name="score">The player's final score.</param>
        public DeathScreen(Game1 game, int wave, int score) : base(game)
        {
            _wave = wave;
            _score = score;
            _previousKeyboard = Keyboard.GetState();
            _scoreSaved = false;
            Game.ChangeScreenSize(500, 500);
        }

        /// <summary>
        /// Initializes the screen resources, buttons, score saving,
        /// and menu music.
        /// </summary>
        public override void Initialize()
        {
            _font = Game.Content.Load<SpriteFont>("Fonts/SpaceInvader_12");
            _titleFont = Game.Content.Load<SpriteFont>("Fonts/SpaceInvader_16");

            _restartButton = new Button("Restart", new XnaRectangle((Game.GraphicsDevice.Viewport.Width - 200) / 2, 300, 200, 50));
            _exitButton = new Button("Exit", new XnaRectangle((Game.GraphicsDevice.Viewport.Width - 200) / 2, 380, 200, 50));

            _buttons = new List<Button>
            {
                _restartButton,
                _exitButton
            };

            if (_buttons.Count > 0)
            {
                _buttons[0].SetSelected(true);
            }

            SaveScore();

            MusicPlayer.Instance.Stop();
            MusicPlayer.Instance.Play("Menu");
        }

        /// <summary>
        /// Saves the player's final score if it has not already been saved.
        /// </summary>
        private void SaveScore()
        {
            if (_scoreSaved)
                return;

            _scoreSaved = true;
            MariaDbManager.SaveScore(Game.PlayerPseudo, _score, _wave, Game.Difficulty);
        }

        /// <summary>
        /// Updates the screen based on mouse and keyboard input.
        /// </summary>
        /// <param name="gameTime">Provides timing information for the current game update.</param>
        /// <param name="keyboard">The current state of the keyboard.</param>
        /// <param name="mousePosition">The current position of the mouse cursor.</param>
        /// <param name="mouseClicked">Indicates whether the mouse button was clicked during this update.</param>
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

            if (keyboard.IsKeyDown(XnaKeys.Up) && _previousKeyboard.IsKeyUp(XnaKeys.Up))
            {
                int selectedIndex = _buttons.FindIndex(b => b.IsSelected);

                if (selectedIndex == -1)
                {
                    _buttons[0].SetSelected(true);
                }
                else
                {
                    _buttons[selectedIndex].SetSelected(false);
                    int newIndex = (selectedIndex - 1 + _buttons.Count) % _buttons.Count;
                    _buttons[newIndex].SetSelected(true);
                }
            }

            if (keyboard.IsKeyDown(XnaKeys.Down) && _previousKeyboard.IsKeyUp(XnaKeys.Down))
            {
                int selectedIndex = _buttons.FindIndex(b => b.IsSelected);

                if (selectedIndex == -1)
                {
                    _buttons[0].SetSelected(true);
                }
                else
                {
                    _buttons[selectedIndex].SetSelected(false);
                    int newIndex = (selectedIndex + 1) % _buttons.Count;
                    _buttons[newIndex].SetSelected(true);
                }
            }

            if ((keyboard.IsKeyDown(XnaKeys.Enter) && _previousKeyboard.IsKeyUp(XnaKeys.Enter)) ||
                (keyboard.IsKeyDown(XnaKeys.Space) && _previousKeyboard.IsKeyUp(XnaKeys.Space)))
            {
                int selectedIndex = _buttons.FindIndex(b => b.IsSelected);

                if (selectedIndex != -1)
                {
                    HandleButton(_buttons[selectedIndex].Text);
                    return;
                }
            }

            if (keyboard.IsKeyDown(XnaKeys.Escape) && _previousKeyboard.IsKeyUp(XnaKeys.Escape))
            {
                Game.ScreenManager.ChangeScreen(new NewGameScreen(Game));
                return;
            }

            _previousKeyboard = keyboard;
        }

        /// <summary>
        /// Handles the action associated with the selected button.
        /// </summary>
        /// <param name="button">The name of the button whose action should be handled.</param>
        private void HandleButton(string button)
        {
            switch (button)
            {
                case "Restart":
                    Game.ScreenManager.ChangeScreen(new PlayScreen(Game));
                    break;

                case "Exit":
                    Game.ScreenManager.ChangeScreen(new NewGameScreen(Game));
                    break;
            }
        }

        /// <summary>
        /// Draws the game over screen, including the player's score,
        /// reached wave, and available buttons.
        /// </summary>
        /// <param name="gameTime">Provides timing information for the current game draw operation.</param>
        /// <param name="spriteBatch">Used to draw the screen elements.</param>
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