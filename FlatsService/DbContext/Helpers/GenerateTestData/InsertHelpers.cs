using FlatsService.Extensions;
using Microsoft.Data.Sqlite;

namespace FlatsService.DbContext.Helpers.GenerateTestData;

public static partial class Helpers
{
    public static void GenerateTestDataIfNeed(int dataCount = 10)
    {
        InsertApartmentData(dataCount);
        InsertPriceData(dataCount);
    }

    private static void InsertPriceData(int dataCount)
    {
        using var connection = Default.OpenSqliteConnection();

        var apartments = QueryHelpers.GetApartments(connection);

        using var insertCommand = new SqliteCommand(
            "INSERT INTO PriceHistory (apartment_id, date, price) VALUES (@apartmentId, @date, @price);", connection);
        insertCommand.Parameters.Add("@apartmentId", SqliteType.Integer);
        insertCommand.Parameters.Add("@date", SqliteType.Integer);
        insertCommand.Parameters.Add("@price", SqliteType.Integer);

        var random = new Random();
        var dates = new SortedSet<DateTime>();
        foreach (var i in Enumerable.Range(1, 10))
        {
            var lastDate = dates.Any() ? dates.Last() : new DateTime(2018, 5, 13);
            dates.Add(lastDate + TimeSpan.FromDays(random.Next(i + 1, i + 5) * 31));
        }
        
        foreach (var apartment in apartments)
        {
            if (apartment.PriceHistory.Count > 0) continue;
            
            var arrangeStartPrice = apartment.RoomsCount * 3000000;
            var startPrice = random.Next(arrangeStartPrice - 500000, arrangeStartPrice + 500000);
            for (var i = 0; i < dates.Count; i++)
            {
                var currentPrice = random.Next(0, 2) == 0
                    ? startPrice - i * (int)Math.Floor(startPrice * random.NextDoubleInRange(0.10, 0.20))
                    : startPrice + i * (int)Math.Floor(startPrice * random.NextDoubleInRange(0.10, 0.20));
                insertCommand.Parameters["@apartmentId"].Value = apartment.Id;
                insertCommand.Parameters["@date"].Value = dates.ElementAt(i);
                insertCommand.Parameters["@price"].Value = Math.Abs(currentPrice);
                insertCommand.ExecuteNonQuery();
            }
        }
    }

    private static void InsertApartmentData(int dataCount)
    {
        using var connection = new SqliteConnection(Default.ConnectionString);
        connection.Open();

        var rowCount = QueryHelpers.GetRowsCount(connection, "Apartments");
        if (rowCount >= dataCount) return;

        using var insertCommand = new SqliteCommand(
            "INSERT INTO Apartments (number_of_rooms, website_link) VALUES (@numberOfRooms, @websiteLink);", connection);
        insertCommand.Parameters.Add("@numberOfRooms", SqliteType.Integer);
        insertCommand.Parameters.Add("@websiteLink", SqliteType.Text);

        var r = new Random();
        for (var i = 0; i < dataCount; i++)
        {
            insertCommand.Parameters["@numberOfRooms"]
                .Value = r.Next(1, 6);
            insertCommand.Parameters["@websiteLink"]
                .Value = $"https://example.com/apartment/{r.Next(1000)}";
            insertCommand.ExecuteNonQuery();
        }
    }
}