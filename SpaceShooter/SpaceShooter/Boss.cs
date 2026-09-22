namespace SpaceShooter
{
    public class Boss
    {
        public Texture2D Texture;
        public Texture2D ProjectileTexture;
        public Texture2D HealthBarTexture;
        public XnaRectangle Size;
        public Vector2 Position;
        public XnaRectangle Hitbox;
        public float Speed;
        public int MaxHealth;
        public int Health;
        public bool State;
        public List<Projectiles> Projectiles;
        public float ShootingCooldown;
        public float MissileCooldown;
        public int Wave;
        public int HealthMultiplier;
        public float MissileTurnSpeed;
        public int MissileCount;

        private static Random _random = new Random();
        private float _direction = 1f;
        private float _attackCooldownBase;
        private float _missileCooldownBase;

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

        public void Update(GameTime gameTime, Game game, Vector2 playerPosition)
        {
            if (!State)
                return;

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

        private float GetAttackCooldown()
        {
            return Math.Max(0.25f, _attackCooldownBase * (0.85f + (float)_random.NextDouble() * 0.3f));
        }

        private float GetMissileCooldown()
        {
            return Math.Max(0.75f, _missileCooldownBase * (0.9f + (float)_random.NextDouble() * 0.2f));
        }

        private void ShootBasic()
        {
            Vector2 projectilePosition = new Vector2(
                Position.X + Size.Width / 2f - ProjectileTexture.Width / 2f,
                Position.Y - ProjectileTexture.Height
            );

            Projectiles.Add(new Projectiles(
                ProjectileTexture,
                projectilePosition,
                new Vector2(ProjectileTexture.Width, ProjectileTexture.Height),
                300f,
                1
            ));
        }

        private void ShootHomingMissiles(Vector2 playerPosition)
        {
            for (int i = 0; i < MissileCount; i++)
            {
                float spread = (i - (MissileCount - 1) / 2f) * 20f;

                Vector2 missilePosition = new Vector2(
                    Position.X + Size.Width / 2f - ProjectileTexture.Width / 2f + spread,
                    Position.Y - ProjectileTexture.Height
                );

                Projectiles missile;
                Projectiles missile2;

                if (Wave < 200)
                {
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
                        new Vector2(-1, -1)
                    );
                }
                else
                {
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
                    new Vector2(-1, -1)
                );

                    missile2 = new Projectiles(
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
                        new Vector2(1, -1)
                    );

                    Projectiles.Add(missile2);
                }

                Projectiles.Add(missile);
            }
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
            if (!State || Texture == null)
                return;

            spriteBatch.Draw(
                Texture,
                new XnaRectangle((int)Position.X, (int)Position.Y, Size.Width, Size.Height),
                XnaColor.White
            );

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

                spriteBatch.Draw(
                    HealthBarTexture,
                    new XnaRectangle((int)Position.X, barY, barWidth, barHeight),
                    XnaColor.DarkRed
                );

                float healthRatio = MaxHealth > 0 ? Health / (float)MaxHealth : 0f;
                int currentWidth = (int)(barWidth * MathHelper.Clamp(healthRatio, 0f, 1f));

                if (currentWidth > 0)
                {
                    spriteBatch.Draw(
                        HealthBarTexture,
                        new XnaRectangle((int)Position.X, barY, currentWidth, barHeight),
                        XnaColor.LimeGreen
                    );
                }
            }
        }
    }
}