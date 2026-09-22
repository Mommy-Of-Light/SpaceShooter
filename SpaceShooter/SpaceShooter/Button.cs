namespace SpaceShooter
{
    public class Button
    {
        public string Text { get; private set; }
        public XnaRectangle Bounds { get; private set; }
        public bool IsHovered { get; private set; }
        public bool IsSelected { get; private set; }
        public bool IsLocked { get; private set; }
        private Texture2D _pixel;

        public Button(string text, XnaRectangle bounds)
        {
            Text = text;
            Bounds = bounds;
        }

        public void Update(Vector2 mousePosition)
        {
            if (IsLocked)
            {
                IsHovered = false;
                return;
            }

            IsHovered = Bounds.Contains(mousePosition.ToPoint());
        }

        public bool IsClicked(Vector2 mousePosition, bool mouseClicked)
        {
            if (IsLocked)
                return false;

            return IsHovered && mouseClicked && Bounds.Contains(mousePosition.ToPoint());
        }

        public void SetSelected(bool selected)
        {
            IsSelected = selected;
        }

        public void SetLocked(bool locked)
        {
            IsLocked = locked;

            if (locked)
            {
                IsHovered = false;
            }
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont font)
        {
            if (_pixel == null)
            {
                _pixel = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
                _pixel.SetData(new[] { XnaColor.White });
            }

            XnaColor buttonColor;

            if (IsLocked)
            {
                buttonColor = XnaColor.DarkGray;
            }
            else if (IsSelected)
            {
                buttonColor = XnaColor.DarkBlue;
            }
            else if (IsHovered)
            {
                buttonColor = XnaColor.DarkBlue;
            }
            else
            {
                buttonColor = XnaColor.DarkSlateGray;
            }

            spriteBatch.Draw(_pixel, Bounds, buttonColor);

            string displayText = IsLocked ? "MAX" : Text;

            if (displayText == "||")
            {
                int barWidth = 5;
                int barHeight = 22;
                int gap = 6;

                int totalWidth = (barWidth * 2) + gap;

                int x = Bounds.X + (Bounds.Width - totalWidth) / 2;
                int y = Bounds.Y + (Bounds.Height - barHeight) / 2;

                spriteBatch.Draw(
                    _pixel,
                    new XnaRectangle(x, y, barWidth, barHeight),
                    XnaColor.White
                );

                spriteBatch.Draw(
                    _pixel,
                    new XnaRectangle(x + barWidth + gap, y, barWidth, barHeight),
                    XnaColor.White
                );
            }
            else
            {
                Vector2 textSize = font.MeasureString(displayText);

                Vector2 textPosition = new Vector2(
                    Bounds.X + (Bounds.Width - textSize.X) / 2f,
                    Bounds.Y + (Bounds.Height - textSize.Y) / 2f
                );

                spriteBatch.DrawString(
                    font,
                    displayText,
                    textPosition,
                    XnaColor.White
                );
            }
        }
    }
}