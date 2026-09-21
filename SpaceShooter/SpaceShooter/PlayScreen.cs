using static System.Formats.Asn1.AsnWriter;

namespace SpaceShooter
{
    public class PlayScreen : GameScreen
    {
        private Player _player;
        private Texture2D _playerTexture;
        private Texture2D _projectileTexture;
        private List<Enemy> _enemies;
        private Texture2D _enemyTexture;
        private Texture2D _enemyProjectileTexture;
        private WaveManager _waveManager;
        private SpriteFont _font;
        private List<Button> _buttons;
        private Button _continueButton;
        private Button _restartButton;
        private Button _exitButton;
        private KeyboardState _previousKeyboard;
        private bool _isPaused;
        private int _score;
        private int _lastUpgradeWave;
        private int startingUpgradeWave = 1;
        private int nextUpgradeWave = 1;
        private int upgradeWaveIncrement = 2;

        public PlayScreen(Game1 game) : base(game)
        {
            _previousKeyboard = Keyboard.GetState();

            Game.ChangeScreenSize(500, 800);
        }

        public override void Initialize()
        {
            _font = Game.Content.Load<SpriteFont>("Fonts/SpaceInvader_12");
            _playerTexture = Game.Content.Load<Texture2D>("Textures/PNG/playerShip1_blue");
            _projectileTexture = Game.Content.Load<Texture2D>("Textures/PNG/Lazers/laserBlue01");
            _enemyTexture = Game.Content.Load<Texture2D>("Textures/PNG/Enemies/enemyBlack1");
            _enemyProjectileTexture = Game.Content.Load<Texture2D>("Textures/PNG/Lazers/laserRed01");

            _waveManager = new WaveManager(_enemyTexture, _enemyProjectileTexture);

            _enemies = _waveManager.CreateNextWave(Game.GraphicsDevice.Viewport.Width);

            Vector2 playerPosition = new Vector2((Game.GraphicsDevice.Viewport.Width - _playerTexture.Width * 0.5f) / 2f, Game.GraphicsDevice.Viewport.Height - _playerTexture.Height * 0.5f - 30);

            _player = new Player(_playerTexture, _projectileTexture, playerPosition, 300f);
            _player.GameEnemyList = _enemies;

            _buttons = new List<Button>();

            _buttons.Add(new Button("||", new XnaRectangle(Game.GraphicsDevice.Viewport.Width - 60, 10, 50, 50)));

            _continueButton = new Button("Continue", new XnaRectangle(150, 300, 200, 50));
            _restartButton = new Button("Restart", new XnaRectangle(150, 370, 200, 50));
            _exitButton = new Button("Exit", new XnaRectangle(150, 440, 200, 50));

            _lastUpgradeWave = 0;
            _isPaused = false;

            startingUpgradeWave = 1;
            nextUpgradeWave = startingUpgradeWave;
            upgradeWaveIncrement = 2;

            _player.AutoAimMissiles = 1;

            _score = 0;
        }

        public override void Update(GameTime gameTime, KeyboardState keyboard, Vector2 mousePosition, bool mouseClicked)
        {
            if (keyboard.IsKeyDown(XnaKeys.Escape) && _previousKeyboard.IsKeyUp(XnaKeys.Escape))
            {
                _isPaused = !_isPaused;
            }

            if (_isPaused)
            {
                _continueButton.Update(mousePosition);
                _restartButton.Update(mousePosition);
                _exitButton.Update(mousePosition);

                if (_continueButton.IsClicked(mousePosition, mouseClicked))
                {
                    _isPaused = false;
                }
                else if (_restartButton.IsClicked(mousePosition, mouseClicked))
                {
                    ResetGame();
                    _isPaused = false;
                }
                else if (_exitButton.IsClicked(mousePosition, mouseClicked))
                {
                    SaveGame();

                    Game.ScreenManager.ChangeScreen(new NewGameScreen(Game));
                    return;
                }

                _previousKeyboard = keyboard;

                return;
            }

            foreach (Enemy enemy in _enemies)
            {
                enemy.Update(gameTime, Game);

                if (enemy.Position.Y + enemy.Size.Height >= Game.GraphicsDevice.Viewport.Height)
                {
                    Game.ScreenManager.ChangeScreen(new DeathScreen(Game, _waveManager.CurrentWave, _score));
                    return;
                }
            }

            _enemies.RemoveAll(enemy => !enemy.State);

            if (_enemies.Count == 0)
            {
                int completedWave = _waveManager.CurrentWave;

                _player.Projectiles.Clear();

                if (completedWave >= nextUpgradeWave && completedWave != _lastUpgradeWave)
                {
                    _lastUpgradeWave = completedWave;

                    nextUpgradeWave += upgradeWaveIncrement;

                    upgradeWaveIncrement++;

                    Game.ScreenManager.ChangeScreen(new UpgradeScreen(Game, this));

                    return;
                }

                _enemies = _waveManager.CreateNextWave(Game.GraphicsDevice.Viewport.Width);

                _player.GameEnemyList = _enemies;
            }

            _player.Update(gameTime, Game);

            CheckCollisions();

            foreach (Button button in _buttons)
            {
                button.Update(mousePosition);

                if (button.IsClicked(mousePosition, mouseClicked))
                {
                    if (button.Text == "||")
                    {
                        _isPaused = true;
                    }

                    break;
                }
            }

            _previousKeyboard = keyboard;
        }

