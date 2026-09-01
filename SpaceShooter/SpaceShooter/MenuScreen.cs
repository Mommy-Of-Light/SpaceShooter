using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace SpaceShooter
{
    public class MenuScreen : GameScreen
    {
        private SpriteFont _font;
        private SpriteFont _font_title;

        private List<Button> _buttons;

        private MouseState _previousMouse;

        public MenuScreen(Game1 game) : base(game)
        {
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
                "New Game",
                new Rectangle(x, startY, buttonWidth, buttonHeight)));

            _buttons.Add(new Button(
                "Save",
                new Rectangle(x, startY + spacing, buttonWidth, buttonHeight)));

            _buttons.Add(new Button(
                "Archive",
                new Rectangle(x, startY + spacing * 2, buttonWidth, buttonHeight)));

            _buttons.Add(new Button(
                "Ranking",
                new Rectangle(x, startY + spacing * 3, buttonWidth, buttonHeight)));

            _buttons.Add(new Button(
                "Exit",
                new Rectangle(x, startY + spacing * 4, buttonWidth, buttonHeight)));
        }

        public override void Update(GameTime gameTime)
        {
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
        }

        private void HandleButton(string button)
        {
            switch (button)
            {
                case "New Game":
                    Game.ScreenManager.ChangeScreen(
                        new NewGameScreen(Game));
                    break;

                case "Save":
                    Game.ScreenManager.ChangeScreen(
                        new SaveScreen(Game));
                    break;

                case "Archive":
                    Game.ScreenManager.ChangeScreen(
                        new ArchiveScreen(Game));
                    break;

                case "Ranking":
                    Game.ScreenManager.ChangeScreen(
                        new RankingScreen(Game));
                    break;

                case "Exit":
                    Game.Exit();
                    break;
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(Game.Content.Load<Texture2D>("Textures/Background/black"), Vector2.Zero, Color.White);

            int screenWidth = Game.GraphicsDevice.Viewport.Width;

            string title = "SPACE SHOOTER";

            Vector2 titleSize = _font_title.MeasureString(title);

            spriteBatch.DrawString(
                _font_title,
                title,
                new Vector2(
                    (screenWidth - titleSize.X) / 2,
                    50),
                Color.White);

            foreach (Button button in _buttons)
            {
                button.Draw(spriteBatch, _font);
            }

            spriteBatch.End();
        }
    }
}