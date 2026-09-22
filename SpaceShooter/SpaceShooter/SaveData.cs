namespace SpaceShooter
{
    public class SaveData
    {
        public string SaveId { get; set; }
        public string RunId { get; set; }
        public string FileName { get; set; }
        public string Pseudo { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastSavedAt { get; set; }
        public GameData Game { get; set; }
    }
}