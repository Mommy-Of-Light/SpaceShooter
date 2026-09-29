namespace SpaceShooter
{
    public class MusicPlayer
    {
        public static MusicPlayer Instance { get; private set; } = new MusicPlayer();

        private Dictionary<string, TrackSettings> _tracks = new Dictionary<string, TrackSettings>();
        private TrackSettings _currentTrack = null;

        private MusicPlayer() { }

        public void Initialize(Game1 game)
        {
            MediaPlayer.IsRepeating = false;
            MediaPlayer.Volume = 0.7f;

            LoadTrack(game, "Menu", "Music/Spacey", 0, 109);
            LoadTrack(game, "Stage1", "Music/Wave After Wave! v0_9", 0, 10, 230);
        }

        private void LoadTrack(Game1 game, string name, string assetPath, double startSec, double loopEndSec)
        {
            LoadTrack(game, name, assetPath, startSec, startSec, loopEndSec);
        }

        private void LoadTrack(Game1 game, string name, string assetPath, double startSec, double loopStartSec, double loopEndSec)
        {
            _tracks[name] = new TrackSettings
            {
                Song = game.Content.Load<Song>(assetPath),
                StartTime = TimeSpan.FromSeconds(startSec),
                LoopStart = TimeSpan.FromSeconds(loopStartSec),
                LoopEnd = TimeSpan.FromSeconds(loopEndSec)
            };
        }

        public void Play(string name)
        {
            if (name == "Menu")
            {
                MediaPlayer.Volume = 0.4f;
            }
            else
            {
                MediaPlayer.Volume = 0.7f;
            }

            if (_tracks.TryGetValue(name, out var track))
            {
                _currentTrack = track;
                MediaPlayer.Play(_currentTrack.Song, _currentTrack.StartTime);
            }
        }

        public void Update()
        {
            if (_currentTrack == null)
                return;

            if (MediaPlayer.PlayPosition >= _currentTrack.LoopEnd)
            {
                TimeSpan offset = MediaPlayer.PlayPosition - _currentTrack.LoopEnd;
                TimeSpan nextPosition = _currentTrack.LoopStart + offset;

                MediaPlayer.Play(_currentTrack.Song, nextPosition);
            }
        }

        public void Stop()
        {
            MediaPlayer.Stop();
            _currentTrack = null;
        }
    }
}