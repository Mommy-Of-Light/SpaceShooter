namespace SpaceShooter
{
    /// <summary>
    /// Represents the screen used to display, load, and delete saved games.
    /// </summary>
    public class SaveScreen : GameScreen
    {
        /// <summary>
        /// Font used for regular text displayed on the screen.
        /// </summary>
        private SpriteFont _font;

        /// <summary>
        /// Font used for the screen title.
        /// </summary>
        private SpriteFont _font_title;

        /// <summary>
        /// Stores the keyboard state from the previous update.
        /// </summary>
        private KeyboardState _previousKeyboard;

        /// <summary>
        /// List of saved games belonging to the current player.
        /// </summary>
        private List<SaveData> _saves;

        /// <summary>
        /// Buttons used to select and load saved games.
        /// </summary>
        private List<Button> _saveButtons;

        /// <summary>
        /// Buttons used to delete saved games.
        /// </summary>
        private List<Button> _deleteButtons;

        /// <summary>
        /// Button used to return to the main menu.
        /// </summary>
        private Button _returnButton;

        /// <summary>
        /// Index of the first save currently visible on the screen.
        /// </summary>
        private int _scrollIndex;

        /// <summary>
        /// Maximum number of saves displayed at the same time.
        /// </summary>
        private int _visibleSaves = 7;

        /// <summary>
        /// Buttons that can be selected using keyboard navigation.
        /// </summary>
        private List<Button> _navigationButtons;

        /// <summary>
        /// Initializes a new instance of the <see cref="SaveScreen"/> class.
        /// </summary>
        /// <param name="game">Reference to the main game instance.</param>
        public SaveScreen(Game1 game) : base(game)
        {
            _previousKeyboard = Keyboard.GetState();
            Game.ChangeScreenSize(800, 500);
        }

        /// <summary>
        /// Loads the fonts, retrieves the player's saved games,
        /// initializes the buttons, and prepares the save list.
        /// </summary>
        public override void Initialize()
        {
            _font = Game.Content.Load<SpriteFont>("Fonts/SpaceInvader_12");
            _font_title = Game.Content.Load<SpriteFont>("Fonts/SpaceInvader_16");

            _saves = SaveManager.GetSaves(Game.PlayerPseudo);

            _saveButtons = new List<Button>();
            _deleteButtons = new List<Button>();

            _scrollIndex = 0;

            _returnButton = new Button("Return", new XnaRectangle(250, 430, 300, 45));

            CreateSaveButtons();
        }

        /// <summary>
        /// Creates the buttons corresponding to the currently visible saved games.
        /// </summary>
        private void CreateSaveButtons()
        {
            _saveButtons.Clear();
            _deleteButtons.Clear();

            int endIndex = Math.Min(_scrollIndex + _visibleSaves, _saves.Count);

            _navigationButtons = new List<Button>();

            for (int i = _scrollIndex; i < endIndex; i++)
            {
                SaveData save = _saves[i];
                int displayIndex = i - _scrollIndex;

                string buttonText = "Wave " + save.Game.CurrentWave + " - " + save.Game.Score;

                Button saveButton = new Button(
                    buttonText,
                    new XnaRectangle(225, 100 + displayIndex * 45, 350, 35));

                _saveButtons.Add(saveButton);

                Button deleteButton = new Button(
                    "Delete",
                    new XnaRectangle(580, 100 + displayIndex * 45, 80, 35));

                _deleteButtons.Add(deleteButton);

                _navigationButtons.Add(saveButton);
                _navigationButtons.Add(deleteButton);
            }

            _navigationButtons.Add(_returnButton);
        }

        /// <summary>
        /// Scrolls the save list one position upward when possible.
        /// </summary>
        private void ScrollUp()
        {
            if (_scrollIndex <= 0)
                return;

            _scrollIndex--;

            CreateSaveButtons();
        }

        /// <summary>
        /// Scrolls the save list one position downward when more saves are available.
        /// </summary>
        private void ScrollDown()
        {
            if (_scrollIndex + _visibleSaves >= _saves.Count)
                return;

            _scrollIndex++;

            CreateSaveButtons();
        }

        /// <summary>
        /// Updates the save buttons, deletion controls, scrolling, and keyboard navigation.
        /// </summary>
        /// <param name="gameTime">Provides timing information for the current update.</param>
        /// <param name="keyboard">Current keyboard state.</param>
        /// <param name="mousePosition">Current position of the mouse cursor.</param>
        /// <param name="mouseClicked">Indicates whether the mouse button was clicked during this update.</param>
        public override void Update(GameTime gameTime, KeyboardState keyboard, Vector2 mousePosition, bool mouseClicked)
        {
            for (int i = 0; i < _saveButtons.Count; i++)
            {
                Button button = _saveButtons[i];

                button.Update(mousePosition);

                if (button.IsClicked(mousePosition, mouseClicked))
                {
                    LoadSave(_scrollIndex + i);
                    return;
                }
            }

            for (int i = 0; i < _deleteButtons.Count; i++)
            {
                Button button = _deleteButtons[i];

                button.Update(mousePosition);

                if (button.IsClicked(mousePosition, mouseClicked))
                {
                    DeleteSave(_scrollIndex + i);
                    return;
                }
            }

            if (keyboard.IsKeyDown(XnaKeys.Up) && _previousKeyboard.IsKeyUp(XnaKeys.Up))
                ScrollUp();

            if (keyboard.IsKeyDown(XnaKeys.Down) && _previousKeyboard.IsKeyUp(XnaKeys.Down))
                ScrollDown();

            _returnButton.Update(mousePosition);

            if (_returnButton.IsClicked(mousePosition, mouseClicked))
            {
                Game.ScreenManager.ChangeScreen(new MenuScreen(Game));
                return;
            }

            if (keyboard.IsKeyDown(XnaKeys.Escape) && _previousKeyboard.IsKeyUp(XnaKeys.Escape))
            {
                Game.ScreenManager.ChangeScreen(new MenuScreen(Game));
                return;
            }

            if (keyboard.IsKeyDown(XnaKeys.Up) && _previousKeyboard.IsKeyUp(XnaKeys.Up))
            {
                int selectedIndex = _navigationButtons.FindIndex(b => b.IsSelected);

                if (selectedIndex == -1)
                {
                    _navigationButtons[0].SetSelected(true);
                }
                else
                {
                    _navigationButtons[selectedIndex].SetSelected(false);

                    int newIndex = (selectedIndex - 1 + _navigationButtons.Count) % _navigationButtons.Count;

                    _navigationButtons[newIndex].SetSelected(true);
                }
            }

            if (keyboard.IsKeyDown(XnaKeys.Down) && _previousKeyboard.IsKeyUp(XnaKeys.Down))
            {
                int selectedIndex = _navigationButtons.FindIndex(b => b.IsSelected);

                if (selectedIndex == -1)
                {
                    _navigationButtons[0].SetSelected(true);
                }
                else
                {
                    _navigationButtons[selectedIndex].SetSelected(false);

                    int newIndex = (selectedIndex + 1) % _navigationButtons.Count;

                    _navigationButtons[newIndex].SetSelected(true);
                }
            }

            if ((keyboard.IsKeyDown(XnaKeys.Enter) && _previousKeyboard.IsKeyUp(XnaKeys.Enter)) ||
                (keyboard.IsKeyDown(XnaKeys.Space) && _previousKeyboard.IsKeyUp(XnaKeys.Space)))
            {
                int selectedIndex = _navigationButtons.FindIndex(b => b.IsSelected);

                if (selectedIndex != -1)
                {
                    Button selectedButton = _navigationButtons[selectedIndex];

                    if (selectedButton == _returnButton)
                    {
                        Game.ScreenManager.ChangeScreen(new MenuScreen(Game));
                        return;
                    }

                    int saveIndex = _scrollIndex + selectedIndex;

                    if (saveIndex >= 0 && saveIndex < _saves.Count)
                    {
                        LoadSave(saveIndex);
                        return;
                    }
                }
            }

            _previousKeyboard = keyboard;
        }

        /// <summary>
        /// Loads the selected saved game and switches to the game screen.
        /// </summary>
        /// <param name="index">Index of the save to load.</param>
        private void LoadSave(int index)
        {
            if (index < 0 || index >= _saves.Count)
                return;

            SaveData save = _saves[index];

            if (save == null || save.Game == null)
                return;

            PlayScreen playScreen = new PlayScreen(Game);

            Game.ScreenManager.ChangeScreen(playScreen);

            playScreen.LoadGame(save);
        }

        /// <summary>
        /// Deletes the selected saved game and refreshes the save list.
        /// </summary>
        /// <param name="index">Index of the save to delete.</param>
        private void DeleteSave(int index)
        {
            if (index < 0 || index >= _saves.Count)
                return;

            SaveData save = _saves[index];

            if (save == null)
                return;

            SaveManager.Delete(save);

            _saves = SaveManager.GetSaves(Game.PlayerPseudo);

            if (_scrollIndex >= _saves.Count && _scrollIndex > 0)
                _scrollIndex--;

            CreateSaveButtons();
        }

        /// <summary>
        /// Draws the saved games, navigation indicators, save information,
        /// and return button.
        /// </summary>
        /// <param name="gameTime">Provides timing information for the current game frame.</param>
        /// <param name="spriteBatch">The sprite batch used to draw the screen.</param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            Texture2D background = Game.Content.Load<Texture2D>("Textures/Background/black");

            spriteBatch.Draw(background, Vector2.Zero, XnaColor.White);

            string title = "SAVED GAMES";
            Vector2 titleSize = _font_title.MeasureString(title);

            spriteBatch.DrawString(_font_title, title, new Vector2((Game.GraphicsDevice.Viewport.Width - titleSize.X) / 2f, 30), XnaColor.White);

            if (_saves.Count == 0)
            {
                string text = "NO SAVED GAMES";
                Vector2 textSize = _font.MeasureString(text);

                spriteBatch.DrawString(_font, text, new Vector2((Game.GraphicsDevice.Viewport.Width - textSize.X) / 2f, 180), XnaColor.White);
            }
            else
            {
                for (int i = 0; i < _saveButtons.Count; i++)
                {
                    int saveIndex = _scrollIndex + i;
                    SaveData save = _saves[saveIndex];

                    string date = save.LastSavedAt.ToString("dd/MM/yyyy HH:mm");
                    string difficulty = save.Game.Difficulty;

                    if (string.IsNullOrEmpty(difficulty))
                        difficulty = "Medium";

                    string information = "Wave " + save.Game.CurrentWave + "  Score " + save.Game.Score + "  " + difficulty + "  " + date;
                    Vector2 informationSize = _font.MeasureString(information);

                    spriteBatch.DrawString(_font, information, new Vector2((Game.GraphicsDevice.Viewport.Width - informationSize.X) / 2f, 82 + i * 45), XnaColor.White);

                    _saveButtons[i].Draw(spriteBatch, _font);
                    _deleteButtons[i].Draw(spriteBatch, _font);
                }

                if (_scrollIndex > 0)
                    spriteBatch.DrawString(_font, "UP", new Vector2(20, 20), XnaColor.White);

                if (_scrollIndex + _visibleSaves < _saves.Count)
                    spriteBatch.DrawString(_font, "DOWN", new Vector2(20, 35), XnaColor.White);

                string counter = (_scrollIndex + 1) + "-" + Math.Min(_scrollIndex + _visibleSaves, _saves.Count) + " / " + _saves.Count;
                Vector2 counterSize = _font.MeasureString(counter);

                spriteBatch.DrawString(_font, counter, new Vector2((Game.GraphicsDevice.Viewport.Width - counterSize.X) / 2f, 405), XnaColor.White);
            }

            _returnButton.Draw(spriteBatch, _font);

            spriteBatch.End();
        }
    }
}