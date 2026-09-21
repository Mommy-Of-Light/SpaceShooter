namespace SpaceShooter
{
    public class NewGameScreen : GameScreen
    {
        private SpriteFont _font;
        private SpriteFont _font_title;
        private List<Button> _buttons;
        private KeyboardState _previousKeyboard;
        private string _selectedDifficulty = "Medium";
        private Button _easyButton;
        private Button _mediumButton;
        private Button _hardButton;

        public NewGameScreen(Game1 game) : base(game)
        {
            _previousKeyboard = Keyboard.GetState();
            Game.ChangeScreenSize(500, 500);
        }

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

            if (SaveManager.Exists())
            {
                _buttons.Add(new Button("Continue", new XnaRectangle(x, startY, buttonWidth, buttonHeight)));
            }

            int difficultyY = startY + spacing;

            int difficultyButtonWidth = 105;
            int difficultySpacing = 10;

            int difficultyTotalWidth = (difficultyButtonWidth * 3) + (difficultySpacing * 2);
            int difficultyX = (screenWidth - difficultyTotalWidth) / 2;

            _easyButton = new Button(
                "Easy",
                new XnaRectangle(
                    difficultyX,
                    difficultyY,
                    difficultyButtonWidth,
                    buttonHeight
                )
            );
            _easyButton.SetSelected(false);

            _mediumButton = new Button(
                "Medium",
                new XnaRectangle(
                    difficultyX + difficultyButtonWidth + difficultySpacing,
                    difficultyY,
                    difficultyButtonWidth,
                    buttonHeight
                )
            );
            _mediumButton.SetSelected(true);

            _hardButton = new Button(
                "Hard",
                new XnaRectangle(
                    difficultyX + (difficultyButtonWidth + difficultySpacing) * 2,
                    difficultyY,
                    difficultyButtonWidth,
                    buttonHeight
                )
            );
            _hardButton.SetSelected(false);

            int newGameY = difficultyY + spacing;

            _buttons.Add(new Button(
                "Create a new game",
                new XnaRectangle(x, newGameY, buttonWidth, buttonHeight)
            ));

            int returnY = newGameY + spacing;

            _buttons.Add(new Button(
                "Return",
                new XnaRectangle(x, returnY, buttonWidth, buttonHeight)
            ));
        }

        public override void Update(GameTime gameTime, KeyboardState keyboard, Vector2 mousePosition, bool mouseClicked)
        {
            if (keyboard.IsKeyDown(XnaKeys.Escape) && _previousKeyboard.IsKeyUp(XnaKeys.Escape))
            {
                HandleButton("Return");
            }

            _easyButton.Update(mousePosition);
            _mediumButton.Update(mousePosition);
            _hardButton.Update(mousePosition);

            if (_easyButton.IsClicked(mousePosition, mouseClicked))
            {
                HandleButton("Easy");
            }
            else if (_mediumButton.IsClicked(mousePosition, mouseClicked))
            {
                HandleButton("Medium");
            }
            else if (_hardButton.IsClicked(mousePosition, mouseClicked))
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

            _previousKeyboard = keyboard;
        }

        private void HandleButton(string button)
        {
            switch (button)
            {
                case "Continue":
                    PlayScreen playScreen = new PlayScreen(Game);
                    Game.ScreenManager.ChangeScreen(playScreen);
                    playScreen.LoadGame();
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

                    SaveManager.Delete();

                    Game.ScreenManager.ChangeScreen(new PlayScreen(Game));
                    break;

                case "Return":
                    Game.ScreenManager.ChangeScreen(new MenuScreen(Game));
                    break;
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(
                Game.Content.Load<Texture2D>("Textures/Background/black"),
                Vector2.Zero,
                XnaColor.White
            );

            int screenWidth = Game.GraphicsDevice.Viewport.Width;

            string title = "NEW GAME";

            Vector2 titleSize = _font_title.MeasureString(title);

            spriteBatch.DrawString(
                _font_title,
                title,
                new Vector2((screenWidth - titleSize.X) / 2, 50),
                XnaColor.White
            );

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