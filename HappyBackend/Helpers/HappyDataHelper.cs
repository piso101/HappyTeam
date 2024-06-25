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

        /// <summary>
        /// Get all cars in one unit from database
        /// </summary>
        /// <param name="dateBegin">Date of beginning of the rental</param>
        /// <param name="dateEnd">Date of end of the rental</param>
        /// <param name="UnitName">Name of the unit</param>
        /// <returns>
        /// List of all cars in one unit
        /// </returns>
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

        /// <summary>
        /// Checks if the user is in the database
        /// </summary>
        /// <param name="user"></param>
        /// <returns>
        /// true if the user is in the database
        /// false if the user is not in the database
        /// </returns>
        public static bool CheckIfUserIsInDataBase(User user)
        {
            return HappyTeslaRepository.Users.Exists(u => u.Email == user.Email);
        }

        /// <summary>
        /// Checks if the car is in the database
        /// </summary>
        /// <param name="car"></param>
        /// <returns>
        /// true if the car is in the database
        /// false if the car is not in the database
        /// </returns>
        public static bool CheckIfCarIsInDataBase(Car car)
        {
            return HappyTeslaRepository.Cars.Contains(car);
        }

        /// <summary>
        /// Purge unverified data
        /// </summary>
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

        /// <summary>
        /// Purge old data
        /// </summary>
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

        /// <summary>
        /// Check if the token is Genuine
        /// </summary>
        /// <param name="keyToCheck">key to check</param>
        /// <returns>
        /// true if the token is genuine
        /// false if the token is not genuine
        /// </returns>
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
