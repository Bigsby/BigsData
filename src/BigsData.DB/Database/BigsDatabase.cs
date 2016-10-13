using System;
using System.Collections.Generic;
using System.IO;

namespace BigsData.Database
{
    public class BigsDatabase
    {
        private readonly string _baseFolder;
        private readonly string _defaultDatabase;
        private readonly string _defaultCollection;
        
        internal BigsDatabase(string baseFolder, string defaultDatabase = Constants.DefaultDatabaseName, string defaultCollection = Constants.DefaultCollectionName)
        {
            _baseFolder = baseFolder;
            _defaultDatabase = defaultDatabase;
            _defaultCollection = defaultCollection;
        }

        #region Create
        public ItemOperationResult<Guid> Add<T>(T item, string collection = null, string database = null) where T : class, new()
        {
            throw new NotImplementedException();
        }

        public ItemOperationResult<Guid> Add<T>(Guid id, T item, string collection = null, string database = null) where T : class, new()
        {
            throw new NotImplementedException();
        }

        public ItemOperationResult<string> Add<T>(string id, T item, string collection = null, string database = null) where T : class, new()
        {
            throw new NotImplementedException();
        }

        public ItemOperationResult<Guid> Add(Stream stream, string collection = null, string database = null)
        {
            throw new NotImplementedException();
        }

        public ItemOperationResult<Guid> Add(Guid id, Stream stream, string collection = null, string database = null)
        {
            throw new NotImplementedException();
        }

        public ItemOperationResult<string> Add(string id, Stream stream, string collection = null, string database = null)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Read
        public T Single<T>(Guid id, string collection = null, string database = null) where T : class, new()
        {
            throw new NotImplementedException();
        }

        public T Single<T>(string id, string collection = null, string database = null) where T : class, new()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> Query<T>(string collection = null, string database = null) where T : class, new()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Update
        public OperationResult Update<T>(Guid id, T item, string collection = null, string database = null) where T : class, new()
        {
            throw new NotImplementedException();
        }

        public OperationResult Update<T>(string id, T item, string collection = null, string database = null) where T : class, new()
        {
            throw new NotImplementedException();
        }

        public OperationResult Update(Guid id, Stream stream, string collection = null, string database = null)
        {
            throw new NotImplementedException();
        }

        public OperationResult Update(string id, Stream stream, string collection = null, string database = null)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Delete
        public OperationResult Delete<T>(Guid id, string collection = null, string database = null) where T : class, new()
        {
            throw new NotImplementedException();
        }

        public OperationResult Delete<T>(string id, string collection = null, string database = null) where T : class, new()
        {
            throw new NotImplementedException();
        }

        public OperationResult Delete(Guid id, string collection = null, string database = null)
        {
            throw new NotImplementedException();
        }

        public OperationResult Delete(string id, string collection = null, string database = null)
        {
            throw new NotImplementedException();
        } 
        #endregion
    }
}
