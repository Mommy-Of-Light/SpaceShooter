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
        public bool IsMissile;
        public List<Enemy> EnemyList;
        public List<Enemy> HitEnemies;
        public float Rotation;

        public Projectiles(Texture2D texture, Vector2 position, Vector2 size, float speed, int damage = 1, int pierce = 0, bool isMissile = false, Enemy target = null, List<Enemy> enemyList = null)
        {
            Texture = texture;
            Position = position;
            Size = size;
            Speed = speed;

            Damage = damage;
            Pierce = pierce;

            IsMissile = isMissile;
            Target = target;
            EnemyList = enemyList;

            Rotation = 0f;

            Hitbox = new XnaRectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);

            State = true;

            HitEnemies = new List<Enemy>();
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

            if (Position.Y < -Size.Y || Position.Y > screenHeight + Size.Y || Position.X < -Size.X || Position.X > screenWidth + Size.X)
            {
                State = false;
            }
        }

        private void UpdateMissile(float deltaTime)
        {
            if (Target == null || !Target.State)
            {
                Target = FindNewTarget();

                if (Target == null)
                {
                    Position.Y -= Speed * deltaTime;
                    return;
                }
            }

            Vector2 missileCenter = Position + Size / 2f;
            Vector2 targetCenter = Target.Position + new Vector2(Target.Size.Width / 2f, Target.Size.Height / 2f);
            Vector2 direction = targetCenter - missileCenter;

            if (direction != Vector2.Zero)
            {
                direction.Normalize();

                Position += direction * Speed * deltaTime;
                Rotation = (float)Math.Atan2(direction.Y, direction.X);

                Rotation += MathHelper.PiOver2;
            }
        }

        public void ChangeTarget()
        {
            Target = FindNewTarget();

            if (Target == null)
            {
                State = false;
            }
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
                spriteBatch.Draw(Texture, new XnaRectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y), XnaColor.White);
                return;
            }

            Vector2 origin = new Vector2(Texture.Width / 2f, Texture.Height / 2f);
            Vector2 drawPosition = Position + Size / 2f;

            spriteBatch.Draw(Texture, drawPosition, null, XnaColor.White, Rotation, origin, 1f, SpriteEffects.None, 0f);
        }
    }
}