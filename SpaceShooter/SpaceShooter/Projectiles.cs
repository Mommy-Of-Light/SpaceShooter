namespace SpaceShooter
{
    public class Projectiles
    {
        public Texture2D Texture;
        public XnaRectangle Hitbox;

        public Vector2 Position;
        
        public Vector2 Size;

        public float Speed;

        public bool State;

        public Projectiles(Texture2D texture, Vector2 position, Vector2 size, float speed)
        {
            Texture = texture;
            Position = position;
            Size = size;
            Speed = speed;
            Hitbox = new XnaRectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
            State = true;
        }

        public void Update(GameTime gameTime, int screenWidth, int screenHeight, int direction)
        {
            Hitbox.X = (int)Position.X;
            Hitbox.Y = (int)Position.Y;
            Position += new Vector2(0, Speed * direction * (float)gameTime.ElapsedGameTime.TotalSeconds);

            if (Position.Y < 0 || Position.Y > screenHeight)
            {
                State = false;
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (Texture != null)
            {
                spriteBatch.Draw(Texture, new XnaRectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y), XnaColor.White);
            }
        }
    }
}
