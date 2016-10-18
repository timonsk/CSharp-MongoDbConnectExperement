using System;
using System.Linq;
using MongoConnector.Exceptions;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MongoConnector
{
    public class Connector
    {
        protected static IMongoClient Client;
        protected static IMongoDatabase Database;
        protected static DocumentCreator DocumentCreator;

        public Connector(string dbName, string connectionString)
        {
            Client = new MongoClient(connectionString);
            Database = Client.GetDatabase(dbName);
            DocumentCreator = new DocumentCreator();
        }

        public async void Create<T>(T obj)
        {
            var collection = Database.GetCollection<BsonDocument>(obj.GetType().Name);
            await collection.InsertOneAsync(obj.ToBsonDocument());
        }

        public async void Update<T>(T obj)
        {
            var collection = Database.GetCollection<BsonDocument>(obj.GetType().Name);
            var id = obj.GetType().GetProperties().FirstOrDefault(p => p.Name.ToLower() == "id");
            if (id == null)
            {
                throw new EmptyClassPropertyException("id");
            }
            var filter = Builders<BsonDocument>.Filter.Eq("_" + id.Name.ToLower(), id.GetValue(obj));
            var document = (await collection.Find(filter).ToListAsync()).First();
            var updatedObj = obj.ToBsonDocument();
              updatedObj.Remove("_id");

            await collection.UpdateOneAsync(document, updatedObj);
        }

        private BsonDocument Update<T>(T obj, BsonDocument document)
        {
            document.Remove("_id");
            //ValidateObject(obj);
            //var props = obj.GetType().GetProperties().ToList();
            //var id = props.FirstOrDefault(p => p.Name.ToLower() == "id");

            //if (id != null)
            //{
            //    obj.GetType().
            //    props.Remove(id);
            //}

            //foreach (var prop in props)
            //{
            //    var val = prop.GetValue(obj);
            //    var name = prop.Name;
            //    var element = document.FirstOrDefault(e => e.Name == name);
            //    if (element.Value.ToJson() != val.ToJson())
            //    {
            //        document.Set(name, val);
            //    }
            //}

            return document;
        }

        private void ValidateObject<T>(T obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }

            var props = obj.GetType().GetProperties();

            if (!props.Any())
            {
                throw new EmptyClassPropertyException("No public property found in obj");
            }
        }
    }
}