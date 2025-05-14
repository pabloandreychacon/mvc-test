using NuGet.Protocol.Plugins;
using System.Data.SQLite;

namespace mvc_test
{
    public static class DatabaseHelper
    {
        private static string? _connectionString;

        // New method to configure from IConfiguration
        public static void Configure(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public static void Configure(string connectionString)
        {
            _connectionString = connectionString;
        }


        public static void Initialise()
        {
            if (_connectionString == null)
                throw new InvalidOperationException("DatabaseHelper not configured.");

            string dbRoute = _connectionString.Replace("Data Source=", "");

            if (!File.Exists(dbRoute))
            {
                SQLiteConnection.CreateFile(dbRoute);

                using var connection = new SQLiteConnection(_connectionString);
                connection.Open();

                string createExpensesTable = @"
                    CREATE TABLE [Expenses] (
                      [Id] bigint NOT NULL,
                      [Price] numeric(53,0) NOT NULL,
                      [Description] text DEFAULT ('Default') NOT NULL,
                      CONSTRAINT [sqlite_master_PK_Expenses] PRIMARY KEY ([Id])
                    );
                ";

                using var command = new SQLiteCommand(createExpensesTable, connection);
                command.ExecuteNonQuery();
            }
        }
    }
}
