namespace SpaceShooter
{
    public static class SaveManager
    {
        private static readonly string SaveDirectory =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SpaceShooter");

        public static string GetSaveDirectory()
        {
            Directory.CreateDirectory(SaveDirectory);
            return SaveDirectory;
        }

        public static SaveData Save(GameData gameData, string pseudo, string runId)
        {
            Directory.CreateDirectory(SaveDirectory);

            if (string.IsNullOrEmpty(runId))
            {
                runId = Guid.NewGuid().ToString();
            }

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

        public static SaveData GetLatestSave(string pseudo)
        {
            List<SaveData> saves = GetSaves(pseudo);

            if (saves.Count == 0)
                return null;

            return saves[0];
        }

        public static bool Exists(string pseudo)
        {
            return GetLatestSave(pseudo) != null;
        }

        public static void Delete(SaveData save)
        {
            if (save == null)
                return;

            string filePath = null;

            if (!string.IsNullOrEmpty(save.FileName))
            {
                filePath = Path.Combine(SaveDirectory, save.FileName);
            }

            if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        public static void Delete(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                return;

            if (File.Exists(filePath))
                File.Delete(filePath);
        }

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

        private static string MakeSafeFileName(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return "Unknown";

            foreach (char invalidCharacter in Path.GetInvalidFileNameChars())
            {
                value = value.Replace(invalidCharacter.ToString(), "");
            }

            if (string.IsNullOrWhiteSpace(value))
                return "Unknown";

            return value;
        }
    }
}