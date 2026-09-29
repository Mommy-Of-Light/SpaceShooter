using Microsoft.Xna.Framework.Audio;

namespace SpaceShooter
{
    /// <summary>
    /// Manages the game's sound effects and provides methods for playing them.
    /// </summary>
    public class SoundEffectPlayer
    {
        /// <summary>
        /// Gets the single shared instance of the <see cref="SoundEffectPlayer"/> class.
        /// </summary>
        public static SoundEffectPlayer Instance { get; private set; } = new SoundEffectPlayer();

        /// <summary>
        /// Sound effect used for the retro laser shot.
        /// </summary>
        private SoundEffect Retro_lazer;

        /// <summary>
        /// Sound effect used for the snare space shot.
        /// </summary>
        private SoundEffect Snare_shot;

        /// <summary>
        /// Sound effect used for the space zap sound.
        /// </summary>
        private SoundEffect Space_zap;

        /// <summary>
        /// Initializes a new instance of the <see cref="SoundEffectPlayer"/> class.
        /// The constructor is private because this class uses the singleton pattern.
        /// </summary>
        private SoundEffectPlayer()
        {
        }

        /// <summary>
        /// Loads all sound effects used by the game.
        /// </summary>
        /// <param name="game">Reference to the main game instance used to access the content manager.</param>
        public void Initialize(Game1 game)
        {
            Retro_lazer = game.Content.Load<SoundEffect>("SoundEffects/freesound_community-retro_laser_gun_shot-96367");
            Snare_shot = game.Content.Load<SoundEffect>("SoundEffects/freesound_community-snare-space-shot-80932");
            Space_zap = game.Content.Load<SoundEffect>("SoundEffects/u_zryegfa0xr-space-zap-217411");
        }

        /// <summary>
        /// Plays the retro laser sound effect.
        /// </summary>
        public void PlayRetroLazer()
        {
            if (Retro_lazer != null)
                Retro_lazer.Play(0.2f, 0.5f, 0);
        }

        /// <summary>
        /// Plays the snare space shot sound effect.
        /// </summary>
        public void PlaySnareShot()
        {
            if (Snare_shot != null)
                Snare_shot.Play(0.5f, 0.5f, 0);
        }

        /// <summary>
        /// Plays the space zap sound effect.
        /// </summary>
        public void PlaySpaceZap()
        {
            if (Space_zap != null)
                Space_zap.Play(1f, 1f, 1f);
        }
    }
}