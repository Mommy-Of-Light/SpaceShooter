namespace SpaceShooter
{
    /// <summary>
    /// Represents a projectile fired by the player, an enemy, or a boss.
    /// Handles projectile movement, missile targeting, collision state, and drawing.
    /// </summary>
    public class Projectiles
    {
        /// <summary>
        /// Texture used to draw the projectile.
        /// </summary>
        public Texture2D Texture;

        /// <summary>
        /// Defines the rectangular collision area of the projectile.
        /// </summary>
        public XnaRectangle Hitbox;

        /// <summary>
        /// Current position of the projectile.
        /// </summary>
        public Vector2 Position;

        /// <summary>
        /// Width and height of the projectile.
        /// </summary>
        public Vector2 Size;

        /// <summary>
        /// Movement speed of the projectile.
        /// </summary>
        public float Speed;

        /// <summary>
        /// Indicates whether the projectile is currently active.
        /// </summary>
        public bool State;

        /// <summary>
        /// Amount of damage dealt when the projectile hits a target.
        /// </summary>
        public int Damage;

        /// <summary>
        /// Number of additional targets the projectile can pierce after a hit.
        /// </summary>
        public int Pierce;

        /// <summary>
        /// Enemy currently targeted by a homing missile.
        /// </summary>
        public Enemy Target;

        /// <summary>
        /// Boss currently targeted by a homing missile.
        /// </summary>
        public Boss BossTarget;

        /// <summary>
        /// Indicates whether the projectile behaves as a homing missile.
        /// </summary>
        public bool IsMissile;

        /// <summary>
        /// Indicates whether the missile moves toward a fixed position instead of an entity.
        /// </summary>
        public bool HasPositionTarget;

        /// <summary>
        /// Position that the missile is targeting when using a fixed position target.
        /// </summary>
        public Vector2 TargetPosition;

        /// <summary>
        /// List of enemies that can be targeted by the missile.
        /// </summary>
        public List<Enemy> EnemyList;

        /// <summary>
        /// List of enemies that have already been hit by this projectile.
        /// </summary>
        public List<Enemy> HitEnemies;

        /// <summary>
        /// Remaining cooldown before the projectile can hit the boss again.
        /// </summary>
        private float _bossHitCooldown;

        /// <summary>
        /// Duration of the cooldown applied after hitting a boss.
        /// </summary>
        private const float BossHitCooldownDuration = 1f;

        /// <summary>
        /// Gets or sets whether the projectile is currently allowed to hit the boss.
        /// Setting this value to false starts the boss hit cooldown.
        /// </summary>
        public bool CanHitBoss
        {
            get { return _bossHitCooldown <= 0f; }
            set { _bossHitCooldown = value ? 0f : BossHitCooldownDuration; }
        }

        /// <summary>
        /// Current rotation of the projectile when it is drawn as a missile.
        /// </summary>
        public float Rotation;

        /// <summary>
        /// Maximum turning speed used by homing missiles.
        /// </summary>
        public float TurnSpeed = 1f;

        /// <summary>
        /// Gets the current normalized movement direction of the projectile.
        /// </summary>
        public Vector2 Velocity => _velocity;

        /// <summary>
        /// Current movement direction and velocity vector of the projectile.
        /// </summary>
        private Vector2 _velocity;

        /// <summary>
        /// Indicates whether the missile's movement angle should be limited when targeting a boss.
        /// </summary>
        private bool _limitBossMissileAngle;

        /// <summary>
        /// Initializes a new instance of the <see cref="Projectiles"/> class.
        /// </summary>
        /// <param name="texture">Texture used to draw the projectile.</param>
        /// <param name="position">Initial position of the projectile.</param>
        /// <param name="size">Width and height of the projectile.</param>
        /// <param name="speed">Movement speed of the projectile.</param>
        /// <param name="damage">Damage dealt by the projectile.</param>
        /// <param name="pierce">Number of additional targets the projectile can pierce.</param>
        /// <param name="isMissile">Indicates whether the projectile is a homing missile.</param>
        /// <param name="target">Initial enemy target of the missile.</param>
        /// <param name="enemyList">List of enemies available as missile targets.</param>
        /// <param name="bossTarget">Boss target of the missile.</param>
        /// <param name="hasPositionTarget">Indicates whether the missile targets a fixed position.</param>
        /// <param name="targetPosition">Fixed position targeted by the missile.</param>
        /// <param name="turnSpeed">Turning speed of the missile.</param>
        /// <param name="limitBossMissileAngle">Indicates whether the missile's angle should be limited when targeting a boss.</param>
        /// <param name="initialVelocity">Initial movement direction of the projectile.</param>
        public Projectiles(Texture2D texture, Vector2 position, Vector2 size, float speed, int damage = 1, int pierce = 0, bool isMissile = false, Enemy target = null, List<Enemy> enemyList = null, Boss bossTarget = null, bool hasPositionTarget = false, Vector2 targetPosition = default, float turnSpeed = 5f, bool limitBossMissileAngle = false, Vector2 initialVelocity = default)
        {
            Texture = texture;
            Position = position;
            Size = size;
            Speed = speed;
            Damage = damage;
            Pierce = pierce;
            IsMissile = isMissile;
            Target = target;
            BossTarget = bossTarget;
            EnemyList = enemyList;
            HasPositionTarget = hasPositionTarget;
            TargetPosition = targetPosition;
            Rotation = 0f;
            TurnSpeed = turnSpeed;
            _limitBossMissileAngle = limitBossMissileAngle;

            Hitbox = new XnaRectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);

            State = true;

            HitEnemies = new List<Enemy>();
            _bossHitCooldown = 0f;

            if (initialVelocity == Vector2.Zero)
            {
                _velocity = new Vector2(0, 1);
            }
            else
            {
                _velocity = initialVelocity;
                _velocity.Normalize();
            }
        }

        /// <summary>
        /// Registers a successful hit against the boss and starts the boss hit cooldown.
        /// </summary>
        public void RegisterBossHit()
        {
            _bossHitCooldown = BossHitCooldownDuration;
        }

        /// <summary>
        /// Updates the projectile position, movement, collision bounds, and active state.
        /// </summary>
        /// <param name="gameTime">Provides timing information for the current update.</param>
        /// <param name="screenWidth">Width of the game screen.</param>
        /// <param name="screenHeight">Height of the game screen.</param>
        /// <param name="direction">Vertical movement direction of the projectile.</param>
        public void Update(GameTime gameTime, int screenWidth, int screenHeight, int direction)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_bossHitCooldown > 0f)
            {
                _bossHitCooldown -= deltaTime;

                if (_bossHitCooldown < 0f)
                {
                    _bossHitCooldown = 0f;
                }
            }

            if (IsMissile)
            {
                UpdateMissile(deltaTime);
            }
            else
            {
                Position += new Vector2(0, Speed * direction * deltaTime);
            }

            Hitbox.X = (int)Position.X;
            Hitbox.Y = (int)Position.Y;
            Hitbox.Width = (int)Size.X;
            Hitbox.Height = (int)Size.Y;

            if (Position.Y < -Size.Y || Position.Y > screenHeight + Size.Y)
            {
                State = false;
            }
        }

        /// <summary>
        /// Updates the movement and targeting behavior of a homing missile.
        /// </summary>
        /// <param name="deltaTime">Elapsed time since the previous update.</param>
        private void UpdateMissile(float deltaTime)
        {
            if (HitEnemies.Count > 1)
                HitEnemies.RemoveAt(0);

            Vector2 targetCenter;

            if (HasPositionTarget)
            {
                targetCenter = TargetPosition;
            }
            else
            {
                if (BossTarget != null)
                {
                    if (!BossTarget.State)
                    {
                        State = false;
                        return;
                    }

                    targetCenter = BossTarget.Position + new Vector2(BossTarget.Size.Width / 2f, BossTarget.Size.Height / 2f);
                }
                else
                {
                    if (Target == null || !Target.State)
                    {
                        Target = FindNewTarget();

                        if (Target == null)
                        {
                            Position += _velocity * Speed * deltaTime;
                            return;
                        }
                    }

                    targetCenter = Target.Position + new Vector2(Target.Size.Width / 2f, Target.Size.Height / 2f);
                }
            }

            Vector2 missileCenter = Position + Size / 2f;
            Vector2 desiredDirection = targetCenter - missileCenter;

            if (desiredDirection != Vector2.Zero)
            {
                desiredDirection.Normalize();

                Vector2 currentDirection = _velocity;

                float currentAngle = (float)Math.Atan2(currentDirection.Y, currentDirection.X);
                float targetAngle = (float)Math.Atan2(desiredDirection.Y, desiredDirection.X);
                float angleDifference = MathHelper.WrapAngle(targetAngle - currentAngle);

                float maxTurn = TurnSpeed * deltaTime;
                float turnAmount = MathHelper.Clamp(angleDifference, -maxTurn, maxTurn);

                currentAngle += turnAmount;

                _velocity = new Vector2((float)Math.Cos(currentAngle), (float)Math.Sin(currentAngle));

                if (_limitBossMissileAngle && _velocity.Y > 0)
                {
                    float maxAngle = MathHelper.ToRadians(20f);
                    float verticalAngle = MathHelper.PiOver2;
                    float minimumAngle = verticalAngle - maxAngle;
                    float maximumAngle = verticalAngle + maxAngle;

                    float currentVerticalAngle = (float)Math.Atan2(_velocity.Y, _velocity.X);

                    currentVerticalAngle = MathHelper.WrapAngle(currentVerticalAngle);
                    currentVerticalAngle = MathHelper.Clamp(currentVerticalAngle, minimumAngle, maximumAngle);

                    _velocity = new Vector2((float)Math.Cos(currentVerticalAngle), (float)Math.Sin(currentVerticalAngle));
                    _velocity.Normalize();
                }

                Rotation = (float)Math.Atan2(_velocity.Y, _velocity.X) + MathHelper.PiOver2;
            }

            Position += _velocity * Speed * deltaTime;
        }

        /// <summary>
        /// Changes the missile to a new available enemy target.
        /// The target is not changed when the missile uses a fixed position or boss target.
        /// </summary>
        public void ChangeTarget()
        {
            if (HasPositionTarget || BossTarget != null)
                return;

            Target = FindNewTarget();

            if (Target == null)
            {
                State = false;
            }
        }

        /// <summary>
        /// Sets the projectile's movement velocity and normalizes the direction.
        /// </summary>
        /// <param name="velocity">The new velocity direction.</param>
        public void SetVelocity(Vector2 velocity)
        {
            _velocity = velocity;

            if (_velocity != Vector2.Zero)
                _velocity.Normalize();
        }

        /// <summary>
        /// Finds the nearest valid enemy that has not already been hit by the missile.
        /// </summary>
        /// <returns>The nearest available enemy, or <c>null</c> if no valid target exists.</returns>
        private Enemy FindNewTarget()
        {
            if (EnemyList == null)
                return null;

            Enemy nearestEnemy = null;
            float nearestDistance = float.MaxValue;

            foreach (Enemy enemy in EnemyList)
            {
                if (!enemy.State)
                    continue;

                if (HitEnemies.Contains(enemy))
                    continue;

                float distance = Vector2.Distance(Position, enemy.Position);

                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestEnemy = enemy;
                }
            }

            return nearestEnemy;
        }

        /// <summary>
        /// Draws the projectile using its normal or missile-specific rendering behavior.
        /// </summary>
        /// <param name="gameTime">Provides timing information for the current game frame.</param>
        /// <param name="spriteBatch">The sprite batch used to draw the projectile.</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (Texture == null)
                return;

            if (!IsMissile)
            {
                spriteBatch.Draw(Texture, new XnaRectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y), XnaColor.White);
                return;
            }

            Vector2 origin = new Vector2(Texture.Width / 2f, Texture.Height / 2f);
            Vector2 drawPosition = Position + Size / 2f;

            spriteBatch.Draw(Texture, drawPosition, null, XnaColor.White, Rotation, origin, 1f, SpriteEffects.None, 0f);
        }
    }
}