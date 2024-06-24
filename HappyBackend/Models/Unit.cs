using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace HappyBackEnd.Models
{
    public class Unit
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string LocationName { get; set; }
       
    }

}
