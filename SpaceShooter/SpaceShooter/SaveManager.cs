namespace SpaceShooter
{
    public static class SaveManager
    {
        private static readonly string SaveDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SpaceShooter");
        private static readonly string SavePath = Path.Combine(SaveDirectory, "save.json");

        public static void Save(GameData data)
        {
            try
            {
                if (!Directory.Exists(SaveDirectory))
                {
                    Directory.CreateDirectory(SaveDirectory);
                }

                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    WriteIndented = true
                };

                string json = JsonSerializer.Serialize(data, options);

                File.WriteAllText(SavePath, json);
            }
            catch (Exception)
            {
                // You can add logging here if desired.
            }
        }

        public static GameData Load()
        {
            try
            {
                if (!File.Exists(SavePath))
                {
                    return null;
                }

                string json = File.ReadAllText(SavePath);

                return JsonSerializer.Deserialize<GameData>(json);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static bool Exists()
        {
            return File.Exists(SavePath);
        }

        public static void Delete()
        {
            try
            {
                if (File.Exists(SavePath))
                {
                    File.Delete(SavePath);
                }
            }
            catch (Exception)
            {
            }
        }
    }
}