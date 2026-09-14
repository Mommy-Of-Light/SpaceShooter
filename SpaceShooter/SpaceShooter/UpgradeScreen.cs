namespace SpaceShooter
{
    public class UpgradeScreen : GameScreen
    {
        private SpriteFont _font;
        private SpriteFont _titleFont;

        private Button _fireRateButton;
        private Button _pierceButton;
        private Button _missileButton;
        private Button _damageButton;

        private KeyboardState _previousKeyboard;

        private PlayScreen _playScreen;

        public UpgradeScreen(
            Game1 game,
            PlayScreen playScreen) : base(game)
        {
            _playScreen = playScreen;

            _previousKeyboard =
                Keyboard.GetState();

            Game.ChangeScreenSize(
                500,
                500);
        }

        public override void Initialize()
        {
            _font =
                Game.Content.Load<SpriteFont>(
                    "Fonts/SpaceInvader_12");

            _titleFont =
                Game.Content.Load<SpriteFont>(
                    "Fonts/SpaceInvader_16");

            _fireRateButton =
                new Button(
                    "RAPID FIRE",
                    new XnaRectangle(
                        100,
                        130,
                        300,
                        50));

            _pierceButton =
                new Button(
                    "PIERCE +1",
                    new XnaRectangle(
                        100,
                        200,
                        300,
                        50));

            _missileButton =
                new Button(
                    "MISSILE +1",
                    new XnaRectangle(
                        100,
                        270,
                        300,
                        50));

            _damageButton =
                new Button(
                    "DAMAGE +1",
                    new XnaRectangle(
                        100,
                        340,
                        300,
                        50));
        }

        public override void Update(
            GameTime gameTime,
            KeyboardState keyboard,
            Vector2 mousePosition,
            bool mouseClicked)
        {
            _fireRateButton.Update(
                mousePosition);

            _pierceButton.Update(
                mousePosition);

            _missileButton.Update(
                mousePosition);

            _damageButton.Update(
                mousePosition);

            if (_fireRateButton.IsClicked(
                mousePosition,
                mouseClicked))
            {
                ChooseUpgrade(0);
                return;
            }

            if (_pierceButton.IsClicked(
                mousePosition,
                mouseClicked))
            {
                ChooseUpgrade(1);
                return;
            }

            if (_missileButton.IsClicked(
                mousePosition,
                mouseClicked))
            {
                ChooseUpgrade(2);
                return;
            }

            if (_damageButton.IsClicked(
                mousePosition,
                mouseClicked))
            {
                ChooseUpgrade(3);
                return;
            }

            if (keyboard.IsKeyDown(
                    XnaKeys.D1) &&
                _previousKeyboard.IsKeyUp(
                    XnaKeys.D1))
            {
                ChooseUpgrade(0);
                return;
            }

            if (keyboard.IsKeyDown(
                    XnaKeys.D2) &&
                _previousKeyboard.IsKeyUp(
                    XnaKeys.D2))
            {
                ChooseUpgrade(1);
                return;
            }

            if (keyboard.IsKeyDown(
                    XnaKeys.D3) &&
                _previousKeyboard.IsKeyUp(
                    XnaKeys.D3))
            {
                ChooseUpgrade(2);
                return;
            }

            if (keyboard.IsKeyDown(
                    XnaKeys.D4) &&
                _previousKeyboard.IsKeyUp(
                    XnaKeys.D4))
            {
                ChooseUpgrade(3);
                return;
            }

            _previousKeyboard =
                keyboard;
        }

        private void ChooseUpgrade(
            int upgrade)
        {
            Game.ChangeScreenSize(
                400,
                800);

            _playScreen.ApplyUpgrade(
                upgrade);

            Game.ScreenManager.ReturnToScreen(
                _playScreen);
        }

        public override void Draw(
            GameTime gameTime,
            SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            Texture2D background =
                Game.Content.Load<Texture2D>(
                    "Textures/Background/black");

            spriteBatch.Draw(
                background,
                Vector2.Zero,
                XnaColor.White);

            string title =
                "CHOOSE UPGRADE";

            Vector2 titleSize =
                _titleFont.MeasureString(
                    title);

            spriteBatch.DrawString(
                _titleFont,
                title,
                new Vector2(
                    (Game.GraphicsDevice.Viewport.Width -
                     titleSize.X) / 2f,
                    50),
                XnaColor.White);

            _fireRateButton.Draw(
                spriteBatch,
                _font);

            _pierceButton.Draw(
                spriteBatch,
                _font);

            _missileButton.Draw(
                spriteBatch,
                _font);

            _damageButton.Draw(
                spriteBatch,
                _font);

            spriteBatch.End();
        }
    }
}