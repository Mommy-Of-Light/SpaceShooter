namespace SpaceShooter
{
    public class NewGameScreen : GameScreen
    {
        private SpriteFont _font;
        private SpriteFont _font_title;

        private List<Button> _buttons;

        private KeyboardState _previousKeyboard;

        public NewGameScreen(Game1 game) : base(game)
        {
            _previousKeyboard = Keyboard.GetState();
        }

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

            _buttons.Add(new Button(
                "Create a new game",
                new XnaRectangle(x, startY, buttonWidth, buttonHeight)));

            _buttons.Add(new Button(
                "Return",
                new XnaRectangle(
                    x,
                    startY + spacing,
                    buttonWidth,
                    buttonHeight)));
        }

        public override void Update(
            GameTime gameTime,
            KeyboardState keyboard,
            Vector2 mousePosition,
            bool mouseClicked)
        {
            if (keyboard.IsKeyDown(XnaKeys.Escape) &&
                _previousKeyboard.IsKeyUp(XnaKeys.Escape))
            {
                HandleButton("Return");
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

            spriteBatch.Draw(
                Game.Content.Load<Texture2D>(
                    "Textures/Background/black"),
                Vector2.Zero,
                XnaColor.White);

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

        private KeyboardState _previousKeyboard;

        public SaveScreen(Game1 game) : base(game)
        {
            _previousKeyboard = Keyboard.GetState();
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

            _buttons.Add(new Button(
                "Return",
                new XnaRectangle(
                    x,
                    startY,
                    buttonWidth,
                    buttonHeight)));
        }

        public override void Update(
            GameTime gameTime,
            KeyboardState keyboard,
            Vector2 mousePosition,
            bool mouseClicked)
        {
            if (keyboard.IsKeyDown(XnaKeys.Escape) &&
                _previousKeyboard.IsKeyUp(XnaKeys.Escape))
            {
                HandleButton("Return");
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
                case "Return":
                    Game.ScreenManager.ChangeScreen(
                        new MenuScreen(Game));
                    break;
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(
                Game.Content.Load<Texture2D>(
                    "Textures/Background/black"),
                Vector2.Zero,
                XnaColor.White);

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

        private KeyboardState _previousKeyboard;

        public ArchiveScreen(Game1 game) : base(game)
        {
            _previousKeyboard = Keyboard.GetState();
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

            _buttons.Add(new Button(
                "Return",
                new XnaRectangle(
                    x,
                    startY,
                    buttonWidth,
                    buttonHeight)));
        }

        public override void Update(
            GameTime gameTime,
            KeyboardState keyboard,
            Vector2 mousePosition,
            bool mouseClicked)
        {
            if (keyboard.IsKeyDown(XnaKeys.Escape) &&
                _previousKeyboard.IsKeyUp(XnaKeys.Escape))
            {
                HandleButton("Return");
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
                case "Return":
                    Game.ScreenManager.ChangeScreen(
                        new MenuScreen(Game));
                    break;
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(
                Game.Content.Load<Texture2D>(
                    "Textures/Background/black"),
                Vector2.Zero,
                XnaColor.White);

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

        private KeyboardState _previousKeyboard;

        public RankingScreen(Game1 game) : base(game)
        {
            _previousKeyboard = Keyboard.GetState();
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

            _buttons.Add(new Button(
                "Return",
                new XnaRectangle(
                    x,
                    startY,
                    buttonWidth,
                    buttonHeight)));
        }

        public override void Update(
            GameTime gameTime,
            KeyboardState keyboard,
            Vector2 mousePosition,
            bool mouseClicked)
        {
            if (keyboard.IsKeyDown(XnaKeys.Escape) &&
                _previousKeyboard.IsKeyUp(XnaKeys.Escape))
            {
                HandleButton("Return");
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
                case "Return":
                    Game.ScreenManager.ChangeScreen(
                        new MenuScreen(Game));
                    break;
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(
                Game.Content.Load<Texture2D>(
                    "Textures/Background/black"),
                Vector2.Zero,
                XnaColor.White);

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
        private Player _player;

        private Texture2D _playerTexture;
        private Texture2D _projectileTexture;

        private SpriteFont _font;

        private List<Button> _buttons;

        private Button _continueButton;
        private Button _restartButton;
        private Button _exitButton;

        private KeyboardState _previousKeyboard;

        private bool _isPaused;

        public PlayScreen(Game1 game) : base(game)
        {
            _previousKeyboard = Keyboard.GetState();

            Game.ChangeScreenSize(400, 800);
        }

        public override void Initialize()
        {
            // -------------------------
            // LOAD CONTENT
            // -------------------------

            _font = Game.Content.Load<SpriteFont>(
                "Fonts/SpaceInvader_12"
            );

            /*
             * REMPLACE ces deux chemins par les chemins
             * que tu utilisais déjà dans ton ancien PlayScreen.
             */
            _playerTexture = Game.Content.Load<Texture2D>(
                "Textures/PNG/player"
            );

            _projectileTexture = Game.Content.Load<Texture2D>(
                "Textures/PNG/projectile"
            );


            // -------------------------
            // CREATE PLAYER
            // -------------------------

            Vector2 playerPosition = new Vector2(
                (Game.GraphicsDevice.Viewport.Width -
                 _playerTexture.Width) / 2f,

                Game.GraphicsDevice.Viewport.Height -
                _playerTexture.Height -
                30
            );

            _player = new Player(
                _playerTexture,
                _projectileTexture,
                playerPosition,
                300f
            );


            // -------------------------
            // BUTTONS
            // -------------------------

            _buttons = new List<Button>();

            _buttons.Add(
                new Button(
                    "||",
                    new XnaRectangle(
                        10,
                        10,
                        50,
                        50
                    )
                )
            );


            // -------------------------
            // PAUSE MENU
            // -------------------------

            _continueButton = new Button(
                "Continue",
                new XnaRectangle(
                    100,
                    300,
                    200,
                    50
                )
            );

            _restartButton = new Button(
                "Restart",
                new XnaRectangle(
                    100,
                    370,
                    200,
                    50
                )
            );

            _exitButton = new Button(
                "Exit",
                new XnaRectangle(
                    100,
                    440,
                    200,
                    50
                )
            );
        }

        public override void Update(
            GameTime gameTime,
            KeyboardState keyboard,
            Vector2 mousePosition,
            bool mouseClicked)
        {
            // -------------------------
            // ESCAPE
            // -------------------------

            if (keyboard.IsKeyDown(XnaKeys.Escape) &&
                _previousKeyboard.IsKeyUp(XnaKeys.Escape))
            {
                _isPaused = !_isPaused;
            }


            // -------------------------
            // PAUSED
            // -------------------------

            if (_isPaused)
            {
                _continueButton.Update(mousePosition);
                _restartButton.Update(mousePosition);
                _exitButton.Update(mousePosition);

                if (_continueButton.IsClicked(
                    mousePosition,
                    mouseClicked))
                {
                    _isPaused = false;
                }
                else if (_restartButton.IsClicked(
                    mousePosition,
                    mouseClicked))
                {
                    ResetGame();

                    _isPaused = false;
                }
                else if (_exitButton.IsClicked(
                    mousePosition,
                    mouseClicked))
                {
                    Game.ScreenManager.ChangeScreen(
                        new NewGameScreen(Game)
                    );

                    return;
                }

                _previousKeyboard = keyboard;

                return;
            }


            // -------------------------
            // PLAYER
            // -------------------------

            _player.Update(
                gameTime,
                Game
            );


            // -------------------------
            // PAUSE BUTTON
            // -------------------------

            foreach (Button button in _buttons)
            {
                button.Update(mousePosition);

                if (button.IsClicked(
                    mousePosition,
                    mouseClicked))
                {
                    if (button.Text == "||")
                    {
                        _isPaused = true;
                    }

                    break;
                }
            }


            _previousKeyboard = keyboard;
        }

        private void ResetGame()
        {
            Vector2 playerPosition = new Vector2(
                (Game.GraphicsDevice.Viewport.Width -
                 _playerTexture.Width) / 2f,

                Game.GraphicsDevice.Viewport.Height -
                _playerTexture.Height -
                30
            );

            _player = new Player(
                _playerTexture,
                _projectileTexture,
                playerPosition,
                300f
            );
        }

        public override void Draw(
            GameTime gameTime,
            SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();


            // -------------------------
            // BACKGROUND
            // -------------------------

            spriteBatch.Draw(
                Game.Content.Load<Texture2D>(
                    "Textures/Background/black"
                ),
                Vector2.Zero,
                XnaColor.White
            );


            // -------------------------
            // PLAYER
            // -------------------------

            _player.Draw(
                gameTime,
                spriteBatch
            );


            // -------------------------
            // PAUSE BUTTON
            // -------------------------

            foreach (Button button in _buttons)
            {
                button.Draw(
                    spriteBatch,
                    _font
                );
            }


            // -------------------------
            // PAUSE MENU
            // -------------------------

            if (_isPaused)
            {
                _continueButton.Draw(
                    spriteBatch,
                    _font
                );

                _restartButton.Draw(
                    spriteBatch,
                    _font
                );

                _exitButton.Draw(
                    spriteBatch,
                    _font
                );
            }


            spriteBatch.End();
        }
    }
}