using HappyBackEnd.Models;
using MongoDB.Driver;
using MongoDB.Bson;
using HappyBackend.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Runtime.CompilerServices;


namespace HappyBackEnd.Repository
{
    public static class HappyTeslaRepository
    {
        
        public static List<Car> Cars { get; set; }
        public static List<Unit> Units { get; set; }
        public static List<User> Users { get; set; }
        public static List<Order> Orders { get; set; }
        
        public static KeyValuePair<string,string> keyValuePair()
        {
            IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();
            KeyValuePair<string,string> temp = new KeyValuePair<string, string>(configuration.GetSection("AppSettings")["Login"], configuration.GetSection("AppSettings")["Password"]);
            return temp;
        }
        public static string Key ()
        {
            IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();
            string key = configuration.GetSection("AppSettings")["Token"];
            return key;
        }
        private static string _connectionUrl()
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();
            string Url = configuration.GetSection("AppSettings")["DataBaseUrl"];
            return Url;
        }


        public static void LoadData()
        {
            var settings = MongoClientSettings.FromConnectionString(_connectionUrl());
            
            var client = new MongoClient(settings);
            
            try
            {
                var db = client.GetDatabase("HappyDataBase");

                var collectionCars = db.GetCollection<Car>("Cars");
                Cars = collectionCars.Find(new BsonDocument()).ToList();

                var collectionUnits = db.GetCollection<Unit>("Units");
                Units = collectionUnits.Find(new BsonDocument()).ToList();

                var collectionUsers = db.GetCollection<User>("Users");
                Users = collectionUsers.Find(new BsonDocument()).ToList();

                var collectionOrders = db.GetCollection<Order>("Orders");
                Orders = collectionOrders.Find(new BsonDocument()).ToList();

            }
            catch (Exception ex)
            {
                Console.WriteLine("exeption: "+ex);
            }
        }


        public static void ModifyData()
        {
            var settings = MongoClientSettings.FromConnectionString(_connectionUrl());

            var client = new MongoClient(settings);

            try
            {
                var db = client.GetDatabase("HappyDataBase");

                var collectionCars = db.GetCollection<Car>("Cars");
                collectionCars.DeleteMany(new BsonDocument());
                collectionCars.InsertMany(Cars);

                var collectionUnits = db.GetCollection<Unit>("Units");
                collectionUnits.DeleteMany(new BsonDocument());
                collectionUnits.InsertMany(Units);

                var collectionUsers = db.GetCollection<User>("Users");
                collectionUsers.DeleteMany(new BsonDocument());
                collectionUsers.InsertMany(Users);

                var collectionOrders = db.GetCollection<Order>("Orders");
                collectionOrders.DeleteMany(new BsonDocument());
                collectionOrders.InsertMany(Orders);

            }
            catch (Exception ex)
            {
                Console.WriteLine("exeption: " + ex);
            }
        }
    }
}