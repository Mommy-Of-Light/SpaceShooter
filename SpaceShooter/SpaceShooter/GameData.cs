namespace SpaceShooter
{
    /// <summary>
    /// Represents the complete state of a game session that can be saved or restored.
    /// </summary>
    public class GameData
    {
        /// <summary>
        /// Gets or sets the unique identifier of the current game run.
        /// </summary>
        public string RunId { get; set; }

        /// <summary>
        /// Gets or sets the current game wave.
        /// </summary>
        public int CurrentWave { get; set; }

        /// <summary>
        /// Gets or sets the wave at which the last upgrade was obtained.
        /// </summary>
        public int LastUpgradeWave { get; set; }

        /// <summary>
        /// Gets or sets the wave at which the next upgrade becomes available.
        /// </summary>
        public int NextUpgradeWave { get; set; }

        /// <summary>
        /// Gets or sets the number of waves between upgrades.
        /// </summary>
        public int UpgradeWaveIncrement { get; set; }

        /// <summary>
        /// Gets or sets the wave at which upgrades begin.
        /// </summary>
        public int StartingUpgradeWave { get; set; }

        /// <summary>
        /// Gets or sets the player's horizontal position.
        /// </summary>
        public float PlayerX { get; set; }

        /// <summary>
        /// Gets or sets the player's vertical position.
        /// </summary>
        public float PlayerY { get; set; }

        /// <summary>
        /// Gets or sets the player's movement speed.
        /// </summary>
        public float PlayerSpeed { get; set; }

        /// <summary>
        /// Gets or sets the width of the player's hitbox or sprite.
        /// </summary>
        public int PlayerWidth { get; set; }

        /// <summary>
        /// Gets or sets the height of the player's hitbox or sprite.
        /// </summary>
        public int PlayerHeight { get; set; }

        /// <summary>
        /// Gets or sets the number of enemies a projectile can pierce.
        /// </summary>
        public int Pierce { get; set; }

        /// <summary>
        /// Gets or sets the number of automatically aimed missiles available to the player.
        /// </summary>
        public int AutoAimMissiles { get; set; }

        /// <summary>
        /// Gets or sets the player's projectile damage.
        /// </summary>
        public int Damage { get; set; }

        /// <summary>
        /// Gets or sets the number of shots remaining before a missile is fired.
        /// </summary>
        public int ShotsUntilMissile { get; set; }

        /// <summary>
        /// Gets or sets the number of shots fired by the player.
        /// </summary>
        public int ShotCount { get; set; }

        /// <summary>
        /// Gets or sets the time remaining before the player can shoot again.
        /// </summary>
        public float ShootCooldown { get; set; }

        /// <summary>
        /// Gets or sets the player's current score.
        /// </summary>
        public int Score { get; set; }

        /// <summary>
        /// Gets or sets the difficulty level of the game.
        /// </summary>
        public string Difficulty { get; set; }

        /// <summary>
        /// Gets or sets the multiplier associated with the selected difficulty.
        /// </summary>
        public double DifficultyMultiplier { get; set; }

        /// <summary>
        /// Gets or sets the enemies currently present in the game.
        /// </summary>
        public List<EnemyData> Enemies { get; set; }

        /// <summary>
        /// Gets or sets the current boss data, if a boss is active.
        /// </summary>
        public BossData Boss { get; set; }

        /// <summary>
        /// Gets or sets the projectiles currently fired by the player.
        /// </summary>
        public List<ProjectileData> PlayerProjectiles { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GameData"/> class
        /// with empty collections for enemies and player projectiles.
        /// </summary>
        public GameData()
        {
            Enemies = new List<EnemyData>();
            PlayerProjectiles = new List<ProjectileData>();
        }
    }

    /// <summary>
    /// Represents the saved state of an enemy in a game session.
    /// </summary>
    public class EnemyData
    {
        /// <summary>
        /// Gets or sets the horizontal position of the enemy.
        /// </summary>
        public float X { get; set; }

        /// <summary>
        /// Gets or sets the vertical position of the enemy.
        /// </summary>
        public float Y { get; set; }

        /// <summary>
        /// Gets or sets the width of the enemy.
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Gets or sets the height of the enemy.
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Gets or sets the movement speed of the enemy.
        /// </summary>
        public float Speed { get; set; }

        /// <summary>
        /// Gets or sets the maximum health of the enemy.
        /// </summary>
        public int MaxHealth { get; set; }

        /// <summary>
        /// Gets or sets the current health of the enemy.
        /// </summary>
        public int Health { get; set; }

        /// <summary>
        /// Gets or sets the remaining time before the enemy can shoot again.
        /// </summary>
        public float ShootingCooldown { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the enemy is currently active.
        /// </summary>
        public bool State { get; set; }

        /// <summary>
        /// Gets or sets the projectiles currently fired by the enemy.
        /// </summary>
        public List<ProjectileData> Projectiles { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EnemyData"/> class
        /// with an empty projectile collection.
        /// </summary>
        public EnemyData()
        {
            Projectiles = new List<ProjectileData>();
        }
    }

    /// <summary>
    /// Represents the saved state of a boss in a game session.
    /// </summary>
    public class BossData
    {
        /// <summary>
        /// Gets or sets the horizontal position of the boss.
        /// </summary>
        public float X { get; set; }

        /// <summary>
        /// Gets or sets the vertical position of the boss.
        /// </summary>
        public float Y { get; set; }

        /// <summary>
        /// Gets or sets the width of the boss.
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Gets or sets the height of the boss.
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Gets or sets the movement speed of the boss.
        /// </summary>
        public float Speed { get; set; }

        /// <summary>
        /// Gets or sets the maximum health of the boss.
        /// </summary>
        public int MaxHealth { get; set; }

        /// <summary>
        /// Gets or sets the current health of the boss.
        /// </summary>
        public int Health { get; set; }

        /// <summary>
        /// Gets or sets the remaining time before the boss can fire a basic projectile.
        /// </summary>
        public float ShootingCooldown { get; set; }

        /// <summary>
        /// Gets or sets the remaining time before the boss can fire homing missiles.
        /// </summary>
        public float MissileCooldown { get; set; }

        /// <summary>
        /// Gets or sets the game wave associated with the boss.
        /// </summary>
        public int Wave { get; set; }

        /// <summary>
        /// Gets or sets the health multiplier applied to the boss.
        /// </summary>
        public int HealthMultiplier { get; set; }

        /// <summary>
        /// Gets or sets the turning speed of the boss's homing missiles.
        /// </summary>
        public float MissileTurnSpeed { get; set; }

        /// <summary>
        /// Gets or sets the number of homing missiles fired by the boss.
        /// </summary>
        public int MissileCount { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the boss is currently active.
        /// </summary>
        public bool State { get; set; }

        /// <summary>
        /// Gets or sets the projectiles currently fired by the boss.
        /// </summary>
        public List<ProjectileData> Projectiles { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BossData"/> class
        /// with an empty projectile collection.
        /// </summary>
        public BossData()
        {
            Projectiles = new List<ProjectileData>();
        }
    }

    /// <summary>
    /// Represents the saved state and properties of a projectile.
    /// </summary>
    public class ProjectileData
    {
        /// <summary>
        /// Gets or sets the horizontal position of the projectile.
        /// </summary>
        public float X { get; set; }

        /// <summary>
        /// Gets or sets the vertical position of the projectile.
        /// </summary>
        public float Y { get; set; }

        /// <summary>
        /// Gets or sets the width of the projectile.
        /// </summary>
        public float Width { get; set; }

        /// <summary>
        /// Gets or sets the height of the projectile.
        /// </summary>
        public float Height { get; set; }

        /// <summary>
        /// Gets or sets the movement speed of the projectile.
        /// </summary>
        public float Speed { get; set; }

        /// <summary>
        /// Gets or sets the horizontal velocity of the projectile.
        /// </summary>
        public float VelocityX { get; set; }

        /// <summary>
        /// Gets or sets the vertical velocity of the projectile.
        /// </summary>
        public float VelocityY { get; set; }

        /// <summary>
        /// Gets or sets the amount of damage dealt by the projectile.
        /// </summary>
        public int Damage { get; set; }

        /// <summary>
        /// Gets or sets the number of targets the projectile can pierce.
        /// </summary>
        public int Pierce { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the projectile is currently active.
        /// </summary>
        public bool State { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the projectile is a missile.
        /// </summary>
        public bool IsMissile { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the projectile has a position target.
        /// </summary>
        public bool HasPositionTarget { get; set; }

        /// <summary>
        /// Gets or sets the horizontal coordinate of the projectile's target position.
        /// </summary>
        public float TargetX { get; set; }

        /// <summary>
        /// Gets or sets the vertical coordinate of the projectile's target position.
        /// </summary>
        public float TargetY { get; set; }

        /// <summary>
        /// Gets or sets the turning speed of the projectile when homing toward a target.
        /// </summary>
        public float TurnSpeed { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the projectile targets the boss.
        /// </summary>
        public bool TargetsBoss { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the projectile has hit the boss.
        /// </summary>
        public bool HitBoss { get; set; }

        /// <summary>
        /// Gets or sets the current rotation of the projectile.
        /// </summary>
        public float Rotation { get; set; }

        /// <summary>
        /// Gets or sets the index of the enemy currently targeted by the projectile.
        /// </summary>
        public int TargetEnemyIndex { get; set; }

        /// <summary>
        /// Gets or sets the indexes of enemies already hit by the projectile.
        /// </summary>
        public List<int> HitEnemyIndexes { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectileData"/> class
        /// with no target enemy and an empty list of hit enemy indexes.
        /// </summary>
        public ProjectileData()
        {
            TargetEnemyIndex = -1;
            HitEnemyIndexes = new List<int>();
        }
    }
}