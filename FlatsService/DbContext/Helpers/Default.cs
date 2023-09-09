using Microsoft.Data.Sqlite;

namespace FlatsService.DbContext.Helpers;

public static class Default
{
    public const string ConnectionString = "Data Source=flats.db";

    public static SqliteConnection OpenSqliteConnection()
    {
        var connection = new SqliteConnection(ConnectionString);
        connection.Open();
        return connection;
    }
}