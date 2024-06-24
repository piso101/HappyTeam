using HappyBackEnd.Models;

namespace HappyBackend.Models
{
    public class EmailAuthModel
    {
        public User User { get; set; }
        public Car Car { get; set; }
        public Order Order { get; set; }
    }
}
