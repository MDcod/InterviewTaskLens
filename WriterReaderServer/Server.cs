namespace WriterReaderServer;

public static class Server
{
    private static int count;

    public static int GetCount()
    {
        rwlock.EnterReadLock();
        try
        {
            return count;
        }
        finally
        {
            rwlock.ExitReadLock();
        }
    }
    
    public static void AddToCount(int value)
    {
        rwlock.EnterWriteLock();
        try
        {
            count += value;
        }
        finally
        {
            rwlock.ExitWriteLock();
        }
    }
    
    private static readonly ReaderWriterLockSlim rwlock = new ReaderWriterLockSlim();
}