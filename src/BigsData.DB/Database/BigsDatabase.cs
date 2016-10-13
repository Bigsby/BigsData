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

        internal BigsDatabase(string baseFolder, string defaultDatabase = "database", string defaultCollection = "collection")
        {
            _baseFolder = baseFolder;
            _defaultDatabase = defaultDatabase;
            _defaultCollection = defaultCollection;
        }

        public Guid Add<T>(T item, string collection = null, string database = null) where T : class, new()
        {
            throw new NotImplementedException();
        }

        public bool Add<T>(Guid id, T item, string collection = null, string database = null) where T : class, new()
        {
            throw new NotImplementedException();
        }

        public bool Add<T>(string id, T item, string collection = null, string database = null) where T : class, new()
        {
            throw new NotImplementedException();
        }

        public Guid Add(Stream stream, string collection = null, string database = null)
        {
            throw new NotImplementedException();
        }

        public bool Add(Guid id, Stream stream, string collection = null, string database = null)
        {
            throw new NotImplementedException();
        }

        public bool Add(string id, Stream stream, string collection = null, string database = null)
        {
            throw new NotImplementedException();
        }

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

        public bool Update<T>(Guid id, T item, string collection = null, string database = null) where T : class, new()
        {
            throw new NotImplementedException();
        }

        public bool Update<T>(string id, T item, string collection = null, string database = null) where T : class, new()
        {
            throw new NotImplementedException();
        }

        public bool Update(Guid id, Stream stream, string collection = null, string database = null)
        {
            throw new NotImplementedException();
        }

        public bool Update(string id, Stream stream, string collection = null, string database = null)
        {
            throw new NotImplementedException();
        }

        public bool Delete<T>(Guid id, string collection = null, string database = null) where T : class, new()
        {
            throw new NotImplementedException();
        }

        public bool Delete<T>(string id, string collection = null, string database = null) where T : class, new()
        {
            throw new NotImplementedException();
        }

        public bool Delete(Guid id, string collection = null, string database = null)
        {
            throw new NotImplementedException();
        }

        public bool Delete(string id, string collection = null, string database = null)
        {
            throw new NotImplementedException();
        }
    }
}
