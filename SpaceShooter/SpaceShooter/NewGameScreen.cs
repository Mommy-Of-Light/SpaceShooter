namespace SpaceShooter
{
    /// <summary>
    /// Represents the screen used to start a new game, continue an existing game,
    /// and select the desired difficulty level.
    /// </summary>
    public class NewGameScreen : GameScreen
    {
        /// <summary>
        /// Font used to display button text.
        /// </summary>
        private SpriteFont _font;

        /// <summary>
        /// Font used to display the screen title.
        /// </summary>
        private SpriteFont _font_title;

        /// <summary>
        /// Collection of the main action buttons displayed on the screen.
        /// </summary>
        private List<Button> _buttons;

        /// <summary>
        /// Stores the keyboard state from the previous update.
        /// Used to detect individual key presses.
        /// </summary>
        private KeyboardState _previousKeyboard;

        /// <summary>
        /// Stores the currently selected difficulty level.
        /// </summary>
        private string _selectedDifficulty = "Medium";

        /// <summary>
        /// Button used to select the Easy difficulty.
        /// </summary>
        private Button _easyButton;

        /// <summary>
        /// Button used to select the Medium difficulty.
        /// </summary>
        private Button _mediumButton;

        /// <summary>
        /// Button used to select the Hard difficulty.
        /// </summary>
        private Button _hardButton;

        /// <summary>
        /// Initializes a new instance of the <see cref="NewGameScreen"/> class.
        /// </summary>
        /// <param name="game">The main game instance associated with this screen.</param>
        public NewGameScreen(Game1 game) : base(game)
        {
            _previousKeyboard = Keyboard.GetState();
            Game.ChangeScreenSize(500, 500);
        }

        /// <summary>
        /// Initializes the fonts, buttons, and difficulty selection controls
        /// displayed on the new game screen.
        /// </summary>
        public override void Initialize()
        {
            _font = Game.Content.Load<SpriteFont>("Fonts/SpaceInvader_12");
            _font_title = Game.Content.Load<SpriteFont>("Fonts/SpaceInvader_16");

            _buttons = new List<Button>();

            int screenWidth = Game.GraphicsDevice.Viewport.Width;
            int buttonWidth = 350;
            int buttonHeight = 60;
            int x = (screenWidth - buttonWidth) / 2;
            int startY = 100;
            int spacing = 75;

            if (SaveManager.Exists(Game.PlayerPseudo))
            {
                _buttons.Add(new Button("Continue", new XnaRectangle(x, startY, buttonWidth, buttonHeight)));
            }

            int difficultyY = startY + spacing;
            int difficultyButtonWidth = 160;
            int difficultySpacing = 10;
            int difficultyTotalWidth = (difficultyButtonWidth * 3) + (difficultySpacing * 2);
            int difficultyX = (screenWidth - difficultyTotalWidth) / 2;

            _easyButton = new Button("Easy (1)", new XnaRectangle(difficultyX, difficultyY, difficultyButtonWidth, buttonHeight));
            _easyButton.SetSelected(false);

            _mediumButton = new Button("Medium (2)", new XnaRectangle(difficultyX + difficultyButtonWidth + difficultySpacing, difficultyY, difficultyButtonWidth, buttonHeight));
            _mediumButton.SetSelected(true);

            _hardButton = new Button("Hard (3)", new XnaRectangle(difficultyX + (difficultyButtonWidth + difficultySpacing) * 2, difficultyY, difficultyButtonWidth, buttonHeight));
            _hardButton.SetSelected(false);

            int newGameY = difficultyY + spacing;
            _buttons.Add(new Button("Create a new game", new XnaRectangle(x, newGameY, buttonWidth, buttonHeight)));

            int returnY = newGameY + spacing;
            _buttons.Add(new Button("Return", new XnaRectangle(x, returnY, buttonWidth, buttonHeight)));

            if (_buttons.Count > 0)
            {
                _buttons[0].SetSelected(true);
            }
        }

        /// <summary>
        /// Updates the screen, processes keyboard and mouse input,
        /// and handles menu and difficulty selection.
        /// </summary>
        /// <param name="gameTime">Provides timing information for the update.</param>
        /// <param name="keyboard">The current keyboard state.</param>
        /// <param name="mousePosition">The current position of the mouse cursor.</param>
        /// <param name="mouseClicked">Indicates whether the mouse was clicked during this update.</param>
        public override void Update(GameTime gameTime, KeyboardState keyboard, Vector2 mousePosition, bool mouseClicked)
        {
            if (keyboard.IsKeyDown(XnaKeys.Escape) && _previousKeyboard.IsKeyUp(XnaKeys.Escape))
            {
                HandleButton("Return");
            }

            _easyButton.Update(mousePosition);
            _mediumButton.Update(mousePosition);
            _hardButton.Update(mousePosition);

            if (_easyButton.IsClicked(mousePosition, mouseClicked) ||
                (keyboard.IsKeyDown(XnaKeys.D1) && _previousKeyboard.IsKeyUp(XnaKeys.D1)) ||
                (keyboard.IsKeyDown(XnaKeys.NumPad1) && _previousKeyboard.IsKeyUp(XnaKeys.NumPad1)))
            {
                HandleButton("Easy");
            }
            else if (_mediumButton.IsClicked(mousePosition, mouseClicked) ||
                     (keyboard.IsKeyDown(XnaKeys.D2) && _previousKeyboard.IsKeyUp(XnaKeys.D2)) ||
                     (keyboard.IsKeyDown(XnaKeys.NumPad2) && _previousKeyboard.IsKeyUp(XnaKeys.NumPad2)))
            {
                HandleButton("Medium");
            }
            else if (_hardButton.IsClicked(mousePosition, mouseClicked) ||
                     (keyboard.IsKeyDown(XnaKeys.D3) && _previousKeyboard.IsKeyUp(XnaKeys.D3)) ||
                     (keyboard.IsKeyDown(XnaKeys.NumPad3) && _previousKeyboard.IsKeyUp(XnaKeys.NumPad3)))
            {
                HandleButton("Hard");
            }

            foreach (Button button in _buttons)
            {
                button.Update(mousePosition);

                if (button.IsClicked(mousePosition, mouseClicked))
                {
                    HandleButton(button.Text);
                    break;
                }
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

            _previousKeyboard = keyboard;
        }

        /// <summary>
        /// Handles the action associated with the selected button.
        /// This includes continuing a saved game, selecting a difficulty,
        /// creating a new game, or returning to the main menu.
        /// </summary>
        /// <param name="button">The text identifying the selected button or difficulty.</param>
        private void HandleButton(string button)
        {
            switch (button)
            {
                case "Continue":
                    SaveData save = SaveManager.GetLatestSave(Game.PlayerPseudo);

                    if (save != null)
                    {
                        PlayScreen playScreen = new PlayScreen(Game);
                        Game.ScreenManager.ChangeScreen(playScreen);
                        playScreen.LoadGame(save);
                    }
                    break;

                case "Easy":
                    _selectedDifficulty = "Easy";
                    _easyButton.SetSelected(true);
                    _mediumButton.SetSelected(false);
                    _hardButton.SetSelected(false);
                    break;

                case "Medium":
                    _selectedDifficulty = "Medium";
                    _easyButton.SetSelected(false);
                    _mediumButton.SetSelected(true);
                    _hardButton.SetSelected(false);
                    break;

                case "Hard":
                    _selectedDifficulty = "Hard";
                    _easyButton.SetSelected(false);
                    _mediumButton.SetSelected(false);
                    _hardButton.SetSelected(true);
                    break;

                case "Create a new game":
                    Game.Difficulty = _selectedDifficulty;

                    if (_selectedDifficulty == "Easy")
                    {
                        Game.DifficultyMultiplier = 0.5;
                    }
                    else if (_selectedDifficulty == "Medium")
                    {
                        Game.DifficultyMultiplier = 1.0;
                    }
                    else if (_selectedDifficulty == "Hard")
                    {
                        Game.DifficultyMultiplier = 2.0;
                    }

                    Game.ScreenManager.ChangeScreen(new PlayScreen(Game));
                    break;

                case "Return":
                    Game.ScreenManager.ChangeScreen(new MenuScreen(Game));
                    break;
            }
        }

        /// <summary>
        /// Draws the new game screen, including the background, title,
        /// main action buttons, and difficulty selection buttons.
        /// </summary>
        /// <param name="gameTime">Provides timing information for the draw operation.</param>
        /// <param name="spriteBatch">The sprite batch used to draw the screen.</param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(Game.Content.Load<Texture2D>("Textures/Background/black"), Vector2.Zero, XnaColor.White);

            int screenWidth = Game.GraphicsDevice.Viewport.Width;
            string title = "NEW GAME";
            Vector2 titleSize = _font_title.MeasureString(title);

            spriteBatch.DrawString(_font_title, title, new Vector2((screenWidth - titleSize.X) / 2, 50), XnaColor.White);

            foreach (Button button in _buttons)
            {
                button.Draw(spriteBatch, _font);
            }

            _easyButton.Draw(spriteBatch, _font);
            _mediumButton.Draw(spriteBatch, _font);
            _hardButton.Draw(spriteBatch, _font);

            spriteBatch.End();
        }
    }
}