namespace SpaceShooter
{
    public class Enemy
    {
        public Texture2D Texture;
        public Texture2D ProjectileTexture;
        public XnaRectangle Size;
        public Vector2 Position;
        public XnaRectangle Hitbox;
        public float Speed;
        public float ShootingCooldown;
        public List<Projectiles> Projectiles;
        public bool State;
        public int MaxHealth;
        public int Health;
        private static Random _random = new Random();

        public Enemy(Texture2D texture, Texture2D projectileTexture, Vector2 position, XnaRectangle size, float speed, int health)
        {
            Texture = texture;
            ProjectileTexture = projectileTexture;

            Position = position;
            Speed = speed;
            Size = size;

            MaxHealth = health;
            Health = health;

            Hitbox = new XnaRectangle((int)position.X, (int)position.Y, Size.Width, Size.Height);

            Projectiles = new List<Projectiles>();

            ShootingCooldown = GetRandomCooldown();

            State = true;
        }

        public void Update(GameTime gameTime, Game game)
        {
            Hitbox.X = (int)Position.X;
            Hitbox.Y = (int)Position.Y;
            Hitbox.Width = Size.Width;
            Hitbox.Height = Size.Height;

            Position.Y += Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            ShootingCooldown -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (ShootingCooldown <= 0)
            {
                Shoot();
                ShootingCooldown = GetRandomCooldown();
            }

            foreach (Projectiles projectile in Projectiles)
            {
                projectile.Update(gameTime, game.GraphicsDevice.Viewport.Width, game.GraphicsDevice.Viewport.Height, 1);
            }

            Projectiles.RemoveAll(projectile => !projectile.State);

            if (Position.Y > game.GraphicsDevice.Viewport.Height)
            {
                State = false;
            }

            if (Health <= 0)
            {
                State = false;
            }
        }

        private float GetRandomCooldown()
        {
            return 1.5f + (float)_random.NextDouble() * 3.5f;
        }

        private void Shoot()
        {
            Vector2 projectilePosition = new Vector2(Position.X + Size.Width / 2f - ProjectileTexture.Width / 2f, Position.Y + Size.Height);

            Projectiles.Add(new Projectiles(ProjectileTexture, projectilePosition, new Vector2(ProjectileTexture.Width, ProjectileTexture.Height), 300f));
        }

        public void TakeDamage(int damage)
        {
            Health -= damage;

            if (Health <= 0)
            {
                Health = 0;
                State = false;
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (Texture != null)
            {
                spriteBatch.Draw(Texture, new XnaRectangle((int)Position.X, (int)Position.Y, Size.Width, Size.Height), XnaColor.White);

                foreach (Projectiles projectile in Projectiles)
                {
                    projectile.Draw(gameTime, spriteBatch);
                }
            }
        }
    }
}