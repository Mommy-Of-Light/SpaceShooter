namespace SpaceShooter
{
    public abstract class GameScreen
    {
        protected Game1 Game;

        public GameScreen(Game1 game)
        {
            Game = game;
        }

        public virtual void Initialize()
        {
        }

        public virtual void Update(GameTime gameTime)
        {
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
        }

        public virtual void OnEnter()
        {
        }

        public virtual void OnExit()
        {
        }
    }
}