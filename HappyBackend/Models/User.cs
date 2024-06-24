using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace HappyBackEnd.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public DateTime DateOfCreationOfToken { get; set; }
    }
}