        private void AddScore()
        {
            double points = 10.0 * 1.0 * (_waveManager.CurrentWave / 10.0) * Game.DifficultyMultiplier;

            _score += (int)Math.Round(points);
        }

        private void CheckCollisions()
        {
            foreach (Projectiles projectile in _player.Projectiles)
            {
                if (!projectile.State)
                    continue;

                foreach (Enemy enemy in _enemies)
                {
                    if (!enemy.State)
                        continue;

                    if (projectile.HitEnemies.Contains(enemy))
                        continue;

                    if (projectile.Hitbox.Intersects(enemy.Hitbox))
                    {
                        bool enemyWasAlive = enemy.State;

                        enemy.TakeDamage(projectile.Damage);

                        if (enemyWasAlive && !enemy.State)
                        {
                            AddScore();
                        }

                        projectile.HitEnemies.Add(enemy);

                        if (projectile.Pierce > 0)
                        {
                            projectile.Pierce--;

                            if (projectile.IsMissile)
                            {
                                projectile.ChangeTarget();
                            }
                        }
                        else
                        {
                            projectile.State = false;
                            break;
                        }
                    }
                }
            }

            foreach (Enemy enemy in _enemies)
            {
                if (!enemy.State)
                    continue;

                foreach (Projectiles projectile in enemy.Projectiles)
                {
                    if (!projectile.State)
                        continue;

                    if (projectile.Hitbox.Intersects(_player.Hitbox))
                    {
                        projectile.State = false;
                        Game.ScreenManager.ChangeScreen(new DeathScreen(Game, _waveManager.CurrentWave, _score));
                        return;
                    }
                }
            }

            foreach (Enemy enemy in _enemies)
            {
                if (!enemy.State)
                    continue;

                if (enemy.Hitbox.Intersects(_player.Hitbox))
                {
                    Game.ScreenManager.ChangeScreen(new DeathScreen(Game, _waveManager.CurrentWave, _score));
                    return;
                }
            }
        }

        private void ResetGame()
        {
            SaveManager.Delete();

            _waveManager.Reset();

            _lastUpgradeWave = 0;

            _score = 0;

            nextUpgradeWave = startingUpgradeWave;

            upgradeWaveIncrement = 2;

            _enemies = _waveManager.CreateNextWave(Game.GraphicsDevice.Viewport.Width);

            ResetPlayer();

            _player.GameEnemyList = _enemies;
        }

        private void ResetPlayer()
        {
            Vector2 playerPosition = new Vector2((Game.GraphicsDevice.Viewport.Width - _playerTexture.Width * 0.5f) / 2f, Game.GraphicsDevice.Viewport.Height - _playerTexture.Height * 0.5f - 30);

            _player = new Player(_playerTexture, _projectileTexture, playerPosition, 300f);

            _player.GameEnemyList = _enemies;
        }

        public void ApplyUpgrade(int upgrade)
        {
            switch (upgrade)
            {
                case 0:
                    _player.ShotsUntilMissile = Math.Max(1, _player.ShotsUntilMissile - 1);
                    break;

                case 1:
                    _player.Pierce++;
                    break;

                case 2:
                    _player.AutoAimMissiles++;
                    break;

                case 3:
                    _player.Damage++;
                    break;
            }

            _enemies = _waveManager.CreateNextWave(Game.GraphicsDevice.Viewport.Width);

            _player.GameEnemyList = _enemies;
        }

