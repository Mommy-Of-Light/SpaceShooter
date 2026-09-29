namespace SpaceShooter
{
    /// <summary>
    /// Represents the player-controlled spaceship.
    /// Handles player movement, shooting, missiles, projectiles, and collision bounds.
    /// </summary>
    public class Player
    {
        /// <summary>
        /// Texture used to draw the player's spaceship.
        /// </summary>
        public Texture2D Texture;

        /// <summary>
        /// Texture used for the player's projectiles.
        /// </summary>
        public Texture2D ProjectileTexture;

        /// <summary>
        /// Current position of the player.
        /// </summary>
        public Vector2 Position;

        /// <summary>
        /// Collision rectangle of the player.
        /// </summary>
        public XnaRectangle Hitbox;

        /// <summary>
        /// Movement speed of the player.
        /// </summary>
        public float Speed;

        /// <summary>
        /// Collection of projectiles currently fired by the player.
        /// </summary>
        public List<Projectiles> Projectiles;

        /// <summary>
        /// Width of the player's spaceship.
        /// </summary>
        public int Width;

        /// <summary>
        /// Height of the player's spaceship.
        /// </summary>
        public int Height;

        /// <summary>
        /// Number of enemies a projectile can pierce.
        /// </summary>
        public int Pierce = 0;

        /// <summary>
        /// Number of auto-aim missiles fired when the missile threshold is reached.
        /// </summary>
        public int AutoAimMissiles = 1;

        /// <summary>
        /// Damage dealt by the player's projectiles.
        /// </summary>
        public int Damage = 1;

        /// <summary>
        /// Number of normal shots fired before missiles are launched.
        /// </summary>
        public int ShotsUntilMissile = 5;

        /// <summary>
        /// Gets or sets the current boss targeted by the player's missiles.
        /// </summary>
        public Boss BossTarget { get; set; }

        /// <summary>
        /// Stores the number of shots fired since the last missile launch.
        /// </summary>
        private int _shotCount = 0;

        /// <summary>
        /// Stores the remaining time before the player can shoot again.
        /// </summary>
        private float _shootCooldown = 0f;

        /// <summary>
        /// Delay between consecutive player shots, in seconds.
        /// </summary>
        public float ShootCooldown = 0.2f;

        /// <summary>
        /// Stores the keyboard state from the previous update.
        /// </summary>
        private KeyboardState _previousKeyboard;

        /// <summary>
        /// Gets or sets the list of enemies currently present in the game.
        /// </summary>
        public List<Enemy> GameEnemyList { get; set; }

        /// <summary>
        /// Indicates whether automatic shooting debug mode is enabled.
        /// </summary>
        private bool debugAutoShoot = false;

        /// <summary>
        /// Gets or sets the number of shots fired since the last missile launch.
        /// </summary>
        public int ShotCount
        {
            get { return _shotCount; }
            set { _shotCount = value; }
        }

        /// <summary>
        /// Gets or sets the current remaining shooting cooldown.
        /// </summary>
        public float CurrentShootCooldown
        {
            get { return _shootCooldown; }
            set { _shootCooldown = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Player"/> class.
        /// </summary>
        /// <param name="texture">Texture used to draw the player.</param>
        /// <param name="projectileTexture">Texture used for player projectiles.</param>
        /// <param name="position">Initial position of the player.</param>
        /// <param name="speed">Movement speed of the player.</param>
        public Player(Texture2D texture, Texture2D projectileTexture, Vector2 position, float speed)
        {
            Texture = texture;
            ProjectileTexture = projectileTexture;
            Position = position;
            Speed = speed;

            float scale = 0.4f;

            Width = (int)(texture.Width * scale);
            Height = (int)(texture.Height * scale);

            Hitbox = new XnaRectangle((int)position.X, (int)position.Y, Width, Height);
            Projectiles = new List<Projectiles>();
            _shotCount = 0;
            _previousKeyboard = Keyboard.GetState();
        }

        /// <summary>
        /// Updates the player's movement, shooting, cooldowns, and projectiles.
        /// </summary>
        /// <param name="gameTime">Provides timing information for the update.</param>
        /// <param name="game">The current game instance used to access the viewport.</param>
        public void Update(GameTime gameTime, Game game)
        {
            Hitbox.X = (int)Position.X;
            Hitbox.Y = (int)Position.Y;
            Hitbox.Width = Width;
            Hitbox.Height = Height;

            Vector2 movement = Vector2.Zero;
            KeyboardState keyboard = Keyboard.GetState();

            if (keyboard.IsKeyDown(XnaKeys.F12))
            {
                debugAutoShoot = !debugAutoShoot;
            }

            if (keyboard.IsKeyDown(XnaKeys.A) || keyboard.IsKeyDown(XnaKeys.Left))
            {
                movement.X -= 1;
            }

            if (keyboard.IsKeyDown(XnaKeys.D) || keyboard.IsKeyDown(XnaKeys.Right))
            {
                movement.X += 1;
            }

            if (keyboard.IsKeyDown(XnaKeys.LeftShift) || keyboard.IsKeyDown(XnaKeys.RightShift))
            {
                movement *= 0.5f;
            }

            if (Position.X < 0 && movement.X < 0)
            {
                movement.X = 0;
            }

            if (Position.X > game.GraphicsDevice.Viewport.Width - Width && movement.X > 0)
            {
                movement.X = 0;
            }

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            _shootCooldown -= deltaTime;

            if (debugAutoShoot && _shootCooldown <= 0f)
            {
                ShootNormal();
                _shotCount++;

                if (_shotCount >= ShotsUntilMissile)
                {
                    if (AutoAimMissiles > 0)
                    {
                        ShootMissiles();
                    }

                    _shotCount = 0;
                }

                _shootCooldown = ShootCooldown;
            }

            if (keyboard.IsKeyDown(XnaKeys.Space) && _shootCooldown <= 0f)
            {
                ShootNormal();
                _shotCount++;

                if (_shotCount >= ShotsUntilMissile)
                {
                    if (AutoAimMissiles > 0)
                    {
                        ShootMissiles();
                    }

                    _shotCount = 0;
                }

                _shootCooldown = ShootCooldown;
            }

            _previousKeyboard = keyboard;

            foreach (Projectiles projectile in Projectiles)
            {
                projectile.Update(gameTime, game.GraphicsDevice.Viewport.Width, game.GraphicsDevice.Viewport.Height, -1);
            }

            Projectiles.RemoveAll(projectile => !projectile.State);

            Position += movement * Speed * deltaTime;
            Position.X = MathHelper.Clamp(Position.X, 0, game.GraphicsDevice.Viewport.Width - Width);
        }

        /// <summary>
        /// Creates and fires a normal projectile from the player's position.
        /// </summary>
        private void ShootNormal()
        {
            Vector2 projectilePosition = new Vector2(Position.X + Width / 2f - ProjectileTexture.Width / 2f, Position.Y);

            Projectiles.Add(new Projectiles(ProjectileTexture, projectilePosition, new Vector2(ProjectileTexture.Width, ProjectileTexture.Height), 500f, Damage, Pierce));

            SoundEffectPlayer.Instance.PlaySnareShot();
        }

        /// <summary>
        /// Creates and fires auto-aim missiles toward the current boss or available enemies.
        /// </summary>
        private void ShootMissiles()
        {
            SoundEffectPlayer.Instance.PlayRetroLazer();

            if (BossTarget != null && BossTarget.State)
            {
                for (int i = 0; i < AutoAimMissiles; i++)
                {
                    Vector2 missilePosition = new Vector2(
                        Position.X + Width / 2f - ProjectileTexture.Width / 2f,
                        Position.Y - ProjectileTexture.Height
                    );

                    Projectiles.Add(new Projectiles(
                        ProjectileTexture,
                        missilePosition,
                        new Vector2(ProjectileTexture.Width, ProjectileTexture.Height),
                        350f,
                        Damage,
                        Pierce,
                        true,
                        null,
                        null,
                        BossTarget,
                        false,
                        default,
                        5f,
                        false,
                        new Vector2(0, -1)
                    ));
                }

                return;
            }

            if (GameEnemyList == null || GameEnemyList.Count == 0)
            {
                return;
            }

            List<Enemy> availableEnemies = GameEnemyList
                .Where(enemy => enemy.State)
                .OrderBy(enemy => Vector2.Distance(Position, enemy.Position))
                .ToList();

            if (availableEnemies.Count == 0)
                return;

            for (int i = 0; i < AutoAimMissiles; i++)
            {
                Enemy target;

                if (i < availableEnemies.Count)
                {
                    target = availableEnemies[i];
                }
                else
                {
                    target = availableEnemies[0];
                }

                Vector2 missilePosition = new Vector2(Position.X + Width / 2f - ProjectileTexture.Width / 2f, Position.Y);

                Projectiles.Add(new Projectiles(
                    ProjectileTexture,
                    missilePosition,
                    new Vector2(ProjectileTexture.Width, ProjectileTexture.Height),
                    350f,
                    Damage,
                    Pierce,
                    true,
                    target,
                    GameEnemyList,
                    null,
                    false,
                    default,
                    5f,
                    false,
                    new Vector2(0, -1)
                ));
            }
        }

        /// <summary>
        /// Draws the player, its collision hitbox, and all active projectiles.
        /// </summary>
        /// <param name="gameTime">Provides timing information for the draw operation.</param>
        /// <param name="spriteBatch">The sprite batch used to draw the player and projectiles.</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (Texture == null)
                return;

            spriteBatch.Draw(Texture, new XnaRectangle((int)Position.X, (int)Position.Y, Width, Height), XnaColor.White);

            foreach (Projectiles projectile in Projectiles)
            {
                projectile.Draw(gameTime, spriteBatch);
            }
        }
    }
}