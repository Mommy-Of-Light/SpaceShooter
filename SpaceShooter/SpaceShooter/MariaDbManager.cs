using MySqlConnector;

namespace SpaceShooter
{
    public static class MariaDbManager
    {
        private static readonly string ConnectionString =
            "Server=localhost;Port=3306;Database=SpaceShooter;User ID=bastien;Password=super;";

        public static string LastError { get; private set; }

        public static void SaveScore(string pseudo, int score, string difficulty)
        {
            LastError = "";

            try
            {
                using MySqlConnection connection = new MySqlConnection(ConnectionString);

                connection.Open();

                string query =
                    "INSERT INTO scores (pseudo, score, difficulty, created_at) " +
                    "VALUES (@pseudo, @score, @difficulty, NOW())";

                using MySqlCommand command = new MySqlCommand(query, connection);

                command.Parameters.AddWithValue("@pseudo", pseudo);
                command.Parameters.AddWithValue("@score", score);
                command.Parameters.AddWithValue("@difficulty", difficulty);

                command.ExecuteNonQuery();
            }
            catch (Exception exception)
            {
                LastError = exception.Message;
            }
        }

        public static List<ScoreData> GetScores()
        {
            List<ScoreData> scores = new List<ScoreData>();

            LastError = "";

            try
            {
                using MySqlConnection connection = new MySqlConnection(ConnectionString);

                connection.Open();

                string query =
                    "SELECT pseudo, score, difficulty, created_at " +
                    "FROM scores " +
                    "ORDER BY score DESC, created_at ASC";

                using MySqlCommand command = new MySqlCommand(query, connection);
                using MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    scores.Add(new ScoreData
                    {
                        Pseudo = reader.GetString("pseudo"),
                        Score = reader.GetInt32("score"),
                        Difficulty = reader.GetString("difficulty"),
                        CreatedAt = reader.GetDateTime("created_at")
                    });
                }
            }
            catch (Exception exception)
            {
                LastError = exception.Message;
            }

            return scores;
        }
    }

    public class ScoreData
    {
        public string Pseudo { get; set; }
        public int Score { get; set; }
        public string Difficulty { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}