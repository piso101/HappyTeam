using HappyBackend.Helpers;
using HappyBackend.Models;
using HappyBackend.Services;
using HappyBackEnd.Models;
using HappyBackEnd.Repository;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.DataProtection.KeyManagement;


namespace HappyBackend.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    public class HappyController : ControllerBase
    {
        /// <summary>
        /// Get all Units
        /// </summary>
        /// <returns>
        /// List of all Units in database
        /// </returns>
        [HttpGet("Units")]
        public IEnumerable<Unit> GetUnits()
        {
            HappyTeslaRepository.LoadData();
            return HappyTeslaRepository.Units;
        }

        /// <summary>
        /// Get all Cars in one Unit from database
        /// </summary>
        /// <param name="dateBegin">Date of beginning of the rental</param>
        /// <param name="dateEnd">Date of end of the rental</param>
        /// <param name="UnitName">Name of the Unit</param>
        /// <returns>
        /// List of all Cars in one Unit
        /// </returns>
        [HttpGet("GetAvailableCars")]
        public IEnumerable<Car> GetAvailableCars(string UnitName, DateTime dateBegin, DateTime dateEnd)
        {
            HappyTeslaRepository.LoadData();
            return HappyDataHelper.GetAvailableCars(UnitName, dateBegin, dateEnd);
        }

        /// <summary>
        /// Send an email to the user with a verification link
        /// </summary>
        /// <param name="request">EmailAuthModel object</param>
        /// <returns>
        /// OK if the email was sent successfully
        /// BadRequest if the car was not found in the database
        /// </returns>
        [HttpPost("EmailVer")]
        public IActionResult SendEmail([FromBody] EmailAuthModel request)
        {
            HappyTeslaRepository.LoadData();
            if (HappyDataHelper.CheckIfCarIsInDataBase(request.Car)) return BadRequest();

            request.User.Token = Guid.NewGuid().ToString();

            request.User.Id = "";
            request.Order.Id = "";
            request.Order.EndDate = request.Order.EndDate.AddDays(1);
            request.Order.StartDate = request.Order.StartDate.AddDays(1);

            ConfirmationEmailService.SendConfirmationEmail(request.User, request.Car, request.Order);

            if (!HappyDataHelper.CheckIfUserIsInDataBase(request.User)) HappyTeslaRepository.Users.Add(request.User);
            else
            {
                HappyTeslaRepository.Users.Find(u => u.Email == request.User.Email).Token = request.User.Token;
                HappyTeslaRepository.Users.Find(u => u.Email == request.User.Email).DateOfCreationOfToken = request.User.DateOfCreationOfToken;
                    }
            request.Order.IsVerified = false;
            request.Order.UserId = HappyTeslaRepository.Users.Find(u => u.Email == request.User.Email).Id;
            HappyTeslaRepository.Orders.Add(request.Order);
            HappyTeslaRepository.ModifyData();

            return Ok();
        }

        /// <summary>
        /// Verify user's Order by token and carId
        /// </summary> 
        /// <param name="carId" >Id of the car</param>
        /// <param name="token">Token of the user</param>
        /// <returns>
        /// Ok if the order was verified
        /// BadRequest if the token is empty or the user is not found in the database
        /// BadRequest if the token is invalid or expired
        /// BadRequest if the order is already verified
        /// BadRequest if the order is not found in the database
        /// </returns>
        [HttpGet("Verify")]
        public IActionResult Verify(string token, string carId)
        {
            HappyTeslaRepository.LoadData();

            if (string.IsNullOrEmpty(token))
            {
                return BadRequest(new { success = false });
            }


            var user = HappyTeslaRepository.Users.Find(u => u.Token == token);

            if (user == null || user.DateOfCreationOfToken.AddHours(1) > DateTime.Now)
            {
                return BadRequest(new { success = false });
            }

            var order = HappyTeslaRepository.Orders.Find(o => o.CarId == carId && o.UserId == user.Id);
            if (order != null && !order.IsVerified)
            {
                order.IsVerified = true;
                HappyTeslaRepository.ModifyData();
                return Ok(new { success = true });
            }

            return BadRequest(new { success = false });
        }

        // Admin panel
        /// <summary>
        /// Login for Admin panel
        /// Creates a jwt token for the Admin
        /// </summary>
        /// <param name="login">Login for Admin</param>
        /// <param name="password">Password for Admin</param>
        /// <returns>
        /// Unauthorized if the login or password is incorrect
        /// Ok and token if the login and password are correct
        /// </returns>
        [HttpGet("AdminLogin")]
        public IActionResult AdminLogin(string login, string password)
        {

            if (login == HappyTeslaRepository.keyValuePair().Key && password == HappyTeslaRepository.keyValuePair().Value)
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(HappyTeslaRepository.Key());
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                    new Claim(ClaimTypes.Name, login)
                    }),
                    Expires = DateTime.UtcNow.AddHours(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);

                return Ok(new { Token = tokenString });
            }

            return Unauthorized();
        }

        /// <summary>
        /// Cleans unverified data from the database
        /// </summary>
        /// <param name="token">Token aquired while logging in, for Admin</param>
        /// <returns>
        /// Unauthorized if the token is invalid
        /// Ok if the data was cleaned successfully and token is valid
        /// </returns>
        [HttpGet("CleanUnVerified")]
        public IActionResult CleanUnVerifiedData(string token)
        {
            HappyTeslaRepository.LoadData();
            if (!HappyDataHelper.IsAuthorized(token)) return Unauthorized();

            HappyDataHelper.PurgeUnverifiedData();
            return Ok();

        }

        /// <summary>
        /// Cleanold data from the database
        /// </summary>
        /// <param name="token">Token aquired while logging in, for Admin</param>
        /// <returns>
        /// Unauthorized if the token is invalid
        /// Ok if the data was cleaned successfully and token is valid
        /// </returns>
        [HttpGet("CleanOldData")]
        public IActionResult CleanOldData(string token)
        {
            HappyTeslaRepository.LoadData();
            if (!HappyDataHelper.IsAuthorized(token)) return Unauthorized();

            HappyDataHelper.PurgeOldData();
            return Ok();
        }

        /// <summary>
        /// Clean car data from order if returned.
        /// </summary>
        /// <example>
        /// Car has returned to the unit, worker scans the car and the order is removed from the database.
        /// </example>
        /// <param name="CarId">Id of the car</param>
        /// <param name="token">Token aquired while logging in, for Admin</param>
        /// <returns>
        /// Unauthorized if the token is invalid
        /// Ok if the data was cleaned successfully and token is valid
        /// BadRequest if the car is not found in the database
        /// </returns>
        [HttpGet("CarReturned")]
        public IActionResult CarReturned(string token, string CarId)
        {
            HappyTeslaRepository.LoadData();
            if (!HappyDataHelper.IsAuthorized(token)) return Unauthorized();

            var order = HappyTeslaRepository.Orders.Find(o => o.CarId == CarId && o.IsVerified);
            if (order != null)
            {
                HappyTeslaRepository.Orders.Remove(order);
                HappyTeslaRepository.Cars.Find(car => car.Id == CarId).UnitId = HappyTeslaRepository.Units.Find(unit => unit.LocationName == order.EndUnit).Id;
                HappyTeslaRepository.ModifyData();
                return Ok();
            }
            return BadRequest();
        }
    }
}
