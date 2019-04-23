using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace HelloWorld_MongoDb.Model
{
    public class UserEntity
    {
        [BsonId(IdGenerator=typeof(StringObjectIdGenerator))]
        public string Id { get; set; }

        [BsonElement("name")] 
        public string Name { get; set; }

        [BsonElement("city")]
        public string City { get; set; }
        
        [BsonElement("country")]
        public string Country { get; set; }

    }
}