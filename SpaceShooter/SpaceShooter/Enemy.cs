namespace SpaceShooter
{
    /// <summary>
    /// Represents an enemy in the game, including its movement,
    /// health, projectiles, and shooting behavior.
    /// </summary>
    public class Enemy
    {
        /// <summary>
        /// Texture used to draw the enemy.
        /// </summary>
        public Texture2D Texture;

        /// <summary>
        /// Texture used for the enemy's projectiles.
        /// </summary>
        public Texture2D ProjectileTexture;

        /// <summary>
        /// Defines the size and dimensions of the enemy.
        /// </summary>
        public XnaRectangle Size;

        /// <summary>
        /// Represents the current position of the enemy.
        /// </summary>
        public Vector2 Position;

        /// <summary>
        /// Defines the collision area of the enemy.
        /// </summary>
        public XnaRectangle Hitbox;

        /// <summary>
        /// Represents the downward movement speed of the enemy.
        /// </summary>
        public float Speed;

        /// <summary>
        /// Represents the remaining time before the enemy can shoot again.
        /// </summary>
        public float ShootingCooldown;

        /// <summary>
        /// Contains the projectiles currently fired by the enemy.
        /// </summary>
        public List<Projectiles> Projectiles;

        /// <summary>
        /// Indicates whether the enemy is currently active.
        /// </summary>
        public bool State;

        /// <summary>
        /// Represents the maximum health of the enemy.
        /// </summary>
        public int MaxHealth;

        /// <summary>
        /// Represents the current health of the enemy.
        /// </summary>
        public int Health;

        /// <summary>
        /// Random number generator used to determine shooting cooldowns.
        /// </summary>
        private static Random _random = new Random();

        /// <summary>
        /// Initializes a new instance of the <see cref="Enemy"/> class.
        /// </summary>
        /// <param name="texture">The texture used to draw the enemy.</param>
        /// <param name="projectileTexture">The texture used for the enemy's projectiles.</param>
        /// <param name="position">The initial position of the enemy.</param>
        /// <param name="size">The size and dimensions of the enemy.</param>
        /// <param name="speed">The downward movement speed of the enemy.</param>
        /// <param name="health">The initial and maximum health of the enemy.</param>
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

        /// <summary>
        /// Updates the enemy's position, shooting behavior, health state,
        /// collision area, and active projectiles.
        /// </summary>
        /// <param name="gameTime">Provides timing information for the current game update.</param>
        /// <param name="game">The main game instance used to access screen dimensions.</param>
        public void Update(GameTime gameTime, Game game)
        {
            Hitbox.X = (int)Position.X;
            Hitbox.Y = (int)Position.Y;
            Hitbox.Width = Size.Width;
            Hitbox.Height = Size.Height;

            if (State)
            {
                Position.Y += Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                ShootingCooldown -= (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (ShootingCooldown <= 0)
                {
                    Shoot();
                    ShootingCooldown = GetRandomCooldown();
                }

                if (Position.Y > game.GraphicsDevice.Viewport.Height)
                {
                    State = false;
                }

                if (Health <= 0)
                {
                    State = false;
                }
            }

            foreach (Projectiles projectile in Projectiles)
            {
                projectile.Update(gameTime, game.GraphicsDevice.Viewport.Width, game.GraphicsDevice.Viewport.Height, 1);
            }

            Projectiles.RemoveAll(projectile => !projectile.State);
        }

        /// <summary>
        /// Generates a random cooldown duration before the enemy's next shot.
        /// </summary>
        /// <returns>A random shooting cooldown between 1.5 and 5 seconds.</returns>
        private float GetRandomCooldown()
        {
            return 1.5f + (float)_random.NextDouble() * 3.5f;
        }

        /// <summary>
        /// Creates and fires a projectile from the enemy's current position.
        /// </summary>
        private void Shoot()
        {
            Vector2 projectilePosition = new Vector2(
                Position.X + Size.Width / 2f - ProjectileTexture.Width / 2f,
                Position.Y + Size.Height
            );

            Projectiles.Add(new Projectiles(
                ProjectileTexture,
                projectilePosition,
                new Vector2(ProjectileTexture.Width, ProjectileTexture.Height),
                300f
            ));
        }

        /// <summary>
        /// Applies damage to the enemy and deactivates it when its health reaches zero.
        /// </summary>
        /// <param name="damage">The amount of damage to inflict on the enemy.</param>
        public void TakeDamage(int damage)
        {
            Health -= damage;

            if (Health <= 0)
            {
                Health = 0;
                State = false;
                SoundEffectPlayer.Instance.PlaySpaceZap();
            }
        }

        /// <summary>
        /// Draws the enemy and its active projectiles.
        /// </summary>
        /// <param name="gameTime">Provides timing information for the current game draw operation.</param>
        /// <param name="spriteBatch">Used to draw the enemy and its projectiles.</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (State && Texture != null)
            {
                spriteBatch.Draw(Texture, new XnaRectangle((int)Position.X, (int)Position.Y, Size.Width, Size.Height), XnaColor.White);
            }

            foreach (Projectiles projectile in Projectiles)
            {
                projectile.Draw(gameTime, spriteBatch);
            }
        }
    }
}