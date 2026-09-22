namespace SpaceShooter
{
    public class ArchiveData
    {
        public string ArchiveId { get; set; }
        public string Pseudo { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime FinishedAt { get; set; }
        public int Score { get; set; }
        public int Wave { get; set; }
        public string Difficulty { get; set; }
        public double DifficultyMultiplier { get; set; }

        public ArchiveData()
        {
            Pseudo = "";
            Difficulty = "Medium";
        }
    }
}