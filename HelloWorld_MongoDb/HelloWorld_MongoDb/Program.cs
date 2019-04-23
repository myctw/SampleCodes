using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using HelloWorld_MongoDb.Model;
using MongoDB;
using MongoDB.Driver;

namespace HelloWorld_MongoDb
{
    class Program
    {
        private const string MongoAddress = "mongodb://localhost:27017";
        private const string HelloDatabase = "hellomongodb";
        private const string UserCollection = "users";
        
        static void Main(string[] args)
        {
            //Create one
            CreateUser();

            //Create many
            CreateMultiUsers();

            //Get all  
            var queryAllUsersResult = GetAllUsers();
            queryAllUsersResult.Result.ForEach((x=>Console.WriteLine($"{x.Name} lives in {x.Country}.{x.City}")));

            //Get one
            var queryUserResult = GetUser("Sion");

            Console.WriteLine(
                queryUserResult.Result.Any()
                    ? $"Found: {queryUserResult.Result.First().Name} in {queryUserResult.Result.First().City}"
                    : $"Unable to find Sion");

            //Update one
            Console.WriteLine(
                UpdateCity("Sion", "Live World").Result
                ? $"Update success. New city of Sion is : {GetUser("Sion").Result.First().City}"
                : $"Update FAIL!");
            
            //Delete one
            if (DeleteOne("Jack").Result)
            {
                var afterDeleteOneResult = GetAllUsers();
                afterDeleteOneResult.Result.ForEach((x=>Console.WriteLine($"{x.Name} lives in {x.Country}.{x.City}")));
            }

            
            //Delete all
            if (DeleteAll().Result)
            {
                var afterDeleteAll = GetAllUsers();
                Console.WriteLine($"After delete all users, the count of collection is : {afterDeleteAll.Result.Count}");
            }

            Console.ReadLine();
        }

        private static IMongoCollection<UserEntity> GetUsersCollection()
        {
            var _client = new MongoClient(MongoAddress);
            var _collection = _client.GetDatabase(HelloDatabase).GetCollection<UserEntity>(UserCollection);
            return _collection;
        }
        private static async void CreateUser()
        {
            await GetUsersCollection().InsertOneAsync(
                    new UserEntity{ Name = "Shaka", Country = "Unknown", City = "Zodiac Temple"}
            );
        }
        private static async void CreateMultiUsers()
        {
            await GetUsersCollection().InsertManyAsync(new List<UserEntity>
                {
                    new UserEntity{ Name = "Jack", Country = "Taiwan", City = "Taipei"},
                    new UserEntity{ Name = "Sion", Country = "Unknown", City = "Underworld"},
                    new UserEntity{ Name = "Saga", Country = "Unknown", City = "Zodiac Temple"}
                }
            );
        }

        private static async Task<List<UserEntity>> GetAllUsers()
        {
            var result = await GetUsersCollection().FindAsync(FilterDefinition<UserEntity>.Empty).Result.ToListAsync();
            return result;
        }
        
        private static async Task<List<UserEntity>> GetUser(string name)
        {
            var filter = Builders<UserEntity>.Filter.Eq("name", name);
            var result = await GetUsersCollection().FindAsync(filter).Result.ToListAsync();
            return result;
        }
        private static async Task<bool> UpdateCity(string name, string newCity)
        {
            var filter = Builders<UserEntity>.Filter.Eq("name", name);
            var update = Builders<UserEntity>.Update.Set("city", newCity);
            var result = await GetUsersCollection().UpdateOneAsync(filter, update);
            return result.IsAcknowledged;
        }

        private static async Task<bool> DeleteAll()
        {
            var result = await GetUsersCollection().DeleteManyAsync(FilterDefinition<UserEntity>.Empty);
            return result.IsAcknowledged;
        }
        private static async Task<bool> DeleteOne(string name)
        {
            var filter = Builders<UserEntity>.Filter.Eq("name", name);
            var result = await GetUsersCollection().DeleteOneAsync(filter);
            return result.IsAcknowledged;
        }
    }
}