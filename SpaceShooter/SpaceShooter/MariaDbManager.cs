using DotNetEnv;
using MySqlConnector;

namespace SpaceShooter
{
    /// <summary>
    /// Provides methods for connecting to the MariaDB database
    /// and managing player scores.
    /// </summary>
    public static class MariaDbManager
    {
        /// <summary>
        /// Connection string used to connect to the MariaDB database.
        /// </summary>
        private static readonly string ConnectionString = BuildConnectionString();

        /// <summary>
        /// Gets the last error message encountered while performing a database operation.
        /// </summary>
        public static string LastError { get; private set; } = "";

        /// <summary>
        /// Builds the database connection string using environment variables.
        /// Default values are used when optional environment variables are not defined.
        /// </summary>
        /// <returns>The connection string used to connect to the database.</returns>
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

        /// <summary>
        /// Saves a player's score to the database.
        /// If a score already exists for the same pseudo and difficulty,
        /// it is only updated when the new score is higher.
        /// </summary>
        /// <param name="pseudo">The player's pseudo.</param>
        /// <param name="score">The player's score.</param>
        /// <param name="wave">The wave reached by the player.</param>
        /// <param name="difficulty">The difficulty level of the game.</param>
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
                        query = "UPDATE scores " +
                                "SET score = @score, created_at = NOW(), wave = @wave " +
                                "WHERE pseudo = @pseudo AND difficulty = @difficulty";
                    }
                }
                else
                {
                    query = "INSERT INTO scores (pseudo, score, wave, difficulty, created_at) " +
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

        /// <summary>
        /// Retrieves all player scores from the database.
        /// Scores are ordered from highest to lowest, with earlier dates
        /// appearing first when scores are equal.
        /// </summary>
        /// <returns>A list containing the retrieved player scores.</returns>
        public static List<ScoreData> GetScores()
        {
            List<ScoreData> scores = new List<ScoreData>();

            LastError = "";

            try
            {
                using MySqlConnection connection = new MySqlConnection(ConnectionString);

                connection.Open();

                string query = "SELECT pseudo, score, difficulty, wave, created_at " +
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

    /// <summary>
    /// Represents the data associated with a player's score.
    /// </summary>
    public class ScoreData
    {
        /// <summary>
        /// Gets or sets the player's pseudo.
        /// </summary>
        public string Pseudo { get; set; }

        /// <summary>
        /// Gets or sets the player's score.
        /// </summary>
        public int Score { get; set; }

        /// <summary>
        /// Gets or sets the wave reached by the player.
        /// </summary>
        public int Wave { get; set; }

        /// <summary>
        /// Gets or sets the difficulty level of the game.
        /// </summary>
        public string Difficulty { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the score was created or updated.
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }
}