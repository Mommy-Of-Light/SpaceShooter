namespace SpaceShooter
{
    public class RankingScreen : GameScreen
    {
        private SpriteFont _font;
        private SpriteFont _font_title;
        private Button _returnButton;
        private KeyboardState _previousKeyboard;
        private List<ScoreData> _scores;

        public RankingScreen(Game1 game) : base(game)
        {
            _previousKeyboard = Keyboard.GetState();
            Game.ChangeScreenSize(500, 500);
        }

        public override void Initialize()
        {
            _font = Game.Content.Load<SpriteFont>("Fonts/SpaceInvader_12");
            _font_title = Game.Content.Load<SpriteFont>("Fonts/SpaceInvader_16");

            _returnButton = new Button("Return", new XnaRectangle(100, 420, 300, 50));

            _scores = MariaDbManager.GetScores();
        }

        public override void Update(GameTime gameTime, KeyboardState keyboard, Vector2 mousePosition, bool mouseClicked)
        {
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

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            Texture2D background = Game.Content.Load<Texture2D>("Textures/Background/black");
            spriteBatch.Draw(background, Vector2.Zero, XnaColor.White);

            string title = "RANKING";
            Vector2 titleSize = _font_title.MeasureString(title);

            spriteBatch.DrawString(_font_title, title, new Vector2((Game.GraphicsDevice.Viewport.Width - titleSize.X) / 2f, 40), XnaColor.White);

            if (!string.IsNullOrEmpty(MariaDbManager.LastError))
            {
                string error = "DATABASE ERROR";

                Vector2 errorSize = _font.MeasureString(error);

                spriteBatch.DrawString(_font, error, new Vector2((Game.GraphicsDevice.Viewport.Width - errorSize.X) / 2f, 120), XnaColor.White);
            }
            else if (_scores.Count == 0)
            {
                string noScores = "NO SCORES";

                Vector2 noScoresSize = _font.MeasureString(noScores);

                spriteBatch.DrawString(_font, noScores, new Vector2((Game.GraphicsDevice.Viewport.Width - noScoresSize.X) / 2f, 150), XnaColor.White);
            }
            else
            {
                int maxScores = Math.Min(_scores.Count, 8);

                for (int i = 0; i < maxScores; i++)
                {
                    ScoreData score = _scores[i];

                    string pseudo = score.Pseudo;

                    if (pseudo.Length > 12)
                        pseudo = pseudo.Substring(0, 12);

                    string line = (i + 1) + ". " + pseudo + "    " + score.Score + "    " + score.Difficulty;

                    spriteBatch.DrawString(_font, line, new Vector2(40, 100 + i * 38), XnaColor.White);
                }
            }

            _returnButton.Draw(spriteBatch, _font);

            spriteBatch.End();
        }
    }
}