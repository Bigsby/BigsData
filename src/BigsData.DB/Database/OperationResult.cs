namespace BigsData.Database
{
    public struct OperationResult
    {
        private OperationResult(bool success, DatabaseException exception)
        {
            Success = success;
            Exception = exception;
        }

        public bool Success { get; private set; }
        public DatabaseException Exception { get; private set; }

        public static OperationResult Successful { get { return new OperationResult(true, null); } }

        public static OperationResult Failed(DatabaseException exception)
        {
            return new OperationResult(false, exception);
        }

        public static implicit operator bool(OperationResult result)
        {
            return result.Success;
        }
    }
}
