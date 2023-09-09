using System.Data;
using FlatsService.DbContext.Models;
using Microsoft.Data.Sqlite;

namespace FlatsService.DbContext.Helpers;

public static class QueryHelpers
{
    public static IEnumerable<Apartment> GetApartments(SqliteConnection connection)
    {
        var apartments = new Dictionary<int, Apartment>();

        using var command = new SqliteCommand(@"
                SELECT A.apartment_id, A.number_of_rooms, A.website_link, 
                       PH.date, PH.price
                FROM Apartments AS A
                LEFT JOIN PriceHistory AS PH
                ON A.apartment_id = PH.apartment_id", connection);

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
                    PriceHistory: new List<PriceHistoryItem>());
                apartments.Add(apartment.Id, apartment);
            }

            if (!reader.IsDBNull("date") && !reader.IsDBNull("price"))
            {
                var priceChangeDate = reader.GetDateTime("date");
                var priceHistoryItem = new PriceHistoryItem(
                    PriceChangeDate: priceChangeDate,
                    Price: reader.GetInt32("price"));

                apartment.PriceHistory.Add(priceHistoryItem);
            }
        }

        return apartments.Values.ToList();
    }
    
    public static int GetRowsCount(SqliteConnection connection, string tableName)
    {
        using var countCommand = new SqliteCommand($"SELECT COUNT(*) FROM {tableName};", connection);
        return Convert.ToInt32(countCommand.ExecuteScalar());
    }
}