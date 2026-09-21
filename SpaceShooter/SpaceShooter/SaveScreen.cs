namespace SpaceShooter
{
    public class SaveScreen : GameScreen
    {
        private SpriteFont _font;
        private SpriteFont _font_title;
        private List<Button> _buttons;
        private KeyboardState _previousKeyboard;

        public SaveScreen(Game1 game) : base(game)
        {
            _previousKeyboard = Keyboard.GetState();
            Game.ChangeScreenSize(500, 500);
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

            _buttons.Add(new Button("Return", new XnaRectangle(x, startY, buttonWidth, buttonHeight)));
        }

        public override void Update(GameTime gameTime, KeyboardState keyboard, Vector2 mousePosition, bool mouseClicked)
        {
            if (keyboard.IsKeyDown(XnaKeys.Escape) && _previousKeyboard.IsKeyUp(XnaKeys.Escape))
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
                    Game.ScreenManager.ChangeScreen(new MenuScreen(Game));
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

            spriteBatch.DrawString(_font_title, title, new Vector2((screenWidth - titleSize.X) / 2, 50), XnaColor.White);

            foreach (Button button in _buttons)
            {
                button.Draw(spriteBatch, _font);
            }

            spriteBatch.End();
        }
    }

}
