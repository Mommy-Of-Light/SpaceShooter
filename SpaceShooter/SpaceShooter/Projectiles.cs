namespace SpaceShooter
{
    public class Projectiles
    {
        public Texture2D Texture;
        public XnaRectangle Hitbox;
        public Vector2 Position;
        public Vector2 Size;
        public float Speed;
        public bool State;
        public int Damage;
        public int Pierce;
        public Enemy Target;
        public Boss BossTarget;
        public bool IsMissile;
        public bool HasPositionTarget;
        public Vector2 TargetPosition;
        public List<Enemy> EnemyList;
        public List<Enemy> HitEnemies;
        public bool HitBoss;
        public float Rotation;
        public float TurnSpeed = 5f;
        public Vector2 Velocity => _velocity;
        private Vector2 _velocity;
        private bool _limitBossMissileAngle;

        public Projectiles(
            Texture2D texture,
            Vector2 position,
            Vector2 size,
            float speed,
            int damage = 1,
            int pierce = 0,
            bool isMissile = false,
            Enemy target = null,
            List<Enemy> enemyList = null,
            Boss bossTarget = null,
            bool hasPositionTarget = false,
            Vector2 targetPosition = default,
            float turnSpeed = 5f,
            bool limitBossMissileAngle = false,
            Vector2 initialVelocity = default)
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

            Hitbox = new XnaRectangle(
                (int)position.X,
                (int)position.Y,
                (int)size.X,
                (int)size.Y
            );

            State = true;

            HitEnemies = new List<Enemy>();
            HitBoss = false;

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

        public void Update(GameTime gameTime, int screenWidth, int screenHeight, int direction)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

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

            if (Position.Y < -Size.Y || Position.Y > screenHeight + Size.Y
            //|| Position.X < -Size.X || Position.X > screenWidth + Size.X
            )
            {
                State = false;
            }
        }

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

                    targetCenter = BossTarget.Position +
                        new Vector2(
                            BossTarget.Size.Width / 2f,
                            BossTarget.Size.Height / 2f
                        );
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

                    targetCenter = Target.Position +
                        new Vector2(
                            Target.Size.Width / 2f,
                            Target.Size.Height / 2f
                        );
                }
            }

            Vector2 missileCenter = Position + Size / 2f;
            Vector2 desiredDirection = targetCenter - missileCenter;

            if (desiredDirection != Vector2.Zero)
            {
                desiredDirection.Normalize();

                Vector2 currentDirection = _velocity;

                float currentAngle = (float)Math.Atan2(
                    currentDirection.Y,
                    currentDirection.X
                );

                float targetAngle = (float)Math.Atan2(
                    desiredDirection.Y,
                    desiredDirection.X
                );

                float angleDifference = MathHelper.WrapAngle(
                    targetAngle - currentAngle
                );

                float maxTurn = TurnSpeed * deltaTime;
                float turnAmount = MathHelper.Clamp(
                    angleDifference,
                    -maxTurn,
                    maxTurn
                );

                currentAngle += turnAmount;

                _velocity = new Vector2(
                    (float)Math.Cos(currentAngle),
                    (float)Math.Sin(currentAngle)
                );

                /*
                 * Boss homing missiles are limited to a 20 degree
                 * angle from the vertical downward direction.
                 *
                 * Straight down:
                 *      90 degrees
                 *
                 * Maximum left:
                 *      120 degrees
                 *
                 * Maximum right:
                 *      60 degrees
                 */
                if (_limitBossMissileAngle && _velocity.Y > 0)
                {
                    float maxAngle = MathHelper.ToRadians(20f);

                    float verticalAngle = MathHelper.PiOver2;

                    float minimumAngle = verticalAngle - maxAngle;
                    float maximumAngle = verticalAngle + maxAngle;

                    float currentVerticalAngle = (float)Math.Atan2(
                        _velocity.Y,
                        _velocity.X
                    );

                    currentVerticalAngle = MathHelper.WrapAngle(
                        currentVerticalAngle
                    );

                    currentVerticalAngle = MathHelper.Clamp(
                        currentVerticalAngle,
                        minimumAngle,
                        maximumAngle
                    );

                    _velocity = new Vector2(
                        (float)Math.Cos(currentVerticalAngle),
                        (float)Math.Sin(currentVerticalAngle)
                    );

                    _velocity.Normalize();
                }

                Rotation = (float)Math.Atan2(
                    _velocity.Y,
                    _velocity.X
                ) + MathHelper.PiOver2;
            }

            Position += _velocity * Speed * deltaTime;
        }

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

        public void SetVelocity(Vector2 velocity)
        {
            _velocity = velocity;

            if (_velocity != Vector2.Zero)
                _velocity.Normalize();
        }

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

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (Texture == null)
                return;

            if (!IsMissile)
            {
                spriteBatch.Draw(
                    Texture,
                    new XnaRectangle(
                        (int)Position.X,
                        (int)Position.Y,
                        (int)Size.X,
                        (int)Size.Y
                    ),
                    XnaColor.White
                );

                return;
            }

            Vector2 origin = new Vector2(
                Texture.Width / 2f,
                Texture.Height / 2f
            );

            Vector2 drawPosition = Position + Size / 2f;

            spriteBatch.Draw(
                Texture,
                drawPosition,
                null,
                XnaColor.White,
                Rotation,
                origin,
                1f,
                SpriteEffects.None,
                0f
            );
        }
    }
}