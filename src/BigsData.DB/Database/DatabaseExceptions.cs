using System;

namespace BigsData.Database
{
    public class DatabaseException : Exception
    {
        internal DatabaseException(string message, Exception ex)
            : base(message, ex)
        { }

        internal DatabaseException(string message)
            : base(message)
        { }

        internal DatabaseException(Exception ex)
            : base(ex.Message, ex)
        { }

        internal static DatabaseException NotImplemented
        { get { return new DatabaseException(new NotImplementedException()); } }
    }

    public class ItemAlreadyExistsException : DatabaseException
    {
        internal ItemAlreadyExistsException(string path) :
            base($"'{path}' already exists.")
        { }
    }

    public class ItemNotFoundException : DatabaseException
    {
        internal ItemNotFoundException(string path)
            : base($"'{path}' not found.")
        { }
    }

    public class InvalidDatabaseOperationException : DatabaseException
    {
        public InvalidDatabaseOperationException(string message)
            : base(message)
        { }
    }
}
