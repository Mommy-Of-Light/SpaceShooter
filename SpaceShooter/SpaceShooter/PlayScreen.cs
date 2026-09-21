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
                    Game.ScreenManager.ChangeScreen(new DeathScreen(Game, _waveManager.CurrentWave));
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
                        enemy.TakeDamage(projectile.Damage);

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
                        Game.ScreenManager.ChangeScreen(new DeathScreen(Game, _waveManager.CurrentWave));
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
                    Game.ScreenManager.ChangeScreen(new DeathScreen(Game, _waveManager.CurrentWave));
                    return;
                }
            }
        }

        private void ResetGame()
        {
            _waveManager.Reset();

            _lastUpgradeWave = 0;

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

            string waveText = "WAVE " + _waveManager.CurrentWave;
            spriteBatch.DrawString(_font, waveText, new Vector2(10, 10), XnaColor.White);

            string damageText = "DMG " + _player.Damage;
            spriteBatch.DrawString(_font, damageText, new Vector2(10, 35), XnaColor.White);

            string missileText = "MISSILES " + _player.AutoAimMissiles;
            spriteBatch.DrawString(_font, missileText, new Vector2(10, 60), XnaColor.White);

            string pierceText = "PIERCE " + _player.Pierce;
            spriteBatch.DrawString(_font, pierceText, new Vector2(10, 85), XnaColor.White);

            string shotsText = "MISSILE EVERY " + _player.ShotsUntilMissile;
            spriteBatch.DrawString(_font, shotsText, new Vector2(10, 110), XnaColor.White);

            string nextUpgradeText = "NEXT UPGRADE " + nextUpgradeWave;
            spriteBatch.DrawString(_font, nextUpgradeText, new Vector2(10, 135), XnaColor.White);

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