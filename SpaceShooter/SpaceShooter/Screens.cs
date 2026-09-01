using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace SpaceShooter
{
    public class NewGameScreen : GameScreen
    {
        private SpriteFont _font;
        private SpriteFont _font_title;

        private List<Button> _buttons;

        private MouseState _previousMouse;

        public NewGameScreen(Game1 game) : base(game)
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
                "Create a new game",
                new Rectangle(x, startY, buttonWidth, buttonHeight)));

            _buttons.Add(new Button(
                "Return",
                new Rectangle(x, startY + spacing * 1, buttonWidth, buttonHeight)));
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
                }
            }

            _previousMouse = mouse;
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

            spriteBatch.Draw(Game.Content.Load<Texture2D>("Textures/Background/black"), Vector2.Zero, Color.White);

            int screenWidth = Game.GraphicsDevice.Viewport.Width;

            string title = "NEW GAME";

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

    public class SaveScreen : GameScreen
    {
        private SpriteFont _font;
        private SpriteFont _font_title;

        private List<Button> _buttons;

        private MouseState _previousMouse;

        public SaveScreen(Game1 game) : base(game)
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
                "Return",
                new Rectangle(x, startY, buttonWidth, buttonHeight)));
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
                }
            }

            _previousMouse = mouse;
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

            spriteBatch.Draw(Game.Content.Load<Texture2D>("Textures/Background/black"), Vector2.Zero, Color.White);

            int screenWidth = Game.GraphicsDevice.Viewport.Width;

            string title = "SAVED GAMES";

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

    public class ArchiveScreen : GameScreen
    {
        private SpriteFont _font;
        private SpriteFont _font_title;

        private List<Button> _buttons;

        private MouseState _previousMouse;

        public ArchiveScreen(Game1 game) : base(game)
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
                "Return",
                new Rectangle(x, startY, buttonWidth, buttonHeight)));
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
                }
            }

            _previousMouse = mouse;
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

            spriteBatch.Draw(Game.Content.Load<Texture2D>("Textures/Background/black"), Vector2.Zero, Color.White);

            int screenWidth = Game.GraphicsDevice.Viewport.Width;

            string title = "ARCHIVED GAMES";

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

    public class RankingScreen : GameScreen
    {
        private SpriteFont _font;
        private SpriteFont _font_title;

        private List<Button> _buttons;

        private MouseState _previousMouse;

        public RankingScreen(Game1 game) : base(game)
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
                "Return",
                new Rectangle(x, startY, buttonWidth, buttonHeight)));
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
                }
            }

            _previousMouse = mouse;
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

            spriteBatch.Draw(Game.Content.Load<Texture2D>("Textures/Background/black"), Vector2.Zero, Color.White);

            int screenWidth = Game.GraphicsDevice.Viewport.Width;

            string title = "RANKING";

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

    public class PlayScreen : GameScreen
    {
        private SpriteFont _font;
        private SpriteFont _font_title;

        private List<Button> _buttons;

        private MouseState _previousMouse;

        public PlayScreen(Game1 game) : base(game)
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
                "Return",
                new Rectangle(x, startY, buttonWidth, buttonHeight)));
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
                }
            }

            _previousMouse = mouse;
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

            spriteBatch.Draw(Game.Content.Load<Texture2D>("Textures/Background/black"), Vector2.Zero, Color.White);

            int screenWidth = Game.GraphicsDevice.Viewport.Width;

            string title = "GAME IN PROGRESS";

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