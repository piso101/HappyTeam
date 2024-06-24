using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace HappyBackEnd.Models
{
    public class Order
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal TotalPrice { get; set; }

        public string CarId { get; set; }
        public string UserId { get; set; }
        public string StartUnit { get; set; }
        public string EndUnit { get; set; }
        public bool IsVerified { get; set; }
    }
}
