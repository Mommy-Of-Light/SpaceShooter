namespace SpaceShooter
{
    /// <summary>
    /// Manages music playback and looping for the game.
    /// Stores the available tracks and controls the currently playing track.
    /// </summary>
    public class MusicPlayer
    {
        /// <summary>
        /// Gets the singleton instance of the <see cref="MusicPlayer"/> class.
        /// </summary>
        public static MusicPlayer Instance { get; private set; } = new MusicPlayer();

        /// <summary>
        /// Stores the available music tracks and their playback settings.
        /// </summary>
        private Dictionary<string, TrackSettings> _tracks = new Dictionary<string, TrackSettings>();

        /// <summary>
        /// Stores the settings of the currently playing track.
        /// </summary>
        private TrackSettings _currentTrack = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="MusicPlayer"/> class.
        /// </summary>
        private MusicPlayer() { }

        /// <summary>
        /// Initializes the music player and loads the available game tracks.
        /// </summary>
        /// <param name="game">The main game instance used to load the music assets.</param>
        public void Initialize(Game1 game)
        {
            MediaPlayer.IsRepeating = false;
            MediaPlayer.Volume = 0.7f;

            LoadTrack(game, "Menu", "Music/Spacey", 0, 109);
            LoadTrack(game, "Stage1", "Music/Wave After Wave! v0_9", 0, 10, 230);
        }

        /// <summary>
        /// Loads a music track with a loop that starts at the beginning of the track.
        /// </summary>
        /// <param name="game">The main game instance used to load the music asset.</param>
        /// <param name="name">The name used to identify the track.</param>
        /// <param name="assetPath">The content path of the music asset.</param>
        /// <param name="startSec">The starting position of the track in seconds.</param>
        /// <param name="loopEndSec">The position in seconds where the loop ends.</param>
        private void LoadTrack(Game1 game, string name, string assetPath, double startSec, double loopEndSec)
        {
            LoadTrack(game, name, assetPath, startSec, startSec, loopEndSec);
        }

        /// <summary>
        /// Loads a music track with configurable start, loop start, and loop end positions.
        /// </summary>
        /// <param name="game">The main game instance used to load the music asset.</param>
        /// <param name="name">The name used to identify the track.</param>
        /// <param name="assetPath">The content path of the music asset.</param>
        /// <param name="startSec">The starting position of the track in seconds.</param>
        /// <param name="loopStartSec">The position in seconds where the loop begins.</param>
        /// <param name="loopEndSec">The position in seconds where the loop ends.</param>
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

        /// <summary>
        /// Starts playing the specified music track.
        /// Adjusts the volume based on the selected track.
        /// </summary>
        /// <param name="name">The name of the track to play.</param>
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

        /// <summary>
        /// Updates the currently playing track and manually handles
        /// looping when the configured loop end position is reached.
        /// </summary>
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

        /// <summary>
        /// Stops the currently playing music and clears the current track.
        /// </summary>
        public void Stop()
        {
            MediaPlayer.Stop();
            _currentTrack = null;
        }
    }
}