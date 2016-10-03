using System;
using Infrastructure.Extensions;
using Infrastructure.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MongoConnector
{
    public class Connector
    {
        private const string MongoConnectionString = @"connection_string_here";
        protected static IMongoClient Client;
        protected static IMongoDatabase Database;

        public Connector()
        {
            Client = new MongoClient(MongoConnectionString);
            Database = Client.GetDatabase("yandb");
        }


        public async void InsertProduct(IProduct product)
        {
            var products = new BsonDocument
            {
                { ObjectName.GetPropertyName(() => product.Id), product.Id},
                { ObjectName.GetPropertyName(() => product.Name), product.Name},
                { ObjectName.GetPropertyName(() => product.Category), product.Category},
                { ObjectName.GetPropertyName(() => product.Price), product.Price.ToString()},
            };

            var collection = Database.GetCollection<BsonDocument>("products");
            await collection.InsertOneAsync(products);
        }
    }
}