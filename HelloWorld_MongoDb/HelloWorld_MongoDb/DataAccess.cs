using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Driver;

namespace HelloWorld_MongoDb
{
    public class DataAccess
    {
        /*
            MongoClient _client;
            private IMongoDatabase _db;

            public DataAccess()
            {
                _client = new MongoClient("mongodb://localhost:27017");
                
                _db = _client.GetDatabase("testDB");
            }

            public IEnumerable<User> GetUsers()
            {
                return _db.GetCollection<User>("Users");
            }


            public User GetUser(ObjectId id)
            {
                var res = Query<User>.EQ(p => p.Id, id);
                return _db.GetCollection<User>("Users").FindOne(res);
            }

            public User CreateUser(User user)
            {
                _db.GetCollection<User>("Users").Save(user);
                return user;
            }

            public bool UpdateUser(ObjectId id, User user)
            {
                user.Id = id;
                var res = Query<User>.EQ(u => u.Id, id);
                var operation = Update<User>.Replace(user);
                var result = _db.GetCollection<User>("Users").Update(res, operation);
                return !result.HasLastErrorMessage;
            }

            public bool RemoveUser(ObjectId id)
            {
                var res = Query<User>.EQ(e => e.Id, id);
                var operation = _db.GetCollection<User>("Users").Remove(res);
                return !operation.HasLastErrorMessage;
            }
            */
    }
}