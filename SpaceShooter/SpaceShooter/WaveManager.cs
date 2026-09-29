namespace SpaceShooter
{
    /// <summary>
    /// Manages enemy waves, including wave progression, enemy health, speed, and formation.
    /// </summary>
    public class WaveManager
    {
        /// <summary>
        /// Texture used for enemy sprites.
        /// </summary>
        private Texture2D _enemyTexture;

        /// <summary>
        /// Texture used for enemy projectile sprites.
        /// </summary>
        private Texture2D _enemyProjectileTexture;

        /// <summary>
        /// Multiplier applied to enemy difficulty values.
        /// </summary>
        private double _difficultyMultiplier;

        /// <summary>
        /// Stores the number of the current wave.
        /// </summary>
        private int _currentWave = 0;

        /// <summary>
        /// Gets the number of the current wave.
        /// </summary>
        public int CurrentWave => _currentWave;

        /// <summary>
        /// Initializes a new instance of the <see cref="WaveManager"/> class.
        /// </summary>
        /// <param name="enemyTexture">Texture used for enemy sprites.</param>
        /// <param name="enemyProjectileTexture">Texture used for enemy projectile sprites.</param>
        /// <param name="difficultyMultiplier">Multiplier used to scale enemy difficulty.</param>
        public WaveManager(Texture2D enemyTexture, Texture2D enemyProjectileTexture, double difficultyMultiplier)
        {
            _enemyTexture = enemyTexture;
            _enemyProjectileTexture = enemyProjectileTexture;
            _difficultyMultiplier = difficultyMultiplier;
            _currentWave = 0;
        }

        /// <summary>
        /// Determines whether the specified wave is a boss wave.
        /// </summary>
        /// <param name="wave">Wave number to check.</param>
        /// <returns><c>true</c> if the wave is a boss wave; otherwise, <c>false</c>.</returns>
        public bool IsBossWave(int wave)
        {
            return wave > 0 && wave % 20 == 0;
        }

        /// <summary>
        /// Creates the enemies for the next wave and advances the current wave number.
        /// </summary>
        /// <param name="screenWidth">Width of the game screen used to center the enemy formation.</param>
        /// <returns>A list containing the enemies created for the new wave.</returns>
        public List<Enemy> CreateNextWave(int screenWidth)
        {
            _currentWave++;

            List<Enemy> enemies = new List<Enemy>();

            if (IsBossWave(_currentWave))
                return enemies;

            int additionalEnemies;

            switch (_currentWave % 5)
            {
                case 1:
                    additionalEnemies = 1;
                    break;

                case 2:
                    additionalEnemies = 2;
                    break;

                case 3:
                    additionalEnemies = 3;
                    break;

                case 4:
                    additionalEnemies = 5;
                    break;

                default:
                    additionalEnemies = 0;
                    break;
            }

            int totalEnemies = (7 * (int)Math.Floor((float)_currentWave / 5.0f)) + additionalEnemies;

            totalEnemies = Math.Min(totalEnemies, 50);

            int enemiesPerRow = 7;

            int enemyWidth = (int)(_enemyTexture.Width * 0.6f);
            int enemyHeight = (int)(_enemyTexture.Height * 0.6f);

            int horizontalSpacing = 10;
            int verticalSpacing = 20;

            XnaRectangle enemySize = new XnaRectangle(0, 0, enemyWidth, enemyHeight);

            int baseEnemyHealth = GetBaseEnemyHealth(_currentWave);
            int enemyHealth = Math.Max(1, (int)Math.Round(baseEnemyHealth * _difficultyMultiplier));

            float enemySpeed = 25f + ((_currentWave - 1) / 4) * 5f;
            enemySpeed = Math.Min(enemySpeed, 100f);

            int totalRows = (int)Math.Ceiling(totalEnemies / (float)enemiesPerRow);

            for (int i = 0; i < totalEnemies; i++)
            {
                int row = i / enemiesPerRow;
                int column = i % enemiesPerRow;

                int enemiesInThisRow = Math.Min(enemiesPerRow, totalEnemies - row * enemiesPerRow);

                int rowWidth = enemiesInThisRow * enemyWidth + (enemiesInThisRow - 1) * horizontalSpacing;

                float startX = (screenWidth - rowWidth) / 2f;
                float x = startX + column * (enemyWidth + horizontalSpacing);

                int reversedRow = (totalRows - 1) - row;
                float y = -reversedRow * (enemyHeight + verticalSpacing);

                enemies.Add(new Enemy(
                    _enemyTexture,
                    _enemyProjectileTexture,
                    new Vector2(x, y),
                    enemySize,
                    enemySpeed,
                    enemyHealth
                ));
            }

            return enemies;
        }

        /// <summary>
        /// Calculates the health of a normal enemy for the specified wave using the current difficulty multiplier.
        /// </summary>
        /// <param name="wave">Wave number used to calculate enemy health.</param>
        /// <returns>The calculated health value for a normal enemy.</returns>
        public int GetNormalEnemyHealth(int wave)
        {
            int baseEnemyHealth = GetBaseEnemyHealth(wave);

            return Math.Max(1, (int)Math.Round(baseEnemyHealth * _difficultyMultiplier));
        }

        /// <summary>
        /// Calculates the base health of an enemy based on the wave number.
        /// </summary>
        /// <param name="wave">Wave number used to calculate the base health.</param>
        /// <returns>The base health value for an enemy on the specified wave.</returns>
        private int GetBaseEnemyHealth(int wave)
        {
            return 1 + ((wave - 1) / 3);
        }

        /// <summary>
        /// Resets the current wave number back to zero.
        /// </summary>
        public void Reset()
        {
            _currentWave = 0;
        }

        /// <summary>
        /// Sets the current wave number to the specified value.
        /// </summary>
        /// <param name="currentWave">Wave number to set as the current wave.</param>
        public void SetCurrentWave(int currentWave)
        {
            _currentWave = currentWave;
        }
    }
}