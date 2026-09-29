namespace SpaceShooter
{
    /// <summary>
    /// Represents a boss enemy with health, movement, basic projectiles,
    /// and homing missile attacks.
    /// </summary>
    public class Boss
    {
        /// <summary>
        /// Texture used to draw the boss.
        /// </summary>
        public Texture2D Texture;

        /// <summary>
        /// Texture used for the boss's projectiles and missiles.
        /// </summary>
        public Texture2D ProjectileTexture;

        /// <summary>
        /// Texture used to draw the boss's health bar.
        /// </summary>
        public Texture2D HealthBarTexture;

        /// <summary>
        /// Defines the size and dimensions of the boss.
        /// </summary>
        public XnaRectangle Size;

        /// <summary>
        /// Represents the current position of the boss on the screen.
        /// </summary>
        public Vector2 Position;

        /// <summary>
        /// Defines the collision area of the boss.
        /// </summary>
        public XnaRectangle Hitbox;

        /// <summary>
        /// Represents the horizontal movement speed of the boss.
        /// </summary>
        public float Speed;

        /// <summary>
        /// Represents the maximum health of the boss.
        /// </summary>
        public int MaxHealth;

        /// <summary>
        /// Represents the current health of the boss.
        /// </summary>
        public int Health;

        /// <summary>
        /// Indicates whether the boss is currently active.
        /// </summary>
        public bool State;

        /// <summary>
        /// Contains the projectiles currently fired by the boss.
        /// </summary>
        public List<Projectiles> Projectiles;

        /// <summary>
        /// Represents the remaining time before the boss can fire a basic projectile.
        /// </summary>
        public float ShootingCooldown;

        /// <summary>
        /// Represents the remaining time before the boss can fire homing missiles.
        /// </summary>
        public float MissileCooldown;

        /// <summary>
        /// Represents the current game wave.
        /// </summary>
        public int Wave;

        /// <summary>
        /// Represents the health multiplier applied to the boss.
        /// </summary>
        public int HealthMultiplier;

        /// <summary>
        /// Represents the turning speed of the boss's homing missiles.
        /// </summary>
        public float MissileTurnSpeed;

        /// <summary>
        /// Represents the number of homing missiles fired during an attack.
        /// </summary>
        public int MissileCount;

        /// <summary>
        /// Random number generator used to vary attack cooldowns.
        /// </summary>
        private static Random _random = new Random();

        /// <summary>
        /// Represents the current horizontal movement direction of the boss.
        /// </summary>
        private float _direction = 1f;

        /// <summary>
        /// Stores the base cooldown for basic attacks.
        /// </summary>
        private float _attackCooldownBase;

        /// <summary>
        /// Stores the base cooldown for homing missile attacks.
        /// </summary>
        private float _missileCooldownBase;

        /// <summary>
        /// Initializes a new instance of the <see cref="Boss"/> class.
        /// </summary>
        /// <param name="texture">The texture used to draw the boss.</param>
        /// <param name="projectileTexture">The texture used for the boss's projectiles.</param>
        /// <param name="healthBarTexture">The texture used to draw the boss's health bar.</param>
        /// <param name="screenWidth">The width of the game screen.</param>
        /// <param name="wave">The current game wave.</param>
        /// <param name="normalEnemyHealth">The base health value used to calculate the boss's health.</param>
        public Boss(Texture2D texture, Texture2D projectileTexture, Texture2D healthBarTexture, int screenWidth, int wave, int normalEnemyHealth)
        {
            Texture = texture;
            ProjectileTexture = projectileTexture;
            HealthBarTexture = healthBarTexture;
            Wave = wave;

            float scale = 2.5f;
            int width = (int)(texture.Width * 0.6f * scale);
            int height = (int)(texture.Height * 0.6f * scale);

            Size = new XnaRectangle(0, 0, width, height);
            Position = new Vector2((screenWidth - width) / 2f, 70f);
            Hitbox = new XnaRectangle((int)Position.X, (int)Position.Y, width, height);

            int attackTier = wave / 100;

            HealthMultiplier = wave % 50 == 0 ? 100 : 100;
            MaxHealth = Math.Max(1, normalEnemyHealth * HealthMultiplier);
            Health = MaxHealth;
            Speed = 80f + attackTier * 10f;
            _attackCooldownBase = Math.Max(0.35f, 1.5f - attackTier * 0.2f);
            _missileCooldownBase = Math.Max(1f, 4f - attackTier * 0.4f);
            MissileTurnSpeed = 1.5f + attackTier * 0.75f;
            MissileCount = 1 + attackTier / 2;

            Projectiles = new List<Projectiles>();
            ShootingCooldown = GetAttackCooldown();
            MissileCooldown = GetMissileCooldown();
            State = true;
        }

        /// <summary>
        /// Updates the boss's movement, attacks, projectiles, and health state.
        /// </summary>
        /// <param name="gameTime">Provides timing information for the current game update.</param>
        /// <param name="game">The main game instance used to access game and screen information.</param>
        /// <param name="playerPosition">The current position of the player, used as the target for homing missiles.</param>
        public void Update(GameTime gameTime, Game game, Vector2 playerPosition)
        {
            if (!State)
                return;

            if (Wave % 200 == 0 && MissileCount < 6)
            {
                MissileCount++;
            }

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            int screenWidth = game.GraphicsDevice.Viewport.Width;

            Position.X += Speed * _direction * deltaTime;

            if (Position.X <= 0)
            {
                Position.X = 0;
                _direction = 1f;
            }
            else if (Position.X + Size.Width >= screenWidth)
            {
                Position.X = screenWidth - Size.Width;
                _direction = -1f;
            }

            Hitbox.X = (int)Position.X;
            Hitbox.Y = (int)Position.Y;
            Hitbox.Width = Size.Width;
            Hitbox.Height = Size.Height;

            ShootingCooldown -= deltaTime;
            MissileCooldown -= deltaTime;

            if (ShootingCooldown <= 0)
            {
                ShootBasic();
                ShootingCooldown = GetAttackCooldown();
            }

            if (MissileCooldown <= 0)
            {
                ShootHomingMissiles(playerPosition);
                MissileCooldown = GetMissileCooldown();
            }

            foreach (Projectiles projectile in Projectiles)
            {
                if (projectile.HasPositionTarget)
                {
                    projectile.TargetPosition = playerPosition;
                }

                projectile.Update(gameTime, game.GraphicsDevice.Viewport.Width, game.GraphicsDevice.Viewport.Height, 1);
            }

            Projectiles.RemoveAll(projectile => !projectile.State);

            if (Health <= 0)
            {
                Health = 0;
                State = false;
            }
        }

        /// <summary>
        /// Calculates a randomized cooldown duration for the boss's basic attack.
        /// </summary>
        /// <returns>The duration in seconds before the next basic attack.</returns>
        private float GetAttackCooldown()
        {
            return Math.Max(0.25f, _attackCooldownBase * (0.85f + (float)_random.NextDouble() * 0.3f));
        }

        /// <summary>
        /// Calculates a randomized cooldown duration for the boss's homing missile attack.
        /// </summary>
        /// <returns>The duration in seconds before the next missile attack.</returns>
        private float GetMissileCooldown()
        {
            return Math.Max(0.75f, _missileCooldownBase * (0.9f + (float)_random.NextDouble() * 0.2f));
        }

        /// <summary>
        /// Fires a basic projectile from the boss toward the player.
        /// </summary>
        private void ShootBasic()
        {
            Vector2 projectilePosition = new Vector2(Position.X + Size.Width / 2f - ProjectileTexture.Width / 2f, Position.Y - ProjectileTexture.Height);

            Projectiles.Add(new Projectiles(ProjectileTexture, projectilePosition, new Vector2(ProjectileTexture.Width, ProjectileTexture.Height), 300f, 1));
        }

        /// <summary>
        /// Fires a group of homing missiles toward the player's current position.
        /// </summary>
        /// <param name="playerPosition">The current position of the player used as the missile target.</param>
        private void ShootHomingMissiles(Vector2 playerPosition)
        {
            for (int i = 0; i < MissileCount; i++)
            {
                float spread = (i - (MissileCount - 1) / 2f) * 20f;

                Vector2 missilePosition = new Vector2(Position.X + Size.Width / 2f - ProjectileTexture.Width / 2f + spread, Position.Y - ProjectileTexture.Height);

                Projectiles missile;

                missile = new Projectiles(
                    ProjectileTexture,
                    missilePosition,
                    new Vector2(ProjectileTexture.Width, ProjectileTexture.Height),
                    250f + (Wave / 100) * 25f,
                    1,
                    0,
                    true,
                    null,
                    null,
                    null,
                    true,
                    playerPosition,
                    MissileTurnSpeed,
                    true,
                    (i % 2 == 0) ? new Vector2(-1, -1) : new Vector2(1, -1)
                );

                Projectiles.Add(missile);
            }
        }

        /// <summary>
        /// Applies damage to the boss and deactivates it when its health reaches zero.
        /// </summary>
        /// <param name="damage">The amount of damage to inflict on the boss.</param>
        public void TakeDamage(int damage)
        {
            Health -= damage;

            if (Health <= 0)
            {
                Health = 0;
                State = false;
            }
        }

        /// <summary>
        /// Draws the boss, its projectiles, and its health bar.
        /// </summary>
        /// <param name="gameTime">Provides timing information for the current game draw operation.</param>
        /// <param name="spriteBatch">Used to draw the boss and its associated visual elements.</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!State || Texture == null)
                return;

            spriteBatch.Draw(Texture, new XnaRectangle((int)Position.X, (int)Position.Y, Size.Width, Size.Height), XnaColor.White);

            foreach (Projectiles projectile in Projectiles)
            {
                projectile.Draw(gameTime, spriteBatch);
            }

            if (HealthBarTexture != null)
            {
                int barWidth = Size.Width;
                int barHeight = 12;
                int barY = (int)Position.Y - barHeight - 8;

                if (barY < 0)
                    barY = (int)Position.Y + Size.Height + 8;

                spriteBatch.Draw(HealthBarTexture, new XnaRectangle((int)Position.X, barY, barWidth, barHeight), XnaColor.DarkRed);

                float healthRatio = MaxHealth > 0 ? Health / (float)MaxHealth : 0f;
                int currentWidth = (int)(barWidth * MathHelper.Clamp(healthRatio, 0f, 1f));

                if (currentWidth > 0)
                {
                    spriteBatch.Draw(HealthBarTexture, new XnaRectangle((int)Position.X, barY, currentWidth, barHeight), XnaColor.LimeGreen);
                }
            }
        }
    }
}