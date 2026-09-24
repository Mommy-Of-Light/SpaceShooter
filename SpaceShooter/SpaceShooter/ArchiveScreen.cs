namespace SpaceShooter
{
    public class ArchiveScreen : GameScreen
    {
        private SpriteFont _font;
        private SpriteFont _font_title;
        private KeyboardState _previousKeyboard;
        private List<ArchiveData> _archives;
        private Button _returnButton;
        private int _scrollIndex;
        private int _visibleArchives = 7;

        public ArchiveScreen(Game1 game) : base(game)
        {
            _previousKeyboard = Keyboard.GetState();
            Game.ChangeScreenSize(500, 500);
        }

        public override void Initialize()
        {
            _font = Game.Content.Load<SpriteFont>("Fonts/SpaceInvader_12");
            _font_title = Game.Content.Load<SpriteFont>("Fonts/SpaceInvader_16");
            _archives = ArchiveManager.GetArchives(Game.PlayerPseudo);
            _scrollIndex = 0;
            _returnButton = new Button("Return", new XnaRectangle(100, 430, 300, 45));
        }

        private void ScrollUp()
        {
            if (_scrollIndex <= 0)
                return;

            _scrollIndex--;
        }

        private void ScrollDown()
        {
            if (_scrollIndex + _visibleArchives >= _archives.Count)
                return;

            _scrollIndex++;
        }

        public override void Update(GameTime gameTime, KeyboardState keyboard, Vector2 mousePosition, bool mouseClicked)
        {
            _returnButton.Update(mousePosition);

            if (_returnButton.IsClicked(mousePosition, mouseClicked))
            {
                Game.ScreenManager.ChangeScreen(new MenuScreen(Game));
                return;
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

            string title = "ARCHIVE";

            Vector2 titleSize = _font_title.MeasureString(title);

            spriteBatch.DrawString(_font_title, title, new Vector2((Game.GraphicsDevice.Viewport.Width - titleSize.X) / 2f, 30), XnaColor.White);

            string pseudoText = "Player: " + Game.PlayerPseudo;

            Vector2 pseudoSize = _font.MeasureString(pseudoText);

            spriteBatch.DrawString(_font, pseudoText, new Vector2((Game.GraphicsDevice.Viewport.Width - pseudoSize.X) / 2f, 70), XnaColor.White);

            if (_archives.Count == 0)
            {
                string text = "NO PLAYED GAMES";

                Vector2 textSize = _font.MeasureString(text);

                spriteBatch.DrawString(_font, text, new Vector2((Game.GraphicsDevice.Viewport.Width - textSize.X) / 2f, 180), XnaColor.White);
            }
            else
            {
                int endIndex = Math.Min(_scrollIndex + _visibleArchives, _archives.Count);

                for (int i = _scrollIndex; i < endIndex; i++)
                {
                    ArchiveData archive = _archives[i];

                    int displayIndex = i - _scrollIndex;

                    string line = (i + 1) + ". Wave " + archive.Wave + "  Score " + archive.Score;

                    spriteBatch.DrawString(_font, line, new Vector2(45, 105 + displayIndex * 42), XnaColor.White);

                    string details = archive.Difficulty + "  " + archive.FinishedAt.ToString("dd/MM/yyyy HH:mm");

                    spriteBatch.DrawString(_font, details, new Vector2(45, 123 + displayIndex * 42), XnaColor.White);
                }

                if (_scrollIndex > 0)
                {
                    spriteBatch.DrawString(_font, "UP", new Vector2(10, 20), XnaColor.White);
                }

                if (_scrollIndex + _visibleArchives < _archives.Count
                )
                {
                    spriteBatch.DrawString(_font, "DOWN", new Vector2(10, 35), XnaColor.White);
                }

                string counter = (_scrollIndex + 1) + "-" + Math.Min(_scrollIndex + _visibleArchives, _archives.Count) + " / " + _archives.Count;

                Vector2 counterSize = _font.MeasureString(counter);

                spriteBatch.DrawString(_font, counter, new Vector2((Game.GraphicsDevice.Viewport.Width - counterSize.X) / 2f, 405), XnaColor.White);
            }

            _returnButton.Draw(spriteBatch, _font);

            spriteBatch.End();
        }
    }
}