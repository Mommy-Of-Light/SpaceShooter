namespace SpaceShooter
{
    /// <summary>
    /// Stores playback settings for a music track, including its start time and loop points.
    /// </summary>
    public class TrackSettings
    {
        /// <summary>
        /// Gets or sets the music track associated with these settings.
        /// </summary>
        public Song Song { get; set; }

        /// <summary>
        /// Gets or sets the time position at which the track should start playing.
        /// </summary>
        public TimeSpan StartTime { get; set; }

        /// <summary>
        /// Gets or sets the time position where the music loop begins.
        /// </summary>
        public TimeSpan LoopStart { get; set; }

        /// <summary>
        /// Gets or sets the time position where the music loop ends.
        /// </summary>
        public TimeSpan LoopEnd { get; set; }
    }
}