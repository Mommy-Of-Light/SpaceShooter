namespace SpaceShooter
{
    public class WaveManager
    {
        private Texture2D _enemyTexture;
        private Texture2D _enemyProjectileTexture;

        private int _currentWave = 0;

        public int CurrentWave => _currentWave;

        public WaveManager(
            Texture2D enemyTexture,
            Texture2D enemyProjectileTexture)
        {
            _enemyTexture = enemyTexture;
            _enemyProjectileTexture = enemyProjectileTexture;
            _currentWave = 0;
        }

        public List<Enemy> CreateNextWave(int screenWidth)
        {
            _currentWave++;

            List<Enemy> enemies =
                new List<Enemy>();

            int additionalEnemies;

            switch (_currentWave)
            {
                case 1:
                    additionalEnemies = 0;
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
                    additionalEnemies =
                        7 +
                        (_currentWave - 5) * 2;
                    break;
            }

            int totalEnemies =
                5 + additionalEnemies;

            totalEnemies =
                Math.Min(totalEnemies, 20);

            int enemiesPerRow = 5;

            int enemyWidth =
                (int)(_enemyTexture.Width * 0.8f);

            int enemyHeight =
                (int)(_enemyTexture.Height * 0.8f);

            int horizontalSpacing = 10;
            int verticalSpacing = 20;

            XnaRectangle enemySize =
                new XnaRectangle(
                    0,
                    0,
                    enemyWidth,
                    enemyHeight);

            int enemyHealth =
                1 +
                ((_currentWave - 1) / 3);

            for (int i = 0;
                 i < totalEnemies;
                 i++)
            {
                int row =
                    i / enemiesPerRow;

                int column =
                    i % enemiesPerRow;

                int enemiesInThisRow =
                    Math.Min(
                        enemiesPerRow,
                        totalEnemies -
                        row * enemiesPerRow);

                int rowWidth =
                    enemiesInThisRow *
                    enemyWidth +
                    (enemiesInThisRow - 1) *
                    horizontalSpacing;

                float startX =
                    (screenWidth - rowWidth) / 2f;

                float x =
                    startX +
                    column *
                    (enemyWidth +
                     horizontalSpacing);

                float y =
                    30 +
                    row *
                    (enemyHeight +
                     verticalSpacing);

                enemies.Add(
                    new Enemy(
                        _enemyTexture,
                        _enemyProjectileTexture,
                        new Vector2(x, y),
                        enemySize,
                        50f,
                        enemyHealth));
            }

            return enemies;
        }

        public void Reset()
        {
            _currentWave = 0;
        }
    }
}