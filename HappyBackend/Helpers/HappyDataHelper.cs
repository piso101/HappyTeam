using HappyBackEnd.Models;
using HappyBackEnd.Repository;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;

namespace HappyBackend.Helpers
{
    public static class HappyDataHelper
    {
        public static List<Car> GetAvailableCars(string UnitName, DateTime dateBegin, DateTime dateEnd)
        {
            
            var unit = HappyTeslaRepository.Units.Find(unit => unit.LocationName == UnitName);
            if (unit == null)
            {
                
                return new List<Car>();
            }

            var UnitId = unit.Id;

            var unavailableCarIds = HappyTeslaRepository.Orders
                .Where(order => order.StartUnit == UnitName && order.IsVerified)
                .Select(order => order.CarId)
                .ToList();

            
            return HappyTeslaRepository.Cars
                .Where(car => car.UnitId == UnitId && !unavailableCarIds.Contains(car.Id))
                .ToList();
        }
        public static bool CheckIfUserIsInDataBase(User user)
        {
            return HappyTeslaRepository.Users.Exists(u => u.Email == user.Email);
        }
        public static bool CheckIfCarIsInDataBase(Car car)
        {
            return HappyTeslaRepository.Cars.Contains(car);
        }
        public static void PurgeUnverifiedData()
        {
            var unverifiedOrders = HappyTeslaRepository.Orders
                .Where(order => !order.IsVerified && order.StartDate < DateTime.Today)
                .ToList(); 

            foreach (var order in unverifiedOrders)
            {
                HappyTeslaRepository.Orders.Remove(order);
            }
        }
        public static void PurgeOldData()
        {
            var ordersToRemove = HappyTeslaRepository.Orders
                .Where(order => order.StartDate < DateTime.Today.AddMonths(-2))
                .ToList(); 

            foreach (var order in ordersToRemove)
            {
                HappyTeslaRepository.Orders.Remove(order);
            }
        }
        public static bool IsAuthorized(string keyToCheck)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var keyBytes = Encoding.ASCII.GetBytes(HappyTeslaRepository.Key());

            try
            {
                tokenHandler.ValidateToken(keyToCheck, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
