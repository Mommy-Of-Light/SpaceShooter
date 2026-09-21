namespace SpaceShooter
{
    public class Player
    {
        public Texture2D Texture;
        public Texture2D ProjectileTexture;
        public Vector2 Position;
        public XnaRectangle Hitbox;
        public float Speed;
        public List<Projectiles> Projectiles;
        public int Width;
        public int Height;
        public int Pierce = 0;
        public int AutoAimMissiles = 1;
        public int Damage = 1;
        public int ShotsUntilMissile = 5;
        private int _shotCount = 0;
        private float _shootCooldown = 0f;
        public float ShootCooldown = 0.2f;
        private KeyboardState _previousKeyboard;
        public List<Enemy> GameEnemyList { get; set; }

        public Player(Texture2D texture, Texture2D projectileTexture, Vector2 position, float speed)
        {
            Texture = texture;
            ProjectileTexture = projectileTexture;

            Position = position;
            Speed = speed;

            float scale = 0.5f;

            Width = (int)(texture.Width * scale);
            Height = (int)(texture.Height * scale);

            Hitbox = new XnaRectangle((int)position.X, (int)position.Y, Width, Height);

            Projectiles = new List<Projectiles>();

            _shotCount = 0;

            _previousKeyboard = Keyboard.GetState();
        }

        public void Update(GameTime gameTime, Game game)
        {
            Hitbox.X = (int)Position.X;
            Hitbox.Y = (int)Position.Y;
            Hitbox.Width = Width;
            Hitbox.Height = Height;

            Vector2 movement = Vector2.Zero;
            KeyboardState keyboard = Keyboard.GetState();

            if (keyboard.IsKeyDown(XnaKeys.A))
            {
                movement.X -= 1;
            }

            if (keyboard.IsKeyDown(XnaKeys.D))
            {
                movement.X += 1;
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

        private void ShootNormal()
        {
            Vector2 projectilePosition = new Vector2(Position.X + Width / 2f - ProjectileTexture.Width / 2f, Position.Y);

            Projectiles.Add(new Projectiles(ProjectileTexture, projectilePosition, new Vector2(ProjectileTexture.Width, ProjectileTexture.Height), 500f, Damage, Pierce));
        }

        private void ShootMissiles()
        {
            if (GameEnemyList == null || GameEnemyList.Count == 0)
            {
                return;
            }

            List<Enemy> availableEnemies = GameEnemyList.Where(enemy => enemy.State).OrderBy(enemy => Vector2.Distance(Position, enemy.Position)).ToList();

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

                Projectiles.Add(new Projectiles(ProjectileTexture, missilePosition, new Vector2(ProjectileTexture.Width, ProjectileTexture.Height), 350f, Damage, Pierce, true, target, GameEnemyList));
            }
        }

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