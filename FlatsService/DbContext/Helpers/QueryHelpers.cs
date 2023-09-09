using System.Data;
using FlatsService.DbContext.Models;
using Microsoft.Data.Sqlite;

namespace FlatsService.DbContext.Helpers;

public static class QueryHelpers
{
    // public static List<Apartment> GetApartments() => GetApartments(CreateSqliteConnection());

    // public static List<Apartment> GetApartments(SqliteConnection connection)
    // {
    //     var result = new List<Apartment>();
    //     var command = new SqliteCommand("SELECT * FROM Apartments;", connection);
    //     using var reader = command.ExecuteReader();
    //     if (!reader.HasRows) return result;
    //     while (reader.Read())
    //     {
    //         var id = reader.GetInt32("apartment_id");
    //         var roomsCount = reader.GetInt32("number_of_rooms");
    //         var link = reader.GetString("website_link");
    //         result.Add(new Apartment(id, roomsCount, link, new SortedList<DateTime, PriceHistoryItem>()));
    //     }
    //
    //     return result;
    // }

    // public static List<PriceHistoryItem> GetPriceHistory()
    // {
    //     using var connection = new SqliteConnection(Default.ConnectionString);
    //     connection.Open();
    //
    //     var result = new List<PriceHistoryItem>();
    //     var command = new SqliteCommand("SELECT * FROM PriceHistory;", connection);
    //     using var reader = command.ExecuteReader();
    //     if (!reader.HasRows) return result;
    //     while (reader.Read())
    //     {
    //         var id = reader.GetInt32("price_id");
    //         var apartmentId = reader.GetInt32("apartment_id");
    //         var date = reader.GetDateTime("date");
    //         var price = reader.GetInt32("price");
    //         result.Add(new PriceHistoryItem(id, apartmentId, date, price));
    //     }
    //
    //     return result;
    // }

    public static int GetRowsCount(SqliteConnection connection, string tableName)
    {
        using var countCommand = new SqliteCommand($"SELECT COUNT(*) FROM {tableName};", connection);
        return Convert.ToInt32(countCommand.ExecuteScalar());
    }

    public static IEnumerable<Apartment> GetApartments() => GetApartments(Default.OpenSqliteConnection());
    public static IEnumerable<Apartment> GetApartments(SqliteConnection connection)
    {
        var apartments = new Dictionary<int, Apartment>();
        
        const string sql = @"
                SELECT A.apartment_id, A.number_of_rooms, A.website_link, 
                       PH.date, PH.price
                FROM Apartments AS A
                LEFT JOIN PriceHistory AS PH
                ON A.apartment_id = PH.apartment_id";

        using var command = new SqliteCommand(sql, connection);

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            var id = reader.GetInt32("apartment_id");
            Apartment apartment;
            if (apartments.TryGetValue(id, out var apart))
                apartment = apart;
            else
            {
                apartment = new Apartment(
                    Id: id,
                    RoomsCount: reader.GetInt32("number_of_rooms"),
                    DeveloperSiteLink: reader.GetString("website_link"),
                    PriceHistory: new SortedList<DateTime, PriceHistoryItem>());
                apartments.Add(apartment.Id, apartment);
            }

            if (!reader.IsDBNull(3) && !reader.IsDBNull(4))
            {
                var priceChangeDate = reader.GetDateTime("date");
                var priceHistoryItem = new PriceHistoryItem(
                    PriceChangeDate: priceChangeDate,
                    Price: reader.GetInt32("price"));

                apartment.PriceHistory.Add(priceChangeDate, priceHistoryItem);
            }
        }

        return apartments.Values.ToList();
    }

    public static Apartment? GetApartment(int id)
    {
        Apartment? apartment = null;

        using var connection = Default.OpenSqliteConnection();

        const string sql = @"
                SELECT A.apartment_id, A.number_of_rooms, A.website_link, 
                       PH.date, PH.price
                FROM Apartments AS A
                LEFT JOIN PriceHistory AS PH
                ON A.apartment_id = PH.apartment_id
                WHERE A.apartment_id = @targetApartmentId";

        using var command = new SqliteCommand(sql, connection);
        command.Parameters.AddWithValue("@targetApartmentId", id);

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            apartment ??= new Apartment(
                Id: reader.GetInt32("apartment_id"),
                RoomsCount: reader.GetInt32("number_of_rooms"),
                DeveloperSiteLink: reader.GetString("website_link"),
                PriceHistory: new SortedList<DateTime, PriceHistoryItem>());

            if (reader.IsDBNull(3) || reader.IsDBNull(4)) continue;

            var priceChangeDate = reader.GetDateTime("date");
            var priceHistoryItem = new PriceHistoryItem(
                PriceChangeDate: priceChangeDate,
                Price: reader.GetInt32("price"));

            apartment.PriceHistory.Add(priceChangeDate, priceHistoryItem);
        }

        return apartment;
    }
}