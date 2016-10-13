using BigsData.Database.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigsData.Database
{
    public class BigsDatabase
    {
        private readonly string _rootFolder;
        private readonly string _defaultDatabase;
        private readonly string _defaultCollection;
        private readonly bool _failSilently;
        private const string _guidFormat = "N";
        private static readonly Encoding _encoding = Encoding.UTF8;

        internal BigsDatabase(string baseFolder, string defaultDatabase = Constants.DefaultDatabaseName, string defaultCollection = Constants.DefaultCollectionName, bool failSilentlyOnReads = true)
        {
            _rootFolder = baseFolder;
            _defaultDatabase = defaultDatabase;
            _defaultCollection = defaultCollection;
            _failSilently = failSilentlyOnReads;
        }

        #region Public methods
        #region Create
        public async Task<ItemOperationResult<Guid>> Add<T>(T item, string collection = null, string database = null) where T : class, new()
        {
            var id = Guid.NewGuid();
            var idString = id.ToString(_guidFormat);

            var result = await AddItem(idString, item, collection, database);

            if (result)
                return ItemOperationResult.Sucessful(id);

            return ItemOperationResult.Failed<Guid>(result);
        }

        public async Task<ItemOperationResult<Guid>> Add<T>(Guid id, T item, string collection = null, string database = null) where T : class, new()
        {
            var idString = id.ToString(_guidFormat);

            var result = await AddItem(idString, item, collection, database);

            if (result)
                return ItemOperationResult.Sucessful(id);

            return ItemOperationResult.Failed<Guid>(result);
        }

        public async Task<ItemOperationResult<string>> Add<T>(string id, T item, string collection = null, string database = null) where T : class, new()
        {
            var result = await AddItem(id, item, collection, database);

            if (result)
                return ItemOperationResult.Sucessful(id);

            return ItemOperationResult.Failed<string>(result);
        }

        public Task<ItemOperationResult<Guid>> Add(Stream stream, string collection = null, string database = null)
        {
            return Task.FromResult(ItemOperationResult.Failed<Guid>(DatabaseException.NotImplemented));
        }

        public ItemOperationResult<Guid> Add(Guid id, Stream stream, string collection = null, string database = null)
        {
            return ItemOperationResult.Failed<Guid>(DatabaseException.NotImplemented);
        }

        public Task<ItemOperationResult<string>> Add(string id, Stream stream, string collection = null, string database = null)
        {
            return Task.FromResult(ItemOperationResult.Failed<string>(DatabaseException.NotImplemented));
        }
        #endregion

        #region Read
        public Task<T> Single<T>(Guid id, string collection = null, string database = null) where T : class, new()
        {
            return Single<T>(id.ToString(_guidFormat), collection, database);
        }

        public Task<T> Single<T>(string id, string collection = null, string database = null) where T : class, new()
        {
            var filePath = BuildItemPath(database, collection, id);
            if (!File.Exists(filePath))
                if (_failSilently)
                    return Task.FromResult(default(T));
                else
                    throw new ItemNotFoundException(filePath);

            return ReadItem<T>(filePath);
        }

        public IEnumerable<Task<T>> Query<T>(string collection = null, string database = null) where T : class, new()
        {
            var path = BuildCollectionPath(database, collection);

            if (!Directory.Exists(path))
                if (_failSilently)
                    return new Task<T>[0];

            return Directory.EnumerateFiles(path).Select(async file => await ReadItem<T>(file));
        }
        #endregion

        #region Update
        public Task<OperationResult> Update<T>(Guid id, T item, string collection = null, string database = null) where T : class, new()
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult> Update<T>(string id, T item, string collection = null, string database = null) where T : class, new()
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult> Update(Guid id, Stream stream, string collection = null, string database = null)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult> Update(string id, Stream stream, string collection = null, string database = null)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Delete
        public Task<OperationResult> Delete<T>(Guid id, string collection = null, string database = null) where T : class, new()
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult> Delete<T>(string id, string collection = null, string database = null) where T : class, new()
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult> Delete(Guid id, string collection = null, string database = null)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult> Delete(string id, string collection = null, string database = null)
        {
            throw new NotImplementedException();
        }
        #endregion
        #endregion

        #region Private methods
        private OperationResult GuaranteeFileSystemStructure(string database, string collection)
        {
            var path = BuildCollectionPath(database, collection);

            try
            {
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                return OperationResult.Successful;
            }
            catch (Exception ex)
            {
                return OperationResult.Failed(new DatabaseException("Error creating database folder", ex));
            }
        }

        private async Task<OperationResult> AddItem<T>(string id, T item, string collection, string database)
        {
            var pathCreated = GuaranteeFileSystemStructure(database, collection);

            if (!pathCreated)
                return pathCreated;

            var filePath = BuildItemPath(database, collection, id);

            if (File.Exists(filePath))
                return OperationResult.Failed(new ItemAlreadyExistsException(filePath));

            var json = Serializer.Serialize(item);
            var bytesToWrite = _encoding.GetBytes(json);

            using (var fileStream = File.OpenWrite(filePath))
                await fileStream.WriteAsync(bytesToWrite, 0, bytesToWrite.Length);

            return OperationResult.Successful;
        }

        private async Task<T> ReadItem<T>(string path)
        {
            using (var fileStream = File.OpenRead(path))
            {
                var bytesRead = new byte[fileStream.Length];
                await fileStream.ReadAsync(bytesRead, 0, (int)fileStream.Length);
                var json = _encoding.GetString(bytesRead);
                return Serializer.Deserialize<T>(json);
            }
        }

        private string BuildCollectionPath(string database, string collection)
        {
            return Path.Combine(
                _rootFolder,
                Constants.DatabasesFolder,
                string.IsNullOrEmpty(database) ? _defaultDatabase : database,
                Constants.CollectionsFolder,
                string.IsNullOrEmpty(collection) ? _defaultCollection : collection);
        }

        private string BuildItemPath(string database, string collection, string id)
        {
            return Path.Combine(
                BuildCollectionPath(database, collection),
                id);
        }
        #endregion
    }
}
