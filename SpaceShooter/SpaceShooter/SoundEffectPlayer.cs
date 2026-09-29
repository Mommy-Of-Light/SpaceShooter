using Microsoft.Xna.Framework.Audio;

namespace SpaceShooter
{
    public class SoundEffectPlayer
    {
        public static SoundEffectPlayer Instance { get; private set; } = new SoundEffectPlayer();

        private SoundEffect Retro_lazer;
        private SoundEffect Snare_shot;
        private SoundEffect Space_zap;

        private SoundEffectPlayer()
        {
        }

        public void Initialize(Game1 game)
        {
            Retro_lazer = game.Content.Load<SoundEffect>("SoundEffects/freesound_community-retro_laser_gun_shot-96367");
            Snare_shot = game.Content.Load<SoundEffect>("SoundEffects/freesound_community-snare-space-shot-80932");
            Space_zap = game.Content.Load<SoundEffect>("SoundEffects/u_zryegfa0xr-space-zap-217411");
        }

        public void PlayRetroLazer()
        {
            if (Retro_lazer != null)
                Retro_lazer.Play(0.2f, 0.5f, 0);
        }

        public void PlaySnareShot()
        {
            if (Snare_shot != null)
                Snare_shot.Play(0.5f, 0.5f, 0);
        }

        public void PlaySpaceZap()
        {
            if (Space_zap != null)
                Space_zap.Play(1f, 1f, 1f);
        }
    }
}