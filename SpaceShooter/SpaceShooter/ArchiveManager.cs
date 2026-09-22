namespace SpaceShooter
{
    public static class ArchiveManager
    {
        private static readonly string ArchiveDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SpaceShooter");

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