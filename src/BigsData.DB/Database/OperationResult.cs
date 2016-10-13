namespace BigsData.Database
{
    public class OperationResult
    {
        protected OperationResult(bool success = true, DatabaseException exception = null)
        {
            Success = success;
            Exception = exception;
        }

        public bool Success { get; private set; }
        public DatabaseException Exception { get; private set; }

        internal static OperationResult Successful { get { return new OperationResult(); } }

        internal static OperationResult Failed(DatabaseException exception)
        {
            return new OperationResult(false, exception);
        }

        public static implicit operator bool(OperationResult result)
        {
            return result.Success;
        }

        public static implicit operator DatabaseException(OperationResult result)
        {
            return result.Exception;
        }
    }

    internal static class ItemOperationResult
    {
        public static ItemOperationResult<T> Sucessful<T>(T itemId)
        {
            return ItemOperationResult<T>.Successful(itemId);
        }

        public static ItemOperationResult<T> Failed<T>(DatabaseException ex)
        {
            return ItemOperationResult<T>.Failed(ex);
        }
    }

    public sealed class ItemOperationResult<T> : OperationResult
    {
        private ItemOperationResult(T itemId, bool success = true, DatabaseException exception = null)
            : base(success, exception)
        {
            ItemId = itemId;
        }

        public T ItemId { get; private set; }

        internal static new ItemOperationResult<T> Successful(T itemId)
        {
            return new ItemOperationResult<T>(itemId);
        }

        internal static new ItemOperationResult<T> Failed(DatabaseException exception)
        {
            return new ItemOperationResult<T>(default(T), false, exception);
        }
    }
}
