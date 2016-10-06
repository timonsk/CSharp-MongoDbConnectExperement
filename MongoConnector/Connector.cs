using System.ComponentModel;
using System.Linq;
using Infrastructure.Interfaces;
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
	        //var products = DocumentCreator.Create(obj);
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
	        var filter = Builders<BsonDocument>.Filter.Eq("_"+id.Name.ToLower(), id.GetValue(obj));
	        var result = await collection.Find(filter).ToListAsync();

	        var document = result.First();

	        //var product = DocumentCreator.Update(obj, document);
	         await collection.UpdateOneAsync(new BsonDocumentFilterDefinition<BsonDocument>(document), obj.ToBsonDocument());
	    }


	    public async void InsertProduct(IProduct product)
		{
			var products = DocumentCreator.Create(product);
			var collection = Database.GetCollection<BsonDocument>(product.GetType().Name);
			await collection.InsertOneAsync(products);
		}
	}
}