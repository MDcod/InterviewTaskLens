namespace FlatsService.DbContext.Models;

public record Apartment(int Id, int RoomsCount, string DeveloperSiteLink, List<PriceHistoryItem> PriceHistory);