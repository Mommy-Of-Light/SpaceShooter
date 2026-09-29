namespace SpaceShooter
{
    /// <summary>
    /// Represents the screen where the player enters their pseudo before starting the game.
    /// </summary>
    public class PseudoScreen : GameScreen
    {
        /// <summary>
        /// Font used for regular text displayed on the screen.
        /// </summary>
        private SpriteFont _font;

        /// <summary>
        /// Font used for the screen title.
        /// </summary>
        private SpriteFont _titleFont;

        /// <summary>
        /// Button used to confirm the entered pseudo and continue.
        /// </summary>
        private Button _continueButton;

        /// <summary>
        /// Stores the keyboard state from the previous update.
        /// </summary>
        private KeyboardState _previousKeyboard;

        /// <summary>
        /// Pseudo currently entered by the player.
        /// </summary>
        private string _pseudo;

        /// <summary>
        /// Initializes a new instance of the <see cref="PseudoScreen"/> class.
        /// </summary>
        /// <param name="game">Reference to the main game instance.</param>
        public PseudoScreen(Game1 game) : base(game)
        {
            _previousKeyboard = Keyboard.GetState();
            _pseudo = "";
            Game.ChangeScreenSize(500, 500);
        }

        /// <summary>
        /// Loads the fonts, creates the continue button, and registers the text input event.
        /// </summary>
        public override void Initialize()
        {
            _font = Game.Content.Load<SpriteFont>("Fonts/SpaceInvader_12");
            _titleFont = Game.Content.Load<SpriteFont>("Fonts/SpaceInvader_16");

            _continueButton = new Button("Continue", new XnaRectangle(150, 300, 200, 50));

            Game.Window.TextInput += OnTextInput;
        }

        /// <summary>
        /// Handles text input from the keyboard and adds valid characters to the pseudo.
        /// </summary>
        /// <param name="sender">Object that raised the text input event.</param>
        /// <param name="e">Contains information about the entered character.</param>
        private void OnTextInput(object sender, TextInputEventArgs e)
        {
            if (char.IsControl(e.Character))
                return;

            if (_pseudo.Length >= 16)
                return;

            if (char.IsLetterOrDigit(e.Character) || e.Character == '_' || e.Character == '-')
                _pseudo += e.Character;
        }

        /// <summary>
        /// Updates the pseudo input, button state, and input controls.
        /// </summary>
        /// <param name="gameTime">Provides timing information for the current update.</param>
        /// <param name="keyboard">Current keyboard state.</param>
        /// <param name="mousePosition">Current position of the mouse cursor.</param>
        /// <param name="mouseClicked">Indicates whether the mouse button was clicked during this update.</param>
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

        /// <summary>
        /// Starts the game after validating and storing the player's pseudo.
        /// </summary>
        private void StartGame()
        {
            if (string.IsNullOrWhiteSpace(_pseudo))
                return;

            Game.PlayerPseudo = _pseudo.Trim();
            Game.Difficulty = "Medium";
            Game.DifficultyMultiplier = 1.0;

            Game.ScreenManager.ChangeScreen(new MenuScreen(Game));
        }

        /// <summary>
        /// Unregisters the text input event when leaving the screen.
        /// </summary>
        public override void OnExit()
        {
            Game.Window.TextInput -= OnTextInput;
        }

        /// <summary>
        /// Draws the background, title, pseudo input, instructions, and continue button.
        /// </summary>
        /// <param name="gameTime">Provides timing information for the current game frame.</param>
        /// <param name="spriteBatch">The sprite batch used to draw the screen.</param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            Texture2D background = Game.Content.Load<Texture2D>("Textures/Background/black");
            spriteBatch.Draw(background, Vector2.Zero, XnaColor.White);

            string title = "ENTER PSEUDO";
            Vector2 titleSize = _titleFont.MeasureString(title);

            spriteBatch.DrawString(_titleFont, title, new Vector2((Game.GraphicsDevice.Viewport.Width - titleSize.X) / 2f, 100), XnaColor.White);

            string pseudoText = _pseudo;

            if (pseudoText.Length == 0)
                pseudoText = "_";

            Vector2 pseudoSize = _font.MeasureString(pseudoText);

            spriteBatch.DrawString(_font, pseudoText, new Vector2((Game.GraphicsDevice.Viewport.Width - pseudoSize.X) / 2f, 200), XnaColor.White);

            string instruction = "Use letters, numbers, - or _";
            Vector2 instructionSize = _font.MeasureString(instruction);

            spriteBatch.DrawString(_font, instruction, new Vector2((Game.GraphicsDevice.Viewport.Width - instructionSize.X) / 2f, 240), XnaColor.White);

            _continueButton.Draw(spriteBatch, _font);

            spriteBatch.End();
        }
    }
}