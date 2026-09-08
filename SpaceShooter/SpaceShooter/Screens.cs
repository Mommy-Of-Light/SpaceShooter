namespace SpaceShooter
{
    public class NewGameScreen : GameScreen
    {
        private SpriteFont _font;
        private SpriteFont _font_title;

        private List<Button> _buttons;

        private MouseState _previousMouse;
        
        private KeyboardState _previousKeyboard;

        public NewGameScreen(Game1 game) : base(game)
        {
            _previousKeyboard = Keyboard.GetState();
        }

        public override void Initialize()
        {
            _font = Game.Content.Load<SpriteFont>("Fonts/SpaceInvader_12"); _font_title = Game.Content.Load<SpriteFont>("Fonts/SpaceInvader_16");

            _buttons = new List<Button>();

            int screenWidth = Game.GraphicsDevice.Viewport.Width;
            int screenHeight = Game.GraphicsDevice.Viewport.Height;

            int buttonWidth = 350;
            int buttonHeight = 60;

            int x = (screenWidth - buttonWidth) / 2;
            int startY = 100;
            int spacing = 75;

            _buttons.Add(new Button(
                "Create a new game",
                new XnaRectangle(x, startY, buttonWidth, buttonHeight)));

            _buttons.Add(new Button(
                "Return",
                new XnaRectangle(x, startY + spacing * 1, buttonWidth, buttonHeight)));
        }

        public override void Update(GameTime gameTime, KeyboardState keyboard)
        {
            if (keyboard.IsKeyDown(XnaKeys.Escape) &&
                _previousKeyboard.IsKeyUp(XnaKeys.Escape))
            {
                HandleButton("Return");
            }

            MouseState mouse = Mouse.GetState();

            foreach (Button button in _buttons)
            {
                button.Update(mouse);

                if (button.IsClicked(mouse, _previousMouse))
                {
                    HandleButton(button.Text);
                    break;
                }
            }

            _previousMouse = mouse;
            _previousKeyboard = keyboard;
        }

        private void HandleButton(string button)
        {
            switch (button)
            {
                case "Create a new game":
                    Game.ScreenManager.ChangeScreen(
                        new PlayScreen(Game));
                    break;

                case "Return":
                    Game.ScreenManager.ChangeScreen(
                        new MenuScreen(Game));
                    break;
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(Game.Content.Load<Texture2D>("Textures/Background/black"), Vector2.Zero, XnaColor.White);

            int screenWidth = Game.GraphicsDevice.Viewport.Width;

            string title = "NEW GAME";

            Vector2 titleSize = _font_title.MeasureString(title);

            spriteBatch.DrawString(
                _font_title,
                title,
                new Vector2(
                    (screenWidth - titleSize.X) / 2,
                    50),
                XnaColor.White);

            foreach (Button button in _buttons)
            {
                button.Draw(spriteBatch, _font);
            }

            spriteBatch.End();
        }
    }

    public class SaveScreen : GameScreen
    {
        private SpriteFont _font;
        private SpriteFont _font_title;

        private List<Button> _buttons;

        private MouseState _previousMouse;

        private KeyboardState _previousKeyboard;

        public SaveScreen(Game1 game) : base(game)
        {
            _previousKeyboard = Keyboard.GetState();
        }

        public override void Initialize()
        {
            _font = Game.Content.Load<SpriteFont>("Fonts/SpaceInvader_12"); _font_title = Game.Content.Load<SpriteFont>("Fonts/SpaceInvader_16");

            _buttons = new List<Button>();

            int screenWidth = Game.GraphicsDevice.Viewport.Width;
            int screenHeight = Game.GraphicsDevice.Viewport.Height;

            int buttonWidth = 350;
            int buttonHeight = 60;

            int x = (screenWidth - buttonWidth) / 2;
            int startY = 100;
            int spacing = 75;

            _buttons.Add(new Button(
                "Return",
                new XnaRectangle(x, startY, buttonWidth, buttonHeight)));
        }

        public override void Update(GameTime gameTime, KeyboardState keyboard)
        {
            if (keyboard.IsKeyDown(XnaKeys.Escape) &&
                _previousKeyboard.IsKeyUp(XnaKeys.Escape))
            {
                HandleButton("Return");
            }

            MouseState mouse = Mouse.GetState();

            foreach (Button button in _buttons)
            {
                button.Update(mouse);

                if (button.IsClicked(mouse, _previousMouse))
                {
                    HandleButton(button.Text);
                    break;
                }
            }

            _previousMouse = mouse;
            _previousKeyboard = keyboard;
        }

        private void HandleButton(string button)
        {
            switch (button)
            {
                case "Return":
                    Game.ScreenManager.ChangeScreen(
                        new MenuScreen(Game));
                    break;
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(Game.Content.Load<Texture2D>("Textures/Background/black"), Vector2.Zero, XnaColor.White);

            int screenWidth = Game.GraphicsDevice.Viewport.Width;

            string title = "SAVED GAMES";

            Vector2 titleSize = _font_title.MeasureString(title);

            spriteBatch.DrawString(
                _font_title,
                title,
                new Vector2(
                    (screenWidth - titleSize.X) / 2,
                    50),
                XnaColor.White);

            foreach (Button button in _buttons)
            {
                button.Draw(spriteBatch, _font);
            }

            spriteBatch.End();
        }
    }

    public class ArchiveScreen : GameScreen
    {
        private SpriteFont _font;
        private SpriteFont _font_title;

        private List<Button> _buttons;

        private MouseState _previousMouse;

        private KeyboardState _previousKeyboard;

        public ArchiveScreen(Game1 game) : base(game)
        {
            _previousKeyboard = Keyboard.GetState();
        }

        public override void Initialize()
        {
            _font = Game.Content.Load<SpriteFont>("Fonts/SpaceInvader_12"); _font_title = Game.Content.Load<SpriteFont>("Fonts/SpaceInvader_16");

            _buttons = new List<Button>();

            int screenWidth = Game.GraphicsDevice.Viewport.Width;
            int screenHeight = Game.GraphicsDevice.Viewport.Height;

            int buttonWidth = 350;
            int buttonHeight = 60;

            int x = (screenWidth - buttonWidth) / 2;
            int startY = 100;
            int spacing = 75;

            _buttons.Add(new Button(
                "Return",
                new XnaRectangle(x, startY, buttonWidth, buttonHeight)));
        }

        public override void Update(GameTime gameTime, KeyboardState keyboard)
        {
            if (keyboard.IsKeyDown(XnaKeys.Escape) &&
                _previousKeyboard.IsKeyUp(XnaKeys.Escape))
            {
                HandleButton("Return");
            }

            MouseState mouse = Mouse.GetState();

            foreach (Button button in _buttons)
            {
                button.Update(mouse);

                if (button.IsClicked(mouse, _previousMouse))
                {
                    HandleButton(button.Text);
                    break;
                }
            }

            _previousMouse = mouse;
            _previousKeyboard = keyboard;
        }

        private void HandleButton(string button)
        {
            switch (button)
            {
                case "Return":
                    Game.ScreenManager.ChangeScreen(
                        new MenuScreen(Game));
                    break;
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(Game.Content.Load<Texture2D>("Textures/Background/black"), Vector2.Zero, XnaColor.White);

            int screenWidth = Game.GraphicsDevice.Viewport.Width;

            string title = "ARCHIVED GAMES";

            Vector2 titleSize = _font_title.MeasureString(title);

            spriteBatch.DrawString(
                _font_title,
                title,
                new Vector2(
                    (screenWidth - titleSize.X) / 2,
                    50),
                XnaColor.White);

            foreach (Button button in _buttons)
            {
                button.Draw(spriteBatch, _font);
            }

            spriteBatch.End();
        }
    }

    public class RankingScreen : GameScreen
    {
        private SpriteFont _font;
        private SpriteFont _font_title;

        private List<Button> _buttons;

        private MouseState _previousMouse;

        private KeyboardState _previousKeyboard;

        public RankingScreen(Game1 game) : base(game)
        {
            _previousKeyboard = Keyboard.GetState();
        }

        public override void Initialize()
        {
            _font = Game.Content.Load<SpriteFont>("Fonts/SpaceInvader_12"); _font_title = Game.Content.Load<SpriteFont>("Fonts/SpaceInvader_16");

            _buttons = new List<Button>();

            int screenWidth = Game.GraphicsDevice.Viewport.Width;
            int screenHeight = Game.GraphicsDevice.Viewport.Height;

            int buttonWidth = 350;
            int buttonHeight = 60;

            int x = (screenWidth - buttonWidth) / 2;
            int startY = 100;
            int spacing = 75;

            _buttons.Add(new Button(
                "Return",
                new XnaRectangle(x, startY, buttonWidth, buttonHeight)));
        }

        public override void Update(GameTime gameTime, KeyboardState keyboard)
        {
            if (keyboard.IsKeyDown(XnaKeys.Escape) &&
                _previousKeyboard.IsKeyUp(XnaKeys.Escape))
            {
                HandleButton("Return");
            }

            MouseState mouse = Mouse.GetState();

            foreach (Button button in _buttons)
            {
                button.Update(mouse);

                if (button.IsClicked(mouse, _previousMouse))
                {
                    HandleButton(button.Text);
                    break;
                }
            }

            _previousMouse = mouse;
            _previousKeyboard = keyboard;
        }

        private void HandleButton(string button)
        {
            switch (button)
            {
                case "Return":
                    Game.ScreenManager.ChangeScreen(
                        new MenuScreen(Game));
                    break;
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(Game.Content.Load<Texture2D>("Textures/Background/black"), Vector2.Zero, XnaColor.White);

            int screenWidth = Game.GraphicsDevice.Viewport.Width;

            string title = "RANKING";

            Vector2 titleSize = _font_title.MeasureString(title);

            spriteBatch.DrawString(
                _font_title,
                title,
                new Vector2(
                    (screenWidth - titleSize.X) / 2,
                    50),
                XnaColor.White);

            foreach (Button button in _buttons)
            {
                button.Draw(spriteBatch, _font);
            }

            spriteBatch.End();
        }
    }

    public class PlayScreen : GameScreen
    {
        private SpriteFont _font;
        private SpriteFont _font_title;

        private List<Button> _buttons;
        private MouseState _previousMouse;
        private KeyboardState _previousKeyboard;

        private Player _player;

        private bool _isPaused;
        private Texture2D _pixel;

        private Button _continueButton;
        private Button _restartButton;
        private Button _exitButton;

        public PlayScreen(Game1 game) : base(game)
        {
            _previousKeyboard = Keyboard.GetState();
            Game.ChangeScreenSize(400, 800);
        }

        public override void Initialize()
        {
            _font = Game.Content.Load<SpriteFont>("Fonts/SpaceInvader_12");
            _font_title = Game.Content.Load<SpriteFont>("Fonts/SpaceInvader_16");

            Texture2D playerTexture =
                Game.Content.Load<Texture2D>("Textures/PNG/playerShip1_blue");
            Texture2D projectileTexture =
                Game.Content.Load<Texture2D>("Textures/PNG/Lazers/laserBlue01");

            _player = new Player(
                playerTexture,
                projectileTexture,
                new Vector2((Game.GraphicsDevice.Viewport.Width - playerTexture.Width) / 2, 700),
                300f);

            _buttons = new List<Button>();

            int screenWidth = Game.GraphicsDevice.Viewport.Width;
            int screenHeight = Game.GraphicsDevice.Viewport.Height;

            int pauseWidth = 60;
            int pauseHeight = 45;

            _buttons.Add(new Button(
                "||",
                new XnaRectangle(
                    screenWidth - pauseWidth - 10,
                    10,
                    pauseWidth,
                    pauseHeight)));

            int buttonWidth = 280;
            int buttonHeight = 55;
            int buttonX = (screenWidth - buttonWidth) / 2;

            int panelHeight = 360;
            int panelY = (screenHeight - panelHeight) / 2;
            int startY = panelY + 115;
            int spacing = 70;

            _continueButton = new Button(
                "Continue",
                new XnaRectangle(buttonX, startY, buttonWidth, buttonHeight));

            _restartButton = new Button(
                "Restart",
                new XnaRectangle(
                    buttonX,
                    startY + spacing,
                    buttonWidth,
                    buttonHeight));

            _exitButton = new Button(
                "Exit",
                new XnaRectangle(
                    buttonX,
                    startY + spacing * 2,
                    buttonWidth,
                    buttonHeight));

            _pixel = new Texture2D(Game.GraphicsDevice, 1, 1);
            _pixel.SetData(new[] { XnaColor.White });
        }

        public override void Update(GameTime gameTime, KeyboardState keyboard)
        {
            // Escape toggles pause
            if (keyboard.IsKeyDown(XnaKeys.Escape) &&
                _previousKeyboard.IsKeyUp(XnaKeys.Escape))
            {
                _isPaused = !_isPaused;
            }

            MouseState mouse = Mouse.GetState();

            // Always update previous keyboard state
            _previousKeyboard = keyboard;

            // If paused, only process pause-menu buttons
            if (_isPaused)
            {
                _continueButton.Update(mouse);
                _restartButton.Update(mouse);
                _exitButton.Update(mouse);

                if (_continueButton.IsClicked(mouse, _previousMouse))
                {
                    _isPaused = false;
                }
                else if (_restartButton.IsClicked(mouse, _previousMouse))
                {
                    ResetGame();
                    _isPaused = false;
                }
                else if (_exitButton.IsClicked(mouse, _previousMouse))
                {
                    Game.ScreenManager.ChangeScreen(
                        new NewGameScreen(Game));

                    _previousMouse = mouse;
                    return;
                }

                _previousMouse = mouse;
                return;
            }

            // Normal gameplay
            _player.Update(gameTime, Game);

            foreach (Button button in _buttons)
            {
                button.Update(mouse);

                if (button.IsClicked(mouse, _previousMouse))
                {
                    if (button.Text == "||")
                    {
                        _isPaused = true;
                    }
                    break;
                }
            }

            _previousMouse = mouse;
        }

        private void ResetGame()
        {
            Texture2D playerTexture =
                Game.Content.Load<Texture2D>("Textures/PNG/playerShip1_blue");

            Texture2D projectileTexture =
                Game.Content.Load<Texture2D>("Textures/PNG/Lazers/laserBlue01");

            _player = new Player(
                playerTexture,
                projectileTexture,
                new Vector2((Game.GraphicsDevice.Viewport.Width - playerTexture.Width) / 2, 700),
                300f);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(
                Game.Content.Load<Texture2D>("Textures/Background/black"),
                Vector2.Zero,
                XnaColor.White);

            foreach (Button button in _buttons)
            {
                button.Draw(spriteBatch, _font);
            }

            _player.Draw(gameTime, spriteBatch);

            if (_isPaused)
            {
                int screenWidth = Game.GraphicsDevice.Viewport.Width;
                int screenHeight = Game.GraphicsDevice.Viewport.Height;

                spriteBatch.Draw(
                    _pixel,
                    new XnaRectangle(0, 0, screenWidth, screenHeight),
                    new XnaColor(0, 0, 0, 180));

                int panelWidth = 330;
                int panelHeight = 360;
                int panelX = (screenWidth - panelWidth) / 2;
                int panelY = (screenHeight - panelHeight) / 2;

                spriteBatch.Draw(
                    _pixel,
                    new XnaRectangle(
                        panelX,
                        panelY,
                        panelWidth,
                        panelHeight),
                    new XnaColor(15, 15, 15, 245));

                string title = "PAUSED";
                Vector2 titleSize = _font_title.MeasureString(title);

                spriteBatch.DrawString(
                    _font_title,
                    title,
                    new Vector2(
                        (screenWidth - titleSize.X) / 2,
                        panelY + 35),
                    XnaColor.White);

                _continueButton.Draw(spriteBatch, _font);
                _restartButton.Draw(spriteBatch, _font);
                _exitButton.Draw(spriteBatch, _font);
            }

            spriteBatch.End();
        }
    }
}