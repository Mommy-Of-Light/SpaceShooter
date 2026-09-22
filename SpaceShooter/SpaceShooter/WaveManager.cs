namespace SpaceShooter
{
    public class WaveManager
    {
        private Texture2D _enemyTexture;
        private Texture2D _enemyProjectileTexture;
        private double _difficultyMultiplier;
        private int _currentWave = 0;
        public int CurrentWave => _currentWave;

        public WaveManager(Texture2D enemyTexture, Texture2D enemyProjectileTexture, double difficultyMultiplier)
        {
            _enemyTexture = enemyTexture;
            _enemyProjectileTexture = enemyProjectileTexture;
            _difficultyMultiplier = difficultyMultiplier;
            _currentWave = 0;
        }

        public bool IsBossWave(int wave)
        {
            return wave > 0 && wave % 20 == 0;
        }

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
            //int totalEnemies = 1; // just one to test boss waves

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

            //enemySpeed = 200f; // just to test boss waves

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
                float y = -(reversedRow) * (enemyHeight + verticalSpacing);

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

        public int GetNormalEnemyHealth(int wave)
        {
            int baseEnemyHealth = GetBaseEnemyHealth(wave);

            return Math.Max(1, (int)Math.Round(baseEnemyHealth * _difficultyMultiplier));
        }

        private int GetBaseEnemyHealth(int wave)
        {
            return 1 + ((wave - 1) / 3);
        }

        public void Reset()
        {
            _currentWave = 0;
        }

        public void SetCurrentWave(int currentWave)
        {
            _currentWave = currentWave;
        }
    }
}
