namespace SpaceShooter
{
    public class PseudoScreen : GameScreen
    {
        private SpriteFont _font;
        private SpriteFont _titleFont;
        private Button _continueButton;
        private KeyboardState _previousKeyboard;
        private string _pseudo;

        public PseudoScreen(Game1 game) : base(game)
        {
            _previousKeyboard = Keyboard.GetState();
            _pseudo = "";

            Game.ChangeScreenSize(500, 500);
        }

        public override void Initialize()
        {
            _font = Game.Content.Load<SpriteFont>("Fonts/SpaceInvader_12");
            _titleFont = Game.Content.Load<SpriteFont>("Fonts/SpaceInvader_16");

            _continueButton = new Button("Continue", new XnaRectangle(150, 300, 200, 50));

            Game.Window.TextInput += OnTextInput;
        }

        private void OnTextInput(object sender, TextInputEventArgs e)
        {
            if (char.IsControl(e.Character))
                return;

            if (_pseudo.Length >= 16)
                return;

            if (char.IsLetterOrDigit(e.Character) || e.Character == '_' || e.Character == '-')
                _pseudo += e.Character;
        }

        public override void Update(GameTime gameTime, KeyboardState keyboard, Vector2 mousePosition, bool mouseClicked)
        {
            _continueButton.Update(mousePosition);

            if (keyboard.IsKeyDown(XnaKeys.Back) && _previousKeyboard.IsKeyUp(XnaKeys.Back))
            {
                if (_pseudo.Length > 0)
                    _pseudo = _pseudo.Substring(0, _pseudo.Length - 1);
            }

            if (keyboard.IsKeyDown(XnaKeys.Enter) && _previousKeyboard.IsKeyUp(XnaKeys.Enter))
            {
                StartGame();
                return;
            }

            if (_continueButton.IsClicked(mousePosition, mouseClicked))
            {
                StartGame();
                return;
            }

            _previousKeyboard = keyboard;
        }

        private void StartGame()
        {
            if (string.IsNullOrWhiteSpace(_pseudo))
                return;

            Game.PlayerPseudo = _pseudo.Trim();

            Game.Difficulty = "Medium";
            Game.DifficultyMultiplier = 1.0;

            Game.ScreenManager.ChangeScreen(new MenuScreen(Game));
        }

        public override void OnExit()
        {
            Game.Window.TextInput -= OnTextInput;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            Texture2D background = Game.Content.Load<Texture2D>("Textures/Background/black");
            spriteBatch.Draw(background, Vector2.Zero, XnaColor.White);

            string title = "ENTER PSEUDO";
            Vector2 titleSize = _titleFont.MeasureString(title);

            spriteBatch.DrawString(
                _titleFont,
                title,
                new Vector2((Game.GraphicsDevice.Viewport.Width - titleSize.X) / 2f, 100),
                XnaColor.White
            );

            string pseudoText = _pseudo;

            if (pseudoText.Length == 0)
                pseudoText = "_";

            Vector2 pseudoSize = _font.MeasureString(pseudoText);

            spriteBatch.DrawString(
                _font,
                pseudoText,
                new Vector2((Game.GraphicsDevice.Viewport.Width - pseudoSize.X) / 2f, 200),
                XnaColor.White
            );

            string instruction = "Use letters, numbers, - or _";
            Vector2 instructionSize = _font.MeasureString(instruction);

            spriteBatch.DrawString(
                _font,
                instruction,
                new Vector2((Game.GraphicsDevice.Viewport.Width - instructionSize.X) / 2f, 240),
                XnaColor.White
            );

            _continueButton.Draw(spriteBatch, _font);

            spriteBatch.End();
        }
    }
}