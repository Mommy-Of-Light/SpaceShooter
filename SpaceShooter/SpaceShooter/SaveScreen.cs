namespace SpaceShooter
{
    public class SaveScreen : GameScreen
    {
        private SpriteFont _font;
        private SpriteFont _font_title;
        private KeyboardState _previousKeyboard;
        private List<SaveData> _saves;
        private List<Button> _saveButtons;
        private Button _returnButton;
        private int _scrollIndex;
        private int _visibleSaves = 7;

        public SaveScreen(Game1 game) : base(game)
        {
            _previousKeyboard = Keyboard.GetState();
            Game.ChangeScreenSize(800, 500);
        }

        public override void Initialize()
        {
            _font = Game.Content.Load<SpriteFont>("Fonts/SpaceInvader_12");
            _font_title = Game.Content.Load<SpriteFont>("Fonts/SpaceInvader_16");

            _saves = SaveManager.GetSaves(Game.PlayerPseudo);

            _saveButtons = new List<Button>();

            _scrollIndex = 0;

            CreateSaveButtons();

            _returnButton = new Button("Return", new XnaRectangle(250, 430, 300, 45));
        }

        private void CreateSaveButtons()
        {
            _saveButtons.Clear();

            int endIndex = Math.Min(_scrollIndex + _visibleSaves, _saves.Count);

            for (int i = _scrollIndex; i < endIndex; i++
            )
            {
                SaveData save = _saves[i];

                string buttonText = "Wave " + save.Game.CurrentWave + " - " + save.Game.Score;

                Button button = new Button(buttonText, new XnaRectangle(225, 100 + (i - _scrollIndex) * 45, 350, 35));

                _saveButtons.Add(button);
            }
        }

        private void ScrollUp()
        {
            if (_scrollIndex <= 0)
                return;

            _scrollIndex--;

            CreateSaveButtons();
        }

        private void ScrollDown()
        {
            if (_scrollIndex + _visibleSaves >= _saves.Count)
                return;

            _scrollIndex++;

            CreateSaveButtons();
        }

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

            if (keyboard.IsKeyDown(XnaKeys.Up) && _previousKeyboard.IsKeyUp(XnaKeys.Up))
            {
                ScrollUp();
            }

            if (keyboard.IsKeyDown(XnaKeys.Down) && _previousKeyboard.IsKeyUp(XnaKeys.Down))
            {
                ScrollDown();
            }

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

            _previousKeyboard = keyboard;
        }

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
                    {
                        difficulty = "Medium";
                    }

                    string information = "Wave " + save.Game.CurrentWave + "  Score " + save.Game.Score + "  " + difficulty + "  " + date;

                    Vector2 informationSize = _font.MeasureString(information);

                    spriteBatch.DrawString(_font, information, new Vector2((Game.GraphicsDevice.Viewport.Width - informationSize.X) / 2f, 82 + i * 45), XnaColor.White);

                    _saveButtons[i].Draw(spriteBatch, _font);
                }

                if (_scrollIndex > 0)
                {
                    spriteBatch.DrawString(_font, "UP", new Vector2(20, 120), XnaColor.White);
                }

                if (_scrollIndex + _visibleSaves < _saves.Count)
                {
                    spriteBatch.DrawString(_font, "DOWN", new Vector2(20, 155), XnaColor.White);
                }

                string counter = (_scrollIndex + 1) + "-" + Math.Min(_scrollIndex + _visibleSaves, _saves.Count) + " / " + _saves.Count;

                Vector2 counterSize = _font.MeasureString(counter);

                spriteBatch.DrawString(_font, counter, new Vector2((Game.GraphicsDevice.Viewport.Width - counterSize.X) / 2f, 405), XnaColor.White);
            }

            _returnButton.Draw(spriteBatch, _font);

            spriteBatch.End();
        }
    }
}