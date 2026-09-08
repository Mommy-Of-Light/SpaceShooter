namespace SpaceShooter
{
    public class Player
    {
        public Texture2D Texture;

        public Vector2 Position;

        public XnaRectangle Hitbox;

        public float Speed;

        public float ShootingCooldown;

        public Texture2D ProjectileTexture;

        public List<Projectiles> Projectiles;

        public Player(Texture2D texture, Texture2D projectileTexture, Vector2 position, float speed)
        {
            Texture = texture;
            ProjectileTexture = projectileTexture;
            Position = position;
            Speed = speed;
            Hitbox = new XnaRectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
            Projectiles = new List<Projectiles>();
        }

        public void Update(GameTime gameTime, Game game)
        {
            Hitbox.X = (int)Position.X;
            Hitbox.Y = (int)Position.Y;

            Vector2 movement = Vector2.Zero;

            if (Keyboard.GetState().IsKeyDown(XnaKeys.A))
            {
                movement.X -= 1;
            }
            if (Keyboard.GetState().IsKeyDown(XnaKeys.D))
            {
                movement.X += 1;
            }

            if (Position.X < 0 && movement.X < 0)
            {
                movement.X = 0;
            }
            if (Position.X > game.GraphicsDevice.Viewport.Width - Texture.Width && movement.X > 0)
            {
                movement.X = 0;
            }

            ShootingCooldown -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (Keyboard.GetState().IsKeyDown(XnaKeys.Space))
            {
                if (ShootingCooldown <= 0)
                {
                    Projectiles.Add(new Projectiles(ProjectileTexture, new Vector2(Position.X + Texture.Width / 2 - ProjectileTexture.Width / 2, Position.Y), new Vector2(ProjectileTexture.Width, ProjectileTexture.Height), 500f));
                    ShootingCooldown = 0.5f;
                }
            }

            foreach (Projectiles projectile in Projectiles)
            {
                projectile.Update(gameTime, game.GraphicsDevice.Viewport.Width, game.GraphicsDevice.Viewport.Height, -1);

                if (!projectile.State)
                {
                    Projectiles.Remove(projectile);
                    break;
                }
            }

            Position += movement * Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (Texture != null)
            {
                spriteBatch.Draw(Texture, Position, XnaColor.White);

                foreach (Projectiles projectile in Projectiles)
                {
                    projectile.Draw(gameTime, spriteBatch);
                }
            }
        }
    }
}
