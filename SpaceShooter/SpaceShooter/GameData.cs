namespace SpaceShooter
{
    public class GameData
    {
        public int CurrentWave { get; set; }
        public int LastUpgradeWave { get; set; }
        public int NextUpgradeWave { get; set; }
        public int UpgradeWaveIncrement { get; set; }
        public int StartingUpgradeWave { get; set; }
        public float PlayerX { get; set; }
        public float PlayerY { get; set; }
        public float PlayerSpeed { get; set; }
        public int PlayerWidth { get; set; }
        public int PlayerHeight { get; set; }
        public int Pierce { get; set; }
        public int AutoAimMissiles { get; set; }
        public int Damage { get; set; }
        public int ShotsUntilMissile { get; set; }
        public int ShotCount { get; set; }
        public float ShootCooldown { get; set; }
        public int Score { get; set; }
        public string Difficulty { get; set; }
        public double DifficultyMultiplier { get; set; }
        public List<EnemyData> Enemies { get; set; }
        public List<ProjectileData> PlayerProjectiles { get; set; }

        public GameData()
        {
            Enemies = new List<EnemyData>();
            PlayerProjectiles = new List<ProjectileData>();
        }
    }

    public class EnemyData
    {
        public float X { get; set; }
        public float Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public float Speed { get; set; }
        public int MaxHealth { get; set; }
        public int Health { get; set; }
        public float ShootingCooldown { get; set; }
        public bool State { get; set; }
        public List<ProjectileData> Projectiles { get; set; }

        public EnemyData()
        {
            Projectiles = new List<ProjectileData>();
        }
    }

    public class ProjectileData
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public float Speed { get; set; }
        public float VelocityX { get; set; }
        public float VelocityY { get; set; }
        public int Damage { get; set; }
        public int Pierce { get; set; }
        public bool State { get; set; }
        public bool IsMissile { get; set; }
        public float Rotation { get; set; }
        public int TargetEnemyIndex { get; set; }
        public List<int> HitEnemyIndexes { get; set; }

        public ProjectileData()
        {
            TargetEnemyIndex = -1;
            HitEnemyIndexes = new List<int>();
        }
    }
}