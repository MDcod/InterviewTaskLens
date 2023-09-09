namespace FlatsService.DbContext.Models;

public record Apartment(int Id, int RoomsCount, string DeveloperSiteLink, SortedList<DateTime, PriceHistoryItem> PriceHistory)
{
    public PriceHistoryItem LastPrice => PriceHistory.Last().Value;
}