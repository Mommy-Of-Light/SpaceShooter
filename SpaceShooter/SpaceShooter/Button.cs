namespace SpaceShooter
{
    /// <summary>
    /// Represents a clickable button that can be hovered, selected, or locked.
    /// </summary>
    public class Button
    {
        /// <summary>
        /// Gets the text displayed on the button.
        /// </summary>
        public string Text { get; private set; }

        /// <summary>
        /// Gets the rectangular area occupied by the button.
        /// </summary>
        public XnaRectangle Bounds { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the mouse is currently hovering over the button.
        /// </summary>
        public bool IsHovered { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the button is currently selected.
        /// </summary>
        public bool IsSelected { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the button is locked and cannot be interacted with.
        /// </summary>
        public bool IsLocked { get; private set; }

        /// <summary>
        /// Texture used to draw the button background and custom elements.
        /// </summary>
        private Texture2D _pixel;

        /// <summary>
        /// Initializes a new instance of the <see cref="Button"/> class.
        /// </summary>
        /// <param name="text">The text displayed on the button.</param>
        /// <param name="bounds">The rectangular area occupied by the button.</param>
        public Button(string text, XnaRectangle bounds)
        {
            Text = text;
            Bounds = bounds;
        }

        /// <summary>
        /// Updates the hover state of the button based on the current mouse position.
        /// </summary>
        /// <param name="mousePosition">The current position of the mouse cursor.</param>
        public void Update(Vector2 mousePosition)
        {
            if (IsLocked)
            {
                IsHovered = false;
                return;
            }

            IsHovered = Bounds.Contains(mousePosition.ToPoint());
        }

        /// <summary>
        /// Determines whether the button has been clicked at the specified mouse position.
        /// </summary>
        /// <param name="mousePosition">The current position of the mouse cursor.</param>
        /// <param name="mouseClicked">Indicates whether the mouse button was clicked.</param>
        /// <returns><see langword="true"/> if the button was clicked; otherwise, <see langword="false"/>.</returns>
        public bool IsClicked(Vector2 mousePosition, bool mouseClicked)
        {
            if (IsLocked)
                return false;

            return IsHovered && mouseClicked && Bounds.Contains(mousePosition.ToPoint());
        }

        /// <summary>
        /// Sets the selected state of the button.
        /// </summary>
        /// <param name="selected">Indicates whether the button should be selected.</param>
        public void SetSelected(bool selected)
        {
            IsSelected = selected;
        }

        /// <summary>
        /// Sets the locked state of the button.
        /// </summary>
        /// <param name="locked">Indicates whether the button should be locked.</param>
        public void SetLocked(bool locked)
        {
            IsLocked = locked;

            if (locked)
            {
                IsHovered = false;
            }
        }

        /// <summary>
        /// Changes the text displayed on the button.
        /// </summary>
        /// <param name="text">The new button text.</param>
        public void SetText(string text)
        {
            Text = text;
        }

        /// <summary>
        /// Draws the button using its current state and appearance.
        /// </summary>
        /// <param name="spriteBatch">Used to draw the button and its contents.</param>
        /// <param name="font">Font used to draw the button text.</param>
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

                spriteBatch.Draw(_pixel, new XnaRectangle(x, y, barWidth, barHeight), XnaColor.White);
                spriteBatch.Draw(_pixel, new XnaRectangle(x + barWidth + gap, y, barWidth, barHeight), XnaColor.White);
            }
            else
            {
                Vector2 textSize = font.MeasureString(displayText);
                Vector2 textPosition = new Vector2(Bounds.X + (Bounds.Width - textSize.X) / 2f, Bounds.Y + (Bounds.Height - textSize.Y) / 2f);

                spriteBatch.DrawString(font, displayText, textPosition, XnaColor.White);
            }
        }
    }
}