using System;
using System.Linq;
using MongoConnector.Exceptions;
using MongoDB.Bson;

namespace MongoConnector
{
	public class DocumentCreator
	{
		public BsonDocument Create<T>(T obj)
		{
			ValidateObject(obj);
			var props = obj.GetType().GetProperties();
			var document = new BsonDocument();

			foreach (var prop in props)
			{
				var val = prop.GetValue(obj);
				var name = prop.Name;
				var be = new BsonElement(name, val.ToString());
				document.Add(be);
			}

			return document;
		}

		public BsonDocument Update<T>(T obj, BsonDocument document)
		{
			ValidateObject(obj);
			var props = obj.GetType().GetProperties();
			foreach (var prop in props)
			{
				var val = prop.GetValue(obj);
				var name = prop.Name;
				var element = document.FirstOrDefault(e => e.Name == name);
			    if (element.Value.ToJson() != val.ToJson())
			    {
			        document.Set(name, val.ToJson());
			    }
			}

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