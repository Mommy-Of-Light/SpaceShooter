namespace SpaceShooter
{
    /// <summary>
    /// Represents the data saved for a completed game archive.
    /// </summary>
    public class ArchiveData
    {
        /// <summary>
        /// Gets or sets the unique identifier of the archive.
        /// </summary>
        public string ArchiveId { get; set; }

        /// <summary>
        /// Gets or sets the player's pseudonym.
        /// </summary>
        public string Pseudo { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the game was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the game was finished.
        /// </summary>
        public DateTime FinishedAt { get; set; }

        /// <summary>
        /// Gets or sets the player's final score.
        /// </summary>
        public int Score { get; set; }

        /// <summary>
        /// Gets or sets the wave reached by the player.
        /// </summary>
        public int Wave { get; set; }

        /// <summary>
        /// Gets or sets the difficulty level of the game.
        /// </summary>
        public string Difficulty { get; set; }

        /// <summary>
        /// Gets or sets the multiplier applied to the selected difficulty.
        /// </summary>
        public double DifficultyMultiplier { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArchiveData"/> class
        /// with default values for the player's pseudonym and difficulty.
        /// </summary>
        public ArchiveData()
        {
            Pseudo = "";
            Difficulty = "Medium";
        }
    }
}