namespace FlatsService.DbContext.Models;

public record PriceHistory(SortedSet<PriceHistoryItem> Items);