using Microsoft.VisualBasic.Devices;

namespace SpaceShooter
{
    /// <summary>
    /// Represents the main menu screen of the game.
    /// Provides navigation to the different game features and allows
    /// interaction through the mouse and keyboard.
    /// </summary>
    public class MenuScreen : GameScreen
    {
        /// <summary>
        /// Font used to display menu button text.
        /// </summary>
        private SpriteFont _font;

        /// <summary>
        /// Font used to display the menu title.
        /// </summary>
        private SpriteFont _font_title;

        /// <summary>
        /// Collection of buttons displayed on the menu.
        /// </summary>
        private List<Button> _buttons;

        /// <summary>
        /// Stores the keyboard state from the previous update.
        /// Used to detect individual key presses.
        /// </summary>
        private KeyboardState _previousKeyboard;

        /// <summary>
        /// Initializes a new instance of the <see cref="MenuScreen"/> class.
        /// </summary>
        /// <param name="game">The main game instance associated with this screen.</param>
        public MenuScreen(Game1 game) : base(game)
        {
            _previousKeyboard = Microsoft.Xna.Framework.Input.Keyboard.GetState();
            Game.ChangeScreenSize(500, 500);
        }

        /// <summary>
        /// Initializes the fonts and creates the buttons displayed on the menu.
        /// </summary>
        public override void Initialize()
        {
            _font = Game.Content.Load<SpriteFont>("Fonts/SpaceInvader_12");
            _font_title = Game.Content.Load<SpriteFont>("Fonts/SpaceInvader_16");

            _buttons = new List<Button>();

            int screenWidth = Game.GraphicsDevice.Viewport.Width;
            int screenHeight = Game.GraphicsDevice.Viewport.Height;

            int buttonWidth = 350;
            int buttonHeight = 60;

            int x = (screenWidth - buttonWidth) / 2;
            int startY = 100;
            int spacing = 75;

            _buttons.Add(new Button("New Game", new XnaRectangle(x, startY, buttonWidth, buttonHeight)));
            _buttons.Add(new Button("Save", new XnaRectangle(x, startY + spacing, buttonWidth, buttonHeight)));
            _buttons.Add(new Button("Archive", new XnaRectangle(x, startY + spacing * 2, buttonWidth, buttonHeight)));
            _buttons.Add(new Button("Ranking", new XnaRectangle(x, startY + spacing * 3, buttonWidth, buttonHeight)));
            _buttons.Add(new Button("Exit", new XnaRectangle(x, startY + spacing * 4, buttonWidth, buttonHeight)));

            if (_buttons.Count > 0)
            {
                _buttons[0].SetSelected(true);
            }
        }

        /// <summary>
        /// Updates the menu, processes keyboard and mouse input,
        /// and handles navigation between menu options.
        /// </summary>
        /// <param name="gameTime">Provides timing information for the update.</param>
        /// <param name="keyboard">The current keyboard state.</param>
        /// <param name="mousePosition">The current position of the mouse cursor.</param>
        /// <param name="mouseClicked">Indicates whether the mouse was clicked during this update.</param>
        public override void Update(GameTime gameTime, KeyboardState keyboard, Vector2 mousePosition, bool mouseClicked)
        {
            if (keyboard.IsKeyDown(XnaKeys.Escape) && _previousKeyboard.IsKeyUp(XnaKeys.Escape))
            {
                Game.Exit();
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
                }
            }

            _previousKeyboard = keyboard;
        }

        /// <summary>
        /// Handles the action associated with the selected menu button.
        /// </summary>
        /// <param name="button">The text of the selected button.</param>
        private void HandleButton(string button)
        {
            switch (button)
            {
                case "New Game":
                    Game.ScreenManager.ChangeScreen(new NewGameScreen(Game));
                    break;

                case "Save":
                    Game.ScreenManager.ChangeScreen(new SaveScreen(Game));
                    break;

                case "Archive":
                    Game.ScreenManager.ChangeScreen(new ArchiveScreen(Game));
                    break;

                case "Ranking":
                    Game.ScreenManager.ChangeScreen(new RankingScreen(Game));
                    break;

                case "Exit":
                    Game.Exit();
                    break;
            }
        }

        /// <summary>
        /// Draws the menu background, title, and buttons.
        /// </summary>
        /// <param name="gameTime">Provides timing information for the draw operation.</param>
        /// <param name="spriteBatch">The sprite batch used to draw the menu.</param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(Game.Content.Load<Texture2D>("Textures/Background/black"), Vector2.Zero, XnaColor.White);

            int screenWidth = Game.GraphicsDevice.Viewport.Width;

            string title = "SPACE SHOOTER";
            Vector2 titleSize = _font_title.MeasureString(title);

            spriteBatch.DrawString(_font_title, title, new Vector2((screenWidth - titleSize.X) / 2, 50), XnaColor.White);

            foreach (Button button in _buttons)
            {
                button.Draw(spriteBatch, _font);
            }

            spriteBatch.End();
        }
    }
}