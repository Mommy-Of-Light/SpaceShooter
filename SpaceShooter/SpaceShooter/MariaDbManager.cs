using DotNetEnv;
using MySqlConnector;

namespace SpaceShooter
{
    public static class MariaDbManager
    {
        private static readonly string ConnectionString = BuildConnectionString();

        public static string LastError { get; private set; } = "";

        private static string BuildConnectionString()
        {
            Env.Load();

            string server = Environment.GetEnvironmentVariable("DB_SERVER") ?? "localhost";
            string port = Environment.GetEnvironmentVariable("DB_PORT") ?? "3306";
            string database = Environment.GetEnvironmentVariable("DB_NAME") ?? "SpaceShooter";
            string user = Environment.GetEnvironmentVariable("DB_USER") ?? "";
            string password = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "";

            return $"Server={server};Port={port};Database={database};User ID={user};Password={password};";
        }

        public static void SaveScore(string pseudo, int score, int wave, string difficulty)
        {
            LastError = "";

            try
            {
                ScoreData existingScore = GetScores().FirstOrDefault(s => s.Pseudo == pseudo && s.Difficulty == difficulty);

                using MySqlConnection connection = new MySqlConnection(ConnectionString);

                string query = "";

                if (existingScore != null)
                {
                    if (score > existingScore.Score && existingScore.Difficulty == difficulty)
                    {
                        query =
                            "UPDATE scores " +
                            "SET score = @score, created_at = NOW(), wave = @wave " +
                            "WHERE pseudo = @pseudo AND difficulty = @difficulty";
                    }
                }
                else
                {
                    query =
                        "INSERT INTO scores (pseudo, score, wave, difficulty, created_at) " +
                        "VALUES (@pseudo, @score, @wave, @difficulty, NOW())";
                }

                connection.Open();

                using MySqlCommand command = new MySqlCommand(query, connection);

                command.Parameters.AddWithValue("@pseudo", pseudo);
                command.Parameters.AddWithValue("@score", score);
                command.Parameters.AddWithValue("@wave", wave);
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
                    "SELECT pseudo, score, difficulty, wave, created_at " +
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
                        Wave = reader.GetInt32("wave"),
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
        public int Wave { get; set; }
        public string Difficulty { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}