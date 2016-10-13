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
        private readonly bool _trackReferences;
        private const string _guidFormat = "N";
        private static readonly Encoding _encoding = Encoding.UTF8;
        private readonly IDictionary<WeakReference, ItemReference> _references;

        internal BigsDatabase(string baseFolder, string defaultDatabase = Constants.DefaultDatabaseName, string defaultCollection = Constants.DefaultCollectionName, bool failSilentlyOnReads = true, bool trackReferences = false)
        {
            _rootFolder = baseFolder;
            _defaultDatabase = defaultDatabase;
            _defaultCollection = defaultCollection;
            _failSilently = failSilentlyOnReads;
            _trackReferences = trackReferences;
            _references = new Dictionary<WeakReference, ItemReference>();
        }

        #region Public methods
        #region Create
        public async Task<ItemOperationResult> Add<T>(T item, string collection = null, string database = null) where T : class, new()
        {
            var id = NewId();

            return await Add(id, item, collection, database);
        }

        public async Task<ItemOperationResult> Add<T>(string id, T item, string collection = null, string database = null) where T : class, new()
        {
            return await Task.Run(() => AddInt(id, item, collection, database, 
                async fileStream =>
                {
                    var json = Serializer.Serialize(item);
                    var bytesToWrite = _encoding.GetBytes(json);
                    await fileStream.WriteAsync(bytesToWrite, 0, bytesToWrite.Length);
                })); 
        }

        public async Task<ItemOperationResult> Add(Stream stream, string collection = null, string database = null)
        {
            var id = NewId();
            return await AddStream(id, stream, collection, database);
        }

        public async Task<ItemOperationResult> AddStream(string id, Stream stream, string collection = null, string database = null)
        {
            return await Task.Run(() => AddInt(id, stream, collection, database, 
                async fileStream => await stream.CopyToAsync(fileStream)));
        }
        #endregion

        #region Read
        public Task<T> Single<T>(string id, string collection = null, string database = null) where T : class, new()
        {
            var itemReference = BuildItemReference(id, collection, database);

            if (!File.Exists(itemReference.RootFullPath))
                if (_failSilently)
                    return Task.FromResult(default(T));
                else
                    throw new ItemNotFoundException(itemReference.FullPath);

            return ReadItem<T>(itemReference);
        }

        public IEnumerable<T> Query<T>(string collection = null, string database = null) where T : class, new()
        {
            var reference = BuildItemReference(null, collection, database);

            if (!Directory.Exists(reference.RootCollectionPath))
                if (_failSilently)
                    return new T[0];

            return Directory.EnumerateFiles(reference.RootCollectionPath)
                .Select(file => 
                ReadItem<T>(
                    new ItemReference(
                        _rootFolder,
                        reference.Database,
                        reference.Collection,
                        Path.GetFileName(file)
                    )).Result);
        }

        public Stream GetSteam(string id, string collection = null, string database = null)
        {
            var itemReference = BuildItemReference(id, collection, database);
            
            if (!File.Exists(itemReference.RootFullPath))
                if (_failSilently)
                    return Stream.Null;
                else
                    throw new ItemNotFoundException(itemReference.FullPath);

            return File.OpenRead(itemReference.RootFullPath);
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

        #region Item Reference
        public string GetId<T>(T item) where T : class, new()
        {
            if (!_trackReferences)
                if (_failSilently)
                    return null;
                else throw new InvalidDatabaseOperationException("References not tracked. Check contructor parameters.");

            string id;
            if (TryGetId(item, out id))
                return id;

            if (_failSilently)
                return null;
            else throw new InvalidDatabaseOperationException("Item Id not found.");
        }

        public bool TryGetId<T>(T item, out string id) where T : class, new()
        {
            var reference = _references.Keys.FirstOrDefault(wr => wr.Target == item);

            id = reference == null ? null : _references[reference].Id;

            return !string.IsNullOrEmpty(id);
        }

        public ItemReference GetReference<T>(T item) where T : class, new()
        {
            if (!_trackReferences)
                if (_failSilently)
                    return ItemReference.Emtpy;
                else throw new InvalidDatabaseOperationException("References not tracked. Check contructor parameters.");

            ItemReference reference;
            if (TryGetReference(item, out reference))
                return reference;

            if (_failSilently)
                return ItemReference.Emtpy;
            else throw new InvalidDatabaseOperationException("Item reference not found.");
        }

        public bool TryGetReference<T>(T item, out ItemReference itemReference) where T : class, new()
        {
            var reference = _references.Keys.FirstOrDefault(wr => wr.Target == item);

            itemReference = reference == null ? ItemReference.Emtpy : _references[reference];

            return !itemReference.IsEmpty;
        } 
        #endregion
        #endregion

        #region Private methods
        private static string NewId()
        {
            return Guid.NewGuid().ToString(_guidFormat);
        }

        private void AddReference<T>(T item, ItemReference itemReference)
        {
            if (_trackReferences)
                _references.Add(new WeakReference(item, false), itemReference);
        }

        private OperationResult GuaranteeFileSystemStructure(ItemReference reference)
        {
            try
            {
                if (!Directory.Exists(reference.RootCollectionPath))
                    Directory.CreateDirectory(reference.RootCollectionPath);

                return OperationResult.Successful;
            }
            catch (Exception ex)
            {
                return OperationResult.Failed(new DatabaseException("Error creating database folder", ex));
            }
        }

        private ItemOperationResult AddInt<T>(string id, T item, string collection, string database, Action<FileStream> process)
        {
            var itemReference = BuildItemReference(id, collection, database);
            if (File.Exists(itemReference.RootFullPath))
                return ItemOperationResult.Failed(new ItemAlreadyExistsException(itemReference.FullPath));

            var pathCreated = GuaranteeFileSystemStructure(itemReference);

            if (!pathCreated)
                return ItemOperationResult.Failed(pathCreated);

            try
            {
                using (var fileStream = File.OpenWrite(itemReference.RootFullPath))
                    process(fileStream);

                AddReference(item, itemReference);
                return ItemOperationResult.Sucessful(id);
            }
            catch (Exception ex)
            {
                return ItemOperationResult.Failed(new DatabaseException($"Faled to add stream '{itemReference.FullPath}'", ex));
            }
        }

        private async Task<T> ReadItem<T>(ItemReference itemReference)
        {
            using (var fileStream = File.OpenRead(itemReference.RootFullPath))
            {
                var bytesRead = new byte[fileStream.Length];
                await fileStream.ReadAsync(bytesRead, 0, (int)fileStream.Length);
                var json = _encoding.GetString(bytesRead);

                var result = Serializer.Deserialize<T>(json);
                AddReference(result, itemReference);
                return result;
            }
        }

        private ItemReference BuildItemReference(string id, string collection, string database)
        {
            return new ItemReference(
                _rootFolder,
                string.IsNullOrEmpty(database) ? _defaultDatabase : database,
                string.IsNullOrEmpty(collection) ? _defaultCollection : collection,
                id);
        }
        #endregion
    }
}
