namespace SpaceShooter
{
    public class PlayScreen : GameScreen
    {
        private Player _player;
        private Texture2D _playerTexture;
        private Texture2D _projectileTexture;
        private List<Enemy> _enemies;
        private Boss _boss;
        private Texture2D _enemyTexture;
        private Texture2D _enemyProjectileTexture;
        private Texture2D _pixelTexture;
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
        private SaveData _currentSave;
        private string _runId;

        public Player Player
        {
            get { return _player; }
        }

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

            _pixelTexture = new Texture2D(Game.GraphicsDevice, 1, 1);
            _pixelTexture.SetData(new[] { XnaColor.White });

            _waveManager = new WaveManager(_enemyTexture, _enemyProjectileTexture, Game.DifficultyMultiplier);

            _enemies = _waveManager.CreateNextWave(Game.GraphicsDevice.Viewport.Width);
            _boss = null;

            Vector2 playerPosition = new Vector2(
                (Game.GraphicsDevice.Viewport.Width - _playerTexture.Width * 0.5f) / 2f,
                Game.GraphicsDevice.Viewport.Height - _playerTexture.Height * 0.5f - 30
            );

            _player = new Player(_playerTexture, _projectileTexture, playerPosition, 300f);
            UpdatePlayerTargets();

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

            _runId = null;
            _currentSave = null;
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
                    HandleDeath();
                    return;
                }
            }

            _enemies.RemoveAll(enemy => !enemy.State && enemy.Projectiles.Count == 0);

            if (!_enemies.Any(enemy => enemy.State))
            {
                _enemies.Clear();
            }

            if (_boss != null)
            {
                _boss.Update(
                    gameTime,
                    Game,
                    _player.Position + new Vector2(_player.Width / 2f, _player.Height / 2f)
                );

                if (!_boss.State)
                {
                    _boss = null;
                    _player.Projectiles.Clear();
                    UpdatePlayerTargets();

                    Game.ScreenManager.ChangeScreen(new UpgradeScreen(Game, this));

                    return;
                }
            }

            if (_enemies.Count == 0 && _boss == null)
            {
                int completedWave = _waveManager.CurrentWave;

                foreach (Enemy enemy in _enemies)
                {
                    enemy.Projectiles.Clear();
                }

                _player.Projectiles.Clear();

                if (completedWave >= nextUpgradeWave && completedWave != _lastUpgradeWave)
                {
                    _lastUpgradeWave = completedWave;

                    nextUpgradeWave += upgradeWaveIncrement;

                    upgradeWaveIncrement++;

                    Game.ScreenManager.ChangeScreen(new UpgradeScreen(Game, this));

                    return;
                }

                StartNextWave();
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

        private void StartNextWave()
        {
            _enemies = _waveManager.CreateNextWave(Game.GraphicsDevice.Viewport.Width);

            if (_waveManager.IsBossWave(_waveManager.CurrentWave))
            {
                int normalEnemyHealth = _waveManager.GetNormalEnemyHealth(_waveManager.CurrentWave);

                _boss = new Boss(
                    _enemyTexture,
                    _enemyProjectileTexture,
                    _pixelTexture,
                    Game.GraphicsDevice.Viewport.Width,
                    _waveManager.CurrentWave,
                    normalEnemyHealth
                );
            }

            UpdatePlayerTargets();
        }

        private void UpdatePlayerTargets()
        {
            _player.GameEnemyList = _enemies;
            _player.BossTarget = _boss;
        }

        private void AddScore()
        {
            double points = 10.0 * 1.0 * (_waveManager.CurrentWave / 10.0) * Game.DifficultyMultiplier;

            _score += (int)Math.Round(points);
        }

        private void AddBossScore()
        {
            double points = 100.0 * (_waveManager.CurrentWave / 10.0) * Game.DifficultyMultiplier;

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

                if (!projectile.State || _boss == null || !_boss.State || projectile.HitBoss)
                    continue;

                if (projectile.Hitbox.Intersects(_boss.Hitbox))
                {
                    bool bossWasAlive = _boss.State;

                    _boss.TakeDamage(projectile.Damage);
                    projectile.HitBoss = true;

                    if (bossWasAlive && !_boss.State)
                    {
                        AddBossScore();
                    }

                    if (projectile.Pierce > 0)
                    {
                        projectile.Pierce--;
                    }
                    else
                    {
                        projectile.State = false;
                    }
                }
            }

            foreach (Enemy enemy in _enemies)
            {
                foreach (Projectiles projectile in enemy.Projectiles)
                {
                    if (!projectile.State)
                        continue;

                    if (projectile.Hitbox.Intersects(_player.Hitbox))
                    {
                        projectile.State = false;

                        HandleDeath();

                        return;
                    }
                }
            }

            if (_boss != null && _boss.State)
            {
                foreach (Projectiles projectile in _boss.Projectiles)
                {
                    if (!projectile.State)
                        continue;

                    if (projectile.Hitbox.Intersects(_player.Hitbox))
                    {
                        projectile.State = false;

                        HandleDeath();

                        return;
                    }
                }

                if (_boss.Hitbox.Intersects(_player.Hitbox))
                {
                    HandleDeath();
                }
            }

            foreach (Enemy enemy in _enemies)
            {
                if (!enemy.State)
                    continue;

                if (enemy.Hitbox.Intersects(_player.Hitbox))
                {
                    HandleDeath();

                    return;
                }
            }
        }

        private void HandleDeath()
        {
            ArchiveCurrentGame();

            Game.ScreenManager.ChangeScreen(new DeathScreen(Game, _waveManager.CurrentWave, _score));
        }

        private void ArchiveCurrentGame()
        {
            ArchiveManager.SaveGameArchive(Game.PlayerPseudo, _score, _waveManager.CurrentWave, Game.Difficulty, Game.DifficultyMultiplier);

            if (!string.IsNullOrEmpty(_runId))
            {
                SaveManager.DeleteRun(Game.PlayerPseudo, _runId);
            }

            _currentSave = null;
            _runId = null;
        }

        private void ResetGame()
        {
            _waveManager.Reset();

            _lastUpgradeWave = 0;

            _score = 0;

            nextUpgradeWave = startingUpgradeWave;

            upgradeWaveIncrement = 2;

            _boss = null;
            _enemies = _waveManager.CreateNextWave(Game.GraphicsDevice.Viewport.Width);

            ResetPlayer();

            UpdatePlayerTargets();

            _runId = Guid.NewGuid().ToString();
            _currentSave = null;
        }

        private void ResetPlayer()
        {
            Vector2 playerPosition = new Vector2(
                (Game.GraphicsDevice.Viewport.Width - _playerTexture.Width * 0.5f) / 2f,
                Game.GraphicsDevice.Viewport.Height - _playerTexture.Height * 0.5f - 30
            );

            _player = new Player(_playerTexture, _projectileTexture, playerPosition, 300f);

            UpdatePlayerTargets();
        }

        public void ApplyUpgrade(int upgrade)
        {
            switch (upgrade)
            {
                case 0:
                    if (_player.ShotsUntilMissile > 1)
                    {
                        _player.ShotsUntilMissile--;
                    }
                    break;

                case 1:
                    if (_player.Pierce < 10)
                    {
                        _player.Pierce++;
                    }
                    break;

                case 2:
                    if (_player.AutoAimMissiles < 15)
                    {
                        _player.AutoAimMissiles++;
                    }
                    break;

                case 3:
                    _player.Damage++;
                    break;
            }

            StartNextWave();
        }

        public void SaveGame()
        {
            GameData data = new GameData();

            data.RunId = _runId;

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

            if (_boss != null)
            {
                BossData bossData = new BossData();

                bossData.X = _boss.Position.X;
                bossData.Y = _boss.Position.Y;
                bossData.Width = _boss.Size.Width;
                bossData.Height = _boss.Size.Height;
                bossData.Speed = _boss.Speed;
                bossData.MaxHealth = _boss.MaxHealth;
                bossData.Health = _boss.Health;
                bossData.ShootingCooldown = _boss.ShootingCooldown;
                bossData.MissileCooldown = _boss.MissileCooldown;
                bossData.Wave = _boss.Wave;
                bossData.HealthMultiplier = _boss.HealthMultiplier;
                bossData.MissileTurnSpeed = _boss.MissileTurnSpeed;
                bossData.MissileCount = _boss.MissileCount;
                bossData.State = _boss.State;

                foreach (Projectiles projectile in _boss.Projectiles)
                {
                    bossData.Projectiles.Add(CreateProjectileData(projectile));
                }

                data.Boss = bossData;
            }

            _currentSave = SaveManager.Save(data, Game.PlayerPseudo, _runId);
        }

        public bool LoadGame(SaveData save)
        {
            if (save == null)
                return false;

            if (save.Game == null)
                return false;

            GameData data = save.Game;

            _runId = save.RunId;

            if (string.IsNullOrEmpty(_runId))
            {
                _runId = Guid.NewGuid().ToString();
            }

            _currentSave = save;

            _score = data.Score;

            if (!string.IsNullOrEmpty(data.Difficulty))
                Game.Difficulty = data.Difficulty;
            else
                Game.Difficulty = "Medium";

            if (data.DifficultyMultiplier > 0)
                Game.DifficultyMultiplier = data.DifficultyMultiplier;
            else
                Game.DifficultyMultiplier = 1.0;

            _waveManager.SetCurrentWave(data.CurrentWave);

            _lastUpgradeWave = data.LastUpgradeWave;
            nextUpgradeWave = data.NextUpgradeWave;
            upgradeWaveIncrement = data.UpgradeWaveIncrement;
            startingUpgradeWave = data.StartingUpgradeWave;

            _enemies = new List<Enemy>();
            _boss = null;

            foreach (EnemyData enemyData in data.Enemies)
            {
                XnaRectangle enemySize = new XnaRectangle(0, 0, enemyData.Width, enemyData.Height);

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
                enemy.Hitbox = new XnaRectangle((int)enemyData.X, (int)enemyData.Y, enemyData.Width, enemyData.Height);

                foreach (ProjectileData projectileData in enemyData.Projectiles)
                {
                    enemy.Projectiles.Add(CreateProjectileFromData(projectileData, _enemyProjectileTexture));
                }

                _enemies.Add(enemy);
            }

            if (data.Boss != null && data.Boss.State)
            {
                int normalEnemyHealth = _waveManager.GetNormalEnemyHealth(data.Boss.Wave);

                _boss = new Boss(
                    _enemyTexture,
                    _enemyProjectileTexture,
                    _pixelTexture,
                    Game.GraphicsDevice.Viewport.Width,
                    data.Boss.Wave,
                    normalEnemyHealth
                );

                _boss.Position = new Vector2(data.Boss.X, data.Boss.Y);
                _boss.Size = new XnaRectangle(0, 0, data.Boss.Width, data.Boss.Height);
                _boss.Speed = data.Boss.Speed;
                _boss.MaxHealth = data.Boss.MaxHealth;
                _boss.Health = data.Boss.Health;
                _boss.ShootingCooldown = data.Boss.ShootingCooldown;
                _boss.MissileCooldown = data.Boss.MissileCooldown;
                _boss.HealthMultiplier = data.Boss.HealthMultiplier;
                _boss.MissileTurnSpeed = data.Boss.MissileTurnSpeed;
                _boss.MissileCount = data.Boss.MissileCount;
                _boss.State = data.Boss.State;
                _boss.Hitbox = new XnaRectangle((int)data.Boss.X, (int)data.Boss.Y, data.Boss.Width, data.Boss.Height);

                foreach (ProjectileData projectileData in data.Boss.Projectiles)
                {
                    _boss.Projectiles.Add(CreateProjectileFromData(projectileData, _enemyProjectileTexture));
                }
            }

            Vector2 playerPosition = new Vector2(data.PlayerX, data.PlayerY);

            _player = new Player(_playerTexture, _projectileTexture, playerPosition, data.PlayerSpeed);

            _player.Width = data.PlayerWidth;
            _player.Height = data.PlayerHeight;
            _player.Pierce = data.Pierce;
            _player.AutoAimMissiles = data.AutoAimMissiles;
            _player.Damage = data.Damage;
            _player.ShotsUntilMissile = data.ShotsUntilMissile;
            _player.ShotCount = data.ShotCount;
            _player.CurrentShootCooldown = data.ShootCooldown;
            _player.Hitbox = new XnaRectangle((int)data.PlayerX, (int)data.PlayerY, data.PlayerWidth, data.PlayerHeight);

            UpdatePlayerTargets();

            foreach (ProjectileData projectileData in data.PlayerProjectiles)
            {
                _player.Projectiles.Add(CreateProjectileFromData(projectileData, _projectileTexture));
            }

            RestoreProjectileReferences(_player.Projectiles, data.PlayerProjectiles);

            for (int enemyIndex = 0; enemyIndex < _enemies.Count && enemyIndex < data.Enemies.Count; enemyIndex++)
            {
                RestoreProjectileReferences(_enemies[enemyIndex].Projectiles, data.Enemies[enemyIndex].Projectiles);
            }

            if (_boss != null && data.Boss != null)
            {
                RestoreProjectileReferences(_boss.Projectiles, data.Boss.Projectiles);
            }

            _isPaused = false;
            _previousKeyboard = Keyboard.GetState();

            return true;
        }

        public bool LoadGame()
        {
            SaveData save = SaveManager.GetLatestSave(Game.PlayerPseudo);

            if (save == null)
                return false;

            return LoadGame(save);
        }

        private ProjectileData CreateProjectileData(Projectiles projectile)
        {
            ProjectileData data = new ProjectileData();

            data.X = projectile.Position.X;
            data.Y = projectile.Position.Y;
            data.Width = projectile.Size.X;
            data.Height = projectile.Size.Y;
            data.Speed = projectile.Speed;
            data.VelocityX = projectile.Velocity.X;
            data.VelocityY = projectile.Velocity.Y;
            data.State = projectile.State;
            data.Damage = projectile.Damage;
            data.Pierce = projectile.Pierce;
            data.IsMissile = projectile.IsMissile;
            data.HasPositionTarget = projectile.HasPositionTarget;
            data.TargetX = projectile.TargetPosition.X;
            data.TargetY = projectile.TargetPosition.Y;
            data.TurnSpeed = projectile.TurnSpeed;
            data.TargetsBoss = projectile.BossTarget != null;
            data.HitBoss = projectile.HitBoss;
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

        private Projectiles CreateProjectileFromData(ProjectileData data, Texture2D projectileTexture)
        {
            Projectiles projectile = new Projectiles(
                projectileTexture,
                new Vector2(data.X, data.Y),
                new Vector2(data.Width, data.Height),
                data.Speed,
                data.Damage,
                data.Pierce,
                data.IsMissile,
                null,
                _enemies,
                null,
                data.HasPositionTarget,
                new Vector2(data.TargetX, data.TargetY),
                data.TurnSpeed > 0 ? data.TurnSpeed : 5f
            );

            projectile.State = data.State;
            projectile.HitBoss = data.HitBoss;
            projectile.Rotation = data.Rotation;
            projectile.SetVelocity(new Vector2(data.VelocityX, data.VelocityY));

            if (data.TargetsBoss)
            {
                projectile.BossTarget = _boss;
            }

            projectile.Hitbox = new XnaRectangle((int)data.X, (int)data.Y, (int)data.Width, (int)data.Height);

            return projectile;
        }

        private void RestoreProjectileReferences(List<Projectiles> projectiles, List<ProjectileData> savedData)
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

                if (data.TargetsBoss)
                {
                    projectile.BossTarget = _boss;
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

            if (_boss != null)
            {
                _boss.Draw(gameTime, spriteBatch);
            }

            string scoreText = "Score: " + _score;
            spriteBatch.DrawString(_font, scoreText, new Vector2(10, 10), XnaColor.White);

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
