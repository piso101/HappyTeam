
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace HappyBackEnd.Models

{
    public class Car
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public decimal PricePerDay { get; set; }
        public string UnitId { get; set; }
    }
}
