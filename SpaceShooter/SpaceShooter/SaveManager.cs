namespace SpaceShooter
{
    /// <summary>
    /// Provides methods for creating, loading, retrieving, and deleting saved games.
    /// </summary>
    public static class SaveManager
    {
        /// <summary>
        /// Directory where the game's save files are stored.
        /// </summary>
        private static readonly string SaveDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SpaceShooter");

        /// <summary>
        /// Gets the directory used to store save files and creates it if necessary.
        /// </summary>
        /// <returns>The path to the save directory.</returns>
        public static string GetSaveDirectory()
        {
            Directory.CreateDirectory(SaveDirectory);
            return SaveDirectory;
        }

        /// <summary>
        /// Saves the current game data to a JSON file.
        /// </summary>
        /// <param name="gameData">Game data to save.</param>
        /// <param name="pseudo">Pseudo of the player associated with the save.</param>
        /// <param name="runId">Identifier of the current game run.</param>
        /// <returns>The <see cref="SaveData"/> object containing information about the created save.</returns>
        public static SaveData Save(GameData gameData, string pseudo, string runId)
        {
            Directory.CreateDirectory(SaveDirectory);

            if (string.IsNullOrEmpty(runId))
                runId = Guid.NewGuid().ToString();

            DateTime now = DateTime.Now;
            string timestamp = now.ToString("yyyyMMdd_HHmmss_fff");
            string safePseudo = MakeSafeFileName(pseudo);
            string saveId = safePseudo + "_" + timestamp;
            string fileName = "save_" + safePseudo + "_" + timestamp + ".json";
            string filePath = Path.Combine(SaveDirectory, fileName);

            gameData.RunId = runId;

            SaveData saveData = new SaveData
            {
                SaveId = saveId,
                RunId = runId,
                FileName = fileName,
                Pseudo = pseudo,
                CreatedAt = now,
                LastSavedAt = now,
                Game = gameData
            };

            string json = JsonSerializer.Serialize(saveData, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            File.WriteAllText(filePath, json);

            return saveData;
        }

        /// <summary>
        /// Loads a saved game from the specified file.
        /// </summary>
        /// <param name="filePath">Path to the save file.</param>
        /// <returns>The loaded <see cref="SaveData"/>, or <c>null</c> if the file does not exist or cannot be loaded.</returns>
        public static SaveData Load(string filePath)
        {
            if (!File.Exists(filePath))
                return null;

            try
            {
                string json = File.ReadAllText(filePath);
                return JsonSerializer.Deserialize<SaveData>(json);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Retrieves all valid saves belonging to the specified player.
        /// </summary>
        /// <param name="pseudo">Pseudo of the player whose saves should be retrieved.</param>
        /// <returns>A list of the player's valid saves, sorted from most recent to oldest.</returns>
        public static List<SaveData> GetSaves(string pseudo)
        {
            Directory.CreateDirectory(SaveDirectory);

            List<SaveData> saves = new List<SaveData>();

            string safePseudo = MakeSafeFileName(pseudo);
            string searchPattern = "save_" + safePseudo + "_*.json";
            string[] files = Directory.GetFiles(SaveDirectory, searchPattern);

            foreach (string file in files)
            {
                SaveData save = Load(file);

                if (save == null)
                    continue;

                if (save.Game == null)
                    continue;

                if (save.Pseudo != pseudo)
                    continue;

                saves.Add(save);
            }

            saves.Sort((a, b) => b.LastSavedAt.CompareTo(a.LastSavedAt));

            return saves;
        }

        /// <summary>
        /// Retrieves the most recently saved game for the specified player.
        /// </summary>
        /// <param name="pseudo">Pseudo of the player whose latest save should be retrieved.</param>
        /// <returns>The most recent <see cref="SaveData"/>, or <c>null</c> if no save exists.</returns>
        public static SaveData GetLatestSave(string pseudo)
        {
            List<SaveData> saves = GetSaves(pseudo);

            if (saves.Count == 0)
                return null;

            return saves[0];
        }

        /// <summary>
        /// Determines whether the specified player has at least one valid save.
        /// </summary>
        /// <param name="pseudo">Pseudo of the player to check.</param>
        /// <returns><c>true</c> if a save exists; otherwise, <c>false</c>.</returns>
        public static bool Exists(string pseudo)
        {
            return GetLatestSave(pseudo) != null;
        }

        /// <summary>
        /// Deletes the file associated with the specified save data.
        /// </summary>
        /// <param name="save">Save data identifying the file to delete.</param>
        public static void Delete(SaveData save)
        {
            if (save == null)
                return;

            string filePath = null;

            if (!string.IsNullOrEmpty(save.FileName))
                filePath = Path.Combine(SaveDirectory, save.FileName);

            if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath))
                File.Delete(filePath);
        }

        /// <summary>
        /// Deletes a save file using its file path.
        /// </summary>
        /// <param name="filePath">Path to the save file to delete.</param>
        public static void Delete(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                return;

            if (File.Exists(filePath))
                File.Delete(filePath);
        }

        /// <summary>
        /// Deletes all saves associated with a specific game run and player.
        /// </summary>
        /// <param name="pseudo">Pseudo of the player whose saves should be deleted.</param>
        /// <param name="runId">Identifier of the game run to delete.</param>
        public static void DeleteRun(string pseudo, string runId)
        {
            if (string.IsNullOrEmpty(runId))
                return;

            Directory.CreateDirectory(SaveDirectory);

            List<SaveData> saves = GetSaves(pseudo);

            foreach (SaveData save in saves)
            {
                if (save == null)
                    continue;

                if (save.RunId != runId)
                    continue;

                Delete(save);
            }
        }

        /// <summary>
        /// Deletes all saves associated with the specified player.
        /// </summary>
        /// <param name="pseudo">Pseudo of the player whose saves should be deleted.</param>
        public static void DeleteAll(string pseudo)
        {
            Directory.CreateDirectory(SaveDirectory);

            string safePseudo = MakeSafeFileName(pseudo);
            string searchPattern = "save_" + safePseudo + "_*.json";
            string[] files = Directory.GetFiles(SaveDirectory, searchPattern);

            foreach (string file in files)
            {
                try
                {
                    File.Delete(file);
                }
                catch
                {
                }
            }
        }

        /// <summary>
        /// Converts a value into a string that can safely be used as part of a file name.
        /// </summary>
        /// <param name="value">Value to sanitize.</param>
        /// <returns>A file-system-safe string, or <c>Unknown</c> when the value is empty or invalid.</returns>
        private static string MakeSafeFileName(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return "Unknown";

            foreach (char invalidCharacter in Path.GetInvalidFileNameChars())
                value = value.Replace(invalidCharacter.ToString(), "");

            if (string.IsNullOrWhiteSpace(value))
                return "Unknown";

            return value;
        }
    }
}