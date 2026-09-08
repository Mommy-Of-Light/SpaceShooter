namespace SpaceShooter
{
    public class Button
    {
        public string Text { get; private set; }

        public XnaRectangle Bounds { get; private set; }

        public bool IsHovered { get; private set; }

        private Texture2D _pixel;

        public Button(string text, XnaRectangle bounds)
        {
            Text = text;
            Bounds = bounds;
        }

        public void Update(MouseState mouse)
        {
            XnaPoint mousePosition = mouse.Position;

            IsHovered = Bounds.Contains(mousePosition);
        }

        public bool IsClicked(MouseState current, MouseState previous)
        {
            return IsHovered &&
                   previous.LeftButton == XnaButtonState.Released &&
                   current.LeftButton == XnaButtonState.Pressed;
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont font)
        {
            if (_pixel == null)
            {
                _pixel = new Texture2D(
                    spriteBatch.GraphicsDevice,
                    1,
                    1);

                _pixel.SetData(new[] { XnaColor.White });
            }

            XnaColor buttonColor = IsHovered
                ? XnaColor.DarkBlue
                : XnaColor.DarkSlateGray;

            spriteBatch.Draw(
                _pixel,
                Bounds,
                buttonColor);

            Vector2 textSize = font.MeasureString(Text);

            Vector2 textPosition = new Vector2(
                Bounds.X + (Bounds.Width - textSize.X) / 2,
                Bounds.Y + (Bounds.Height - textSize.Y) / 2);

            spriteBatch.DrawString(
                font,
                Text,
                textPosition,
                XnaColor.White);
        }
    }
}