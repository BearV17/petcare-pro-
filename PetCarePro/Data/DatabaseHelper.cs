using System;
using Microsoft.Data.Sqlite;
using System.IO;

namespace PetCarePro.Data
{
    public static class DatabaseHelper
    {
        private static string _connectionString;
        private static string _dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "PetCarePro.db");

        public static string ConnectionString
        {
            get
            {
                if (_connectionString == null)
                {
                    _connectionString = $"Data Source={_dbPath}";
                }
                return _connectionString;
            }
        }

        public static void InitializeDatabase()
        {
            using (var connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();
                CreateTables(connection);
                InsertDefaultData(connection);
            }
        }

        private static void CreateTables(SqliteConnection connection)
        {
            // Users table
            string createUsersTable = @"
                CREATE TABLE IF NOT EXISTS Users (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Username TEXT NOT NULL UNIQUE,
                    Password TEXT NOT NULL,
                    Role TEXT NOT NULL,
                    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP
                );";

            // Owners table
            string createOwnersTable = @"
                CREATE TABLE IF NOT EXISTS Owners (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    FirstName TEXT NOT NULL,
                    LastName TEXT NOT NULL,
                    Email TEXT,
                    Phone TEXT,
                    Address TEXT,
                    City TEXT,
                    PostalCode TEXT,
                    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP
                );";

            // Pets table
            string createPetsTable = @"
                CREATE TABLE IF NOT EXISTS Pets (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    OwnerId INTEGER NOT NULL,
                    Name TEXT NOT NULL,
                    Species TEXT NOT NULL,
                    Breed TEXT,
                    Age INTEGER,
                    Gender TEXT,
                    ChipNumber TEXT,
                    PhotoPath TEXT,
                    Notes TEXT,
                    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
                    FOREIGN KEY (OwnerId) REFERENCES Owners(Id) ON DELETE CASCADE
                );";

            // Stays table (Verblijven)
            string createStaysTable = @"
                CREATE TABLE IF NOT EXISTS Stays (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    PetId INTEGER NOT NULL,
                    CheckInDate DATETIME NOT NULL,
                    CheckOutDate DATETIME,
                    Status TEXT NOT NULL,
                    KennelNumber TEXT,
                    Notes TEXT,
                    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
                    FOREIGN KEY (PetId) REFERENCES Pets(Id) ON DELETE CASCADE
                );";

            // MedicalRecords table
            string createMedicalRecordsTable = @"
                CREATE TABLE IF NOT EXISTS MedicalRecords (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    PetId INTEGER NOT NULL,
                    RecordDate DATETIME NOT NULL,
                    RecordType TEXT NOT NULL,
                    Description TEXT,
                    Veterinarian TEXT,
                    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
                    FOREIGN KEY (PetId) REFERENCES Pets(Id) ON DELETE CASCADE
                );";

            // CareSchedules table
            string createCareSchedulesTable = @"
                CREATE TABLE IF NOT EXISTS CareSchedules (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    PetId INTEGER NOT NULL,
                    StayId INTEGER,
                    ScheduleDate DATETIME NOT NULL,
                    MealTime TEXT,
                    MealType TEXT,
                    MealPortion TEXT,
                    Medication TEXT,
                    Notes TEXT,
                    Completed BOOLEAN DEFAULT 0,
                    FOREIGN KEY (PetId) REFERENCES Pets(Id) ON DELETE CASCADE,
                    FOREIGN KEY (StayId) REFERENCES Stays(Id) ON DELETE CASCADE
                );";

            ExecuteCommand(connection, createUsersTable);
            ExecuteCommand(connection, createOwnersTable);
            ExecuteCommand(connection, createPetsTable);
            ExecuteCommand(connection, createStaysTable);
            ExecuteCommand(connection, createMedicalRecordsTable);
            ExecuteCommand(connection, createCareSchedulesTable);
        }

        private static void InsertDefaultData(SqliteConnection connection)
        {
            // Check if admin user exists
            string checkAdmin = "SELECT COUNT(*) FROM Users WHERE Username = 'admin'";
            using (var cmd = new SqliteCommand(checkAdmin, connection))
            {
                var count = Convert.ToInt32(cmd.ExecuteScalar());
                if (count == 0)
                {
                    // Create default admin user (password: admin123)
                    string insertAdmin = @"
                        INSERT INTO Users (Username, Password, Role)
                        VALUES ('admin', 'admin123', 'Administrator')";
                    ExecuteCommand(connection, insertAdmin);
                }
            }
        }

        private static void ExecuteCommand(SqliteConnection connection, string commandText)
        {
            using (var command = new SqliteCommand(commandText, connection))
            {
                command.ExecuteNonQuery();
            }
        }

        public static SqliteConnection GetConnection()
        {
            return new SqliteConnection(ConnectionString);
        }

        public static System.Data.DataTable ExecuteDataTable(SqliteCommand command)
        {
            var dataTable = new System.Data.DataTable();
            using (var reader = command.ExecuteReader())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                    dataTable.Columns.Add(reader.GetName(i), typeof(string));
                while (reader.Read())
                {
                    var row = dataTable.NewRow();
                    for (int i = 0; i < reader.FieldCount; i++)
                        row[i] = reader.IsDBNull(i) ? DBNull.Value : reader[i].ToString();
                    dataTable.Rows.Add(row);
                }
            }
            return dataTable;
        }
    }
}