        public void SaveGame()
        {
            GameData data = new GameData();

            data.CurrentWave = _waveManager.CurrentWave;

            data.LastUpgradeWave = _lastUpgradeWave;
            data.NextUpgradeWave = nextUpgradeWave;
            data.UpgradeWaveIncrement = upgradeWaveIncrement;
            data.StartingUpgradeWave = startingUpgradeWave;

            data.PlayerX = _player.Position.X;
            data.PlayerY = _player.Position.Y;
            data.PlayerSpeed = _player.Speed;
            data.PlayerWidth = _player.Width;
            data.PlayerHeight = _player.Height;

            data.Pierce = _player.Pierce;
            data.AutoAimMissiles = _player.AutoAimMissiles;
            data.Damage = _player.Damage;
            data.ShotsUntilMissile = _player.ShotsUntilMissile;

            data.ShotCount = _player.ShotCount;
            data.ShootCooldown = _player.CurrentShootCooldown;

            data.Score = _score;
            data.Difficulty = Game.Difficulty;
            data.DifficultyMultiplier = Game.DifficultyMultiplier;

            foreach (Projectiles projectile in _player.Projectiles)
            {
                data.PlayerProjectiles.Add(CreateProjectileData(projectile));
            }

            foreach (Enemy enemy in _enemies)
            {
                EnemyData enemyData = new EnemyData();

                enemyData.X = enemy.Position.X;
                enemyData.Y = enemy.Position.Y;

                enemyData.Width = enemy.Size.Width;
                enemyData.Height = enemy.Size.Height;

                enemyData.Speed = enemy.Speed;

                enemyData.MaxHealth = enemy.MaxHealth;
                enemyData.Health = enemy.Health;

                enemyData.ShootingCooldown = enemy.ShootingCooldown;
                enemyData.State = enemy.State;

                foreach (Projectiles projectile in enemy.Projectiles)
                {
                    enemyData.Projectiles.Add(CreateProjectileData(projectile));
                }

                data.Enemies.Add(enemyData);
            }

            SaveManager.Save(data);
        }

        public bool LoadGame()
        {
            GameData data = SaveManager.Load();

            _score = data.Score;

            if (!string.IsNullOrEmpty(data.Difficulty))
                Game.Difficulty = data.Difficulty;
            else
                Game.Difficulty = "Medium";

            if (data.DifficultyMultiplier > 0)
                Game.DifficultyMultiplier = data.DifficultyMultiplier;
            else
                Game.DifficultyMultiplier = 1.0;

            if (data == null)
                return false;

            _waveManager.SetCurrentWave(data.CurrentWave);

            _lastUpgradeWave = data.LastUpgradeWave;
            nextUpgradeWave = data.NextUpgradeWave;
            upgradeWaveIncrement = data.UpgradeWaveIncrement;
            startingUpgradeWave = data.StartingUpgradeWave;

            _enemies = new List<Enemy>();

            foreach (EnemyData enemyData in data.Enemies)
            {
                XnaRectangle enemySize = new XnaRectangle(
                    0,
                    0,
                    enemyData.Width,
                    enemyData.Height
                );

                Enemy enemy = new Enemy(
                    _enemyTexture,
                    _enemyProjectileTexture,
                    new Vector2(enemyData.X, enemyData.Y),
                    enemySize,
                    enemyData.Speed,
                    enemyData.MaxHealth
                );

                enemy.Health = enemyData.Health;
                enemy.ShootingCooldown = enemyData.ShootingCooldown;
                enemy.State = enemyData.State;

                enemy.Hitbox = new XnaRectangle(
                    (int)enemyData.X,
                    (int)enemyData.Y,
                    enemyData.Width,
                    enemyData.Height
                );

                foreach (ProjectileData projectileData in enemyData.Projectiles)
                {
                    enemy.Projectiles.Add(
                        CreateProjectileFromData(
                            projectileData,
                            _enemyProjectileTexture
                        )
                    );
                }

                _enemies.Add(enemy);
            }

            Vector2 playerPosition = new Vector2(
                data.PlayerX,
                data.PlayerY
            );

            _player = new Player(
                _playerTexture,
                _projectileTexture,
                playerPosition,
                data.PlayerSpeed
            );

            _player.Width = data.PlayerWidth;
            _player.Height = data.PlayerHeight;

            _player.Pierce = data.Pierce;
            _player.AutoAimMissiles = data.AutoAimMissiles;
            _player.Damage = data.Damage;
            _player.ShotsUntilMissile = data.ShotsUntilMissile;

            _player.ShotCount = data.ShotCount;
            _player.CurrentShootCooldown = data.ShootCooldown;

            _player.Hitbox = new XnaRectangle(
                (int)data.PlayerX,
                (int)data.PlayerY,
                data.PlayerWidth,
                data.PlayerHeight
            );

            _player.GameEnemyList = _enemies;

            foreach (ProjectileData projectileData in data.PlayerProjectiles)
            {
                _player.Projectiles.Add(
                    CreateProjectileFromData(
                        projectileData,
                        _projectileTexture
                    )
                );
            }

            RestoreProjectileReferences(
                _player.Projectiles,
                data.PlayerProjectiles
            );

            for (int enemyIndex = 0; enemyIndex < _enemies.Count; enemyIndex++)
            {
                RestoreProjectileReferences(
                    _enemies[enemyIndex].Projectiles,
                    data.Enemies[enemyIndex].Projectiles
                );
            }

            _isPaused = false;
            _previousKeyboard = Keyboard.GetState();

            return true;
        }

