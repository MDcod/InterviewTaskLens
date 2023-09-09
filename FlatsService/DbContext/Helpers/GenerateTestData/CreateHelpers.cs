using Microsoft.Data.Sqlite;

namespace FlatsService.DbContext.Helpers.GenerateTestData;

public static partial class Helpers
{
    public static void CreateTablesIfNeed()
    {
        // Создаем базу данных, если она не существует
        using var connection = Default.OpenSqliteConnection();

        // Создание таблицы "Apartments"
        using var createApartmentsTable = new SqliteCommand(
            "CREATE TABLE IF NOT EXISTS Apartments (" +
            "apartment_id INTEGER PRIMARY KEY," +
            "number_of_rooms INTEGER," +
            "website_link TEXT);", connection);
        createApartmentsTable.ExecuteNonQuery();

        // Создание таблицы "PriceHistory"
        using var createPriceHistoryTable = new SqliteCommand(
            "CREATE TABLE IF NOT EXISTS PriceHistory (" +
            "price_id INTEGER PRIMARY KEY," +
            "apartment_id INTEGER," +
            "date DATETIME," +
            "price INTEGER," +
            "FOREIGN KEY (apartment_id) REFERENCES Apartments(apartment_id));", connection);
        createPriceHistoryTable.ExecuteNonQuery();
    }
}