namespace SpaceShooter
{
    /// <summary>
    /// Represents the data associated with a saved game.
    /// </summary>
    public class SaveData
    {
        /// <summary>
        /// Unique identifier of the save.
        /// </summary>
        public string SaveId { get; set; }

        /// <summary>
        /// Identifier of the game run associated with the save.
        /// </summary>
        public string RunId { get; set; }

        /// <summary>
        /// Name of the file used to store the save.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Pseudo of the player who created the save.
        /// </summary>
        public string Pseudo { get; set; }

        /// <summary>
        /// Date and time when the save was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Date and time when the save was last updated.
        /// </summary>
        public DateTime LastSavedAt { get; set; }

        /// <summary>
        /// Game data stored in the save.
        /// </summary>
        public GameData Game { get; set; }
    }
}