        private ProjectileData CreateProjectileData(Projectiles projectile)
        {
            ProjectileData data = new ProjectileData();

            data.X = projectile.Position.X;
            data.Y = projectile.Position.Y;

            data.Width = projectile.Size.X;
            data.Height = projectile.Size.Y;

            data.Speed = projectile.Speed;
            data.State = projectile.State;

            data.Damage = projectile.Damage;
            data.Pierce = projectile.Pierce;

            data.IsMissile = projectile.IsMissile;
            data.Rotation = projectile.Rotation;

            if (projectile.IsMissile && projectile.Target != null)
            {
                data.TargetEnemyIndex = _enemies.IndexOf(projectile.Target);
            }

            foreach (Enemy hitEnemy in projectile.HitEnemies)
            {
                int enemyIndex = _enemies.IndexOf(hitEnemy);

                if (enemyIndex >= 0)
                {
                    data.HitEnemyIndexes.Add(enemyIndex);
                }
            }

            return data;
        }

        private Projectiles CreateProjectileFromData(
            ProjectileData data,
            Texture2D projectileTexture)
        {
            Projectiles projectile =
                new Projectiles(
                    projectileTexture,
                    new Vector2(data.X, data.Y),
                    new Vector2(data.Width, data.Height),
                    data.Speed,
                    data.Damage,
                    data.Pierce,
                    data.IsMissile,
                    null,
                    _enemies
                );

            projectile.State = data.State;
            projectile.Rotation = data.Rotation;

            projectile.Hitbox = new XnaRectangle(
                (int)data.X,
                (int)data.Y,
                (int)data.Width,
                (int)data.Height
            );

            return projectile;
        }

        private void RestoreProjectileReferences(
            List<Projectiles> projectiles,
            List<ProjectileData> savedData)
        {
            for (int i = 0; i < projectiles.Count && i < savedData.Count; i++)
            {
                Projectiles projectile = projectiles[i];
                ProjectileData data = savedData[i];

                projectile.HitEnemies.Clear();

                foreach (int enemyIndex in data.HitEnemyIndexes)
                {
                    if (enemyIndex >= 0 && enemyIndex < _enemies.Count)
                    {
                        projectile.HitEnemies.Add(_enemies[enemyIndex]);
                    }
                }

                if (projectile.IsMissile &&
                    data.TargetEnemyIndex >= 0 &&
                    data.TargetEnemyIndex < _enemies.Count)
                {
                    projectile.Target = _enemies[data.TargetEnemyIndex];
                }
                else
                {
                    projectile.Target = null;
                }
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            Texture2D background = Game.Content.Load<Texture2D>("Textures/Background/black");
            spriteBatch.Draw(background, Vector2.Zero, XnaColor.White);
            _player.Draw(gameTime, spriteBatch);

            foreach (Enemy enemy in _enemies)
            {
                enemy.Draw(gameTime, spriteBatch);
            }

            string scoreText = "Score: " + _score;

            spriteBatch.DrawString(
                _font,
                scoreText,
                new Vector2(10, 10),
                XnaColor.White
            );

            string waveText = "WAVE " + _waveManager.CurrentWave;
            spriteBatch.DrawString(_font, waveText, new Vector2(10, 35), XnaColor.White);

            string damageText = "DMG " + _player.Damage;
            spriteBatch.DrawString(_font, damageText, new Vector2(10, 60), XnaColor.White);

            string missileText = "MISSILES " + _player.AutoAimMissiles;
            spriteBatch.DrawString(_font, missileText, new Vector2(10, 85), XnaColor.White);

            string pierceText = "PIERCE " + _player.Pierce;
            spriteBatch.DrawString(_font, pierceText, new Vector2(10, 110), XnaColor.White);

            string shotsText = "MISSILE EVERY " + _player.ShotsUntilMissile;
            spriteBatch.DrawString(_font, shotsText, new Vector2(10, 135), XnaColor.White);

            string nextUpgradeText = "NEXT UPGRADE " + nextUpgradeWave;
            spriteBatch.DrawString(_font, nextUpgradeText, new Vector2(10, 160), XnaColor.White);

            foreach (Button button in _buttons)
            {
                button.Draw(spriteBatch, _font);
            }

            if (_isPaused)
            {
                _continueButton.Draw(spriteBatch, _font);
                _restartButton.Draw(spriteBatch, _font);
                _exitButton.Draw(spriteBatch, _font);
            }

            spriteBatch.End();
        }
    }
}