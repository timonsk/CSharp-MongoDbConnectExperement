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
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }

            var props = obj.GetType().GetProperties();

            if (!props.Any())
            {
                throw new EmptyClassPropertyException("No public property found in obj");
            }

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
    }
}