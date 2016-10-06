using Infrastructure.Interfaces;
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


        public async void InsertProduct(IProduct product)
        {
            var products = DocumentCreator.Create(product);
            var collection = Database.GetCollection<BsonDocument>(product.GetType().Name);
            await collection.InsertOneAsync(products);
        }
    }
}