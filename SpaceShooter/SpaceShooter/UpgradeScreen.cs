namespace SpaceShooter
{
    /// <summary>
    /// Represents the screen where the player can choose an upgrade for their ship.
    /// </summary>
    public class UpgradeScreen : GameScreen
    {
        /// <summary>
        /// Font used for displaying upgrade button text.
        /// </summary>
        private SpriteFont _font;

        /// <summary>
        /// Font used for displaying the screen title.
        /// </summary>
        private SpriteFont _titleFont;

        /// <summary>
        /// Button used to upgrade the missile attack speed.
        /// </summary>
        private Button _fireRateButton;

        /// <summary>
        /// Button used to increase the projectile pierce value.
        /// </summary>
        private Button _pierceButton;

        /// <summary>
        /// Button used to increase the number of auto-aim missiles.
        /// </summary>
        private Button _missileButton;

        /// <summary>
        /// Button used to increase the player's damage.
        /// </summary>
        private Button _damageButton;

        /// <summary>
        /// Stores the keyboard state from the previous update.
        /// </summary>
        private KeyboardState _previousKeyboard;

        /// <summary>
        /// Reference to the current play screen.
        /// </summary>
        private PlayScreen _playScreen;

        /// <summary>
        /// Maximum amount of pierce the player can have.
        /// </summary>
        private const int MaxPierce = 10;

        /// <summary>
        /// Maximum number of auto-aim missiles the player can have.
        /// </summary>
        private const int MaxMissiles = 15;

        /// <summary>
        /// Minimum number of shots until a missile can be fired.
        /// </summary>
        private const int MaxShotsUntilMissile = 1;

        /// <summary>
        /// Starting number of shots until a missile can be fired.
        /// </summary>
        private const int StartShotsUntilMissile = 5;

        /// <summary>
        /// List containing all upgrade buttons displayed on the screen.
        /// </summary>
        private List<Button> _buttons;

        /// <summary>
        /// The current missile fire rate of the player's ship.
        /// </summary>
        private int missileFireRate;

        /// <summary>
        /// The current pierce value of the player's projectiles.
        /// </summary>
        private int pierce;

        /// <summary>
        /// The current number of auto-aim missiles available to the player.
        /// </summary>
        private int missiles;

        /// <summary>
        /// The current damage value of the player's ship.
        /// </summary>
        private int damage;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpgradeScreen"/> class.
        /// </summary>
        /// <param name="game">Reference to the main game instance.</param>
        /// <param name="playScreen">Reference to the current play screen.</param>
        /// <param name="missileFireRate">The current missile fire rate.</param>
        /// <param name="pierce">The current pierce value.</param>
        /// <param name="missiles">The current number of auto-aim missiles.</param>
        /// <param name="damage">The current damage value.</param>
        public UpgradeScreen(Game1 game, PlayScreen playScreen, int missileFireRate, int pierce, int missiles, int damage) : base(game)
        {
            _playScreen = playScreen;
            _previousKeyboard = Keyboard.GetState();
            Game.ChangeScreenSize(500, 500);
            this.missileFireRate = missileFireRate;
            this.pierce = pierce;
            this.missiles = missiles;
            this.damage = damage;
        }

        /// <summary>
        /// Loads the fonts, creates the upgrade buttons, and initializes their states.
        /// </summary>
        public override void Initialize()
        {
            _font = Game.Content.Load<SpriteFont>("Fonts/SpaceInvader_12");
            _titleFont = Game.Content.Load<SpriteFont>("Fonts/SpaceInvader_16");

            _fireRateButton = new Button("MISSILE ATK SPD " + (StartShotsUntilMissile - missileFireRate + 1) + "/" + StartShotsUntilMissile  + " (1)", new XnaRectangle(50, 130, 400, 50));
            _pierceButton = new Button("PIERCE +1 " + pierce + "/" + MaxPierce + " (2)", new XnaRectangle(50, 200, 400, 50));
            _missileButton = new Button("MISSILE +1 " + missiles + "/" + MaxMissiles + " (3)", new XnaRectangle(50, 270, 400, 50));
            _damageButton = new Button("DAMAGE +1 " + damage + " (4)", new XnaRectangle(50, 340, 400, 50));

            _buttons = new List<Button>
            {
                _fireRateButton,
                _pierceButton,
                _missileButton,
                _damageButton
            };

            if (_buttons.Count > 0)
            {
                _buttons[0].SetSelected(true);
            }

            UpdateButtonStates();
        }

        /// <summary>
        /// Updates the upgrade buttons and handles mouse and keyboard input.
        /// </summary>
        /// <param name="gameTime">Provides timing information for the current update.</param>
        /// <param name="keyboard">Current keyboard state.</param>
        /// <param name="mousePosition">Current position of the mouse cursor.</param>
        /// <param name="mouseClicked">Indicates whether the mouse button was clicked during this update.</param>
        public override void Update(GameTime gameTime, KeyboardState keyboard, Vector2 mousePosition, bool mouseClicked)
        {
            _fireRateButton.Update(mousePosition);
            _pierceButton.Update(mousePosition);
            _missileButton.Update(mousePosition);
            _damageButton.Update(mousePosition);

            if (_fireRateButton.IsClicked(mousePosition, mouseClicked))
            {
                ChooseUpgrade(0);
                return;
            }

            if (_pierceButton.IsClicked(mousePosition, mouseClicked))
            {
                ChooseUpgrade(1);
                return;
            }

            if (_missileButton.IsClicked(mousePosition, mouseClicked))
            {
                ChooseUpgrade(2);
                return;
            }

            if (_damageButton.IsClicked(mousePosition, mouseClicked))
            {
                ChooseUpgrade(3);
                return;
            }

            if ((keyboard.IsKeyDown(XnaKeys.D1) && _previousKeyboard.IsKeyUp(XnaKeys.D1)) || (keyboard.IsKeyDown(XnaKeys.NumPad1) && _previousKeyboard.IsKeyUp(XnaKeys.NumPad1)) && !_fireRateButton.IsLocked)
            {
                ChooseUpgrade(0);
                return;
            }

            if ((keyboard.IsKeyDown(XnaKeys.D2) && _previousKeyboard.IsKeyUp(XnaKeys.D2)) || (keyboard.IsKeyDown(XnaKeys.NumPad2) && _previousKeyboard.IsKeyUp(XnaKeys.NumPad2)) && !_pierceButton.IsLocked)
            {
                ChooseUpgrade(1);
                return;
            }

            if ((keyboard.IsKeyDown(XnaKeys.D3) && _previousKeyboard.IsKeyUp(XnaKeys.D3)) || (keyboard.IsKeyDown(XnaKeys.NumPad3) && _previousKeyboard.IsKeyUp(XnaKeys.NumPad3)) && !_missileButton.IsLocked)
            {
                ChooseUpgrade(2);
                return;
            }

            if ((keyboard.IsKeyDown(XnaKeys.D4) && _previousKeyboard.IsKeyUp(XnaKeys.D4)) || (keyboard.IsKeyDown(XnaKeys.NumPad4) && _previousKeyboard.IsKeyUp(XnaKeys.NumPad4)) && !_damageButton.IsLocked)
            {
                ChooseUpgrade(3);
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

            if ((keyboard.IsKeyDown(XnaKeys.Enter) && _previousKeyboard.IsKeyUp(XnaKeys.Enter)) || (keyboard.IsKeyDown(XnaKeys.Space) && _previousKeyboard.IsKeyUp(XnaKeys.Space)))
            {
                int selectedIndex = _buttons.FindIndex(b => b.IsSelected);

                if (selectedIndex != -1 && !_buttons[selectedIndex].IsLocked)
                {
                    ChooseUpgrade(selectedIndex);
                    return;
                }
            }

            _previousKeyboard = keyboard;
        }

        /// <summary>
        /// Updates the locked state of each upgrade button based on the player's current upgrades.
        /// </summary>
        private void UpdateButtonStates()
        {
            _fireRateButton.SetLocked(_playScreen.Player.ShotsUntilMissile <= MaxShotsUntilMissile);
            _pierceButton.SetLocked(_playScreen.Player.Pierce >= MaxPierce);
            _missileButton.SetLocked(_playScreen.Player.AutoAimMissiles >= MaxMissiles);
        }

        /// <summary>
        /// Applies the selected upgrade and returns to the play screen.
        /// </summary>
        /// <param name="upgrade">Index of the upgrade that was selected.</param>
        private void ChooseUpgrade(int upgrade)
        {
            Game.ChangeScreenSize(500, 800);
            _playScreen.ApplyUpgrade(upgrade);
            Game.ScreenManager.ReturnToScreen(_playScreen);
        }

        /// <summary>
        /// Draws the upgrade selection screen, including the background, title, and upgrade buttons.
        /// </summary>
        /// <param name="gameTime">Provides timing information for the current game frame.</param>
        /// <param name="spriteBatch">The sprite batch used to draw the screen.</param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            Texture2D background = Game.Content.Load<Texture2D>("Textures/Background/black");
            spriteBatch.Draw(background, Vector2.Zero, XnaColor.White);

            string title = "CHOOSE UPGRADE";
            Vector2 titleSize = _titleFont.MeasureString(title);
            spriteBatch.DrawString(_titleFont, title, new Vector2((Game.GraphicsDevice.Viewport.Width - titleSize.X) / 2f, 50), XnaColor.White);

            _fireRateButton.Draw(spriteBatch, _font);
            _pierceButton.Draw(spriteBatch, _font);
            _missileButton.Draw(spriteBatch, _font);
            _damageButton.Draw(spriteBatch, _font);

            spriteBatch.End();
        }
    }
}