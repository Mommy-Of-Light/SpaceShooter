namespace SpaceShooter
{
    /// <summary>
    /// Provides methods for saving and retrieving game archives.
    /// </summary>
    public static class ArchiveManager
    {
        /// <summary>
        /// Defines the directory where game archives are stored.
        /// </summary>
        private static readonly string ArchiveDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SpaceShooter");

        /// <summary>
        /// Saves the current game data as a JSON archive file.
        /// </summary>
        /// <param name="pseudo">The player's pseudonym.</param>
        /// <param name="score">The player's final score.</param>
        /// <param name="wave">The wave reached by the player.</param>
        /// <param name="difficulty">The difficulty level selected for the game.</param>
        /// <param name="difficultyMultiplier">The multiplier associated with the selected difficulty.</param>
        public static void SaveGameArchive(string pseudo, int score, int wave, string difficulty, double difficultyMultiplier)
        {
            Directory.CreateDirectory(ArchiveDirectory);
            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string safePseudo = MakeSafeFileName(pseudo);
            string archiveId = safePseudo + "_" + timestamp;

            ArchiveData archiveData = new ArchiveData
            {
                ArchiveId = archiveId,
                Pseudo = pseudo,
                CreatedAt = DateTime.Now,
                FinishedAt = DateTime.Now,
                Score = score,
                Wave = wave,
                Difficulty = difficulty,
                DifficultyMultiplier = difficultyMultiplier
            };

            string fileName = "archive_" + safePseudo + "_" + timestamp + ".json";
            string filePath = Path.Combine(ArchiveDirectory, fileName);
            string json = JsonSerializer.Serialize(archiveData, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            File.WriteAllText(filePath, json);
        }

        /// <summary>
        /// Retrieves all valid game archives associated with the specified player.
        /// </summary>
        /// <param name="pseudo">The player's pseudonym used to identify the archives.</param>
        /// <returns>A list of game archives ordered from the most recently finished to the oldest.</returns>
        public static List<ArchiveData> GetArchives(string pseudo)
        {
            List<ArchiveData> archives = new List<ArchiveData>();
            Directory.CreateDirectory(ArchiveDirectory);
            string safePseudo = MakeSafeFileName(pseudo);
            string searchPattern = "archive_" + safePseudo + "_*.json";
            string[] files = Directory.GetFiles(ArchiveDirectory, searchPattern);

            foreach (string file in files)
            {
                try
                {
                    string json = File.ReadAllText(file);
                    ArchiveData archive = JsonSerializer.Deserialize<ArchiveData>(json);

                    if (archive == null)
                        continue;

                    if (archive.Pseudo != pseudo)
                        continue;

                    archives.Add(archive);
                }
                catch
                {
                }
            }

            archives = archives.OrderByDescending(archive => archive.FinishedAt).ToList();
            return archives;
        }

        /// <summary>
        /// Converts a value into a valid file-name component by removing invalid characters.
        /// </summary>
        /// <param name="value">The value to sanitize for use in a file name.</param>
        /// <returns>A sanitized file-name component, or "Unknown" if the value is empty or contains only whitespace.</returns>
        private static string MakeSafeFileName(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return "Unknown";

            foreach (char invalidCharacter in Path.GetInvalidFileNameChars())
            {
                value = value.Replace(invalidCharacter.ToString(), "");
            }

            return value;
        }
    }
}