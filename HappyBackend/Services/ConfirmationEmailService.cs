using HappyBackEnd.Models;
using MimeKit;
using MailKit.Net.Smtp;
using System.Linq.Expressions;
using HappyBackEnd.Repository;
namespace HappyBackend.Services
{
    public static class ConfirmationEmailService
    {
        /// <summary>
        /// Sends a confirmation email to the user's email address
        /// </summary>
        /// <param name="user">user object to get information about client</param>
        /// <param name="car">car object to get information about car client is trying to rent</param>
        /// <param name="order">order object to get information about date's and unit's involved</param>
        public static void SendConfirmationEmail(User user, Car car,Order order)
        {
            var confirmationLink = $"http://localhost:3000/Verify?token={user.Token}&carId={car.Id}";
            try
            {
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse("oskarkacala43@gmail.com"));
                email.To.Add(MailboxAddress.Parse(user.Email));
                email.Subject = "Potwierdzenie Emaila";
                email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                {
                    Text = $@"
                    <html>
                    <head>
                        <style>
                            body {{
                                font-family: Arial, sans-serif;
                                background-color: #f4f4f4;
                                
                                margin: 0;
                                padding: 0;
                            }}
                            .container {{
                                max-width: 600px;
                                margin: 50px auto;
                                background-color: #ffffff;
                                padding: 20px;
                                box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
                            }}
                            .header {{
                                text-align: center;
                                padding: 10px 0;
                            }}
                            .header h1 {{
                                color: #007BFF;
                            }}
                            .content {{
                                line-height: 1.6;
                                padding: 20px 0;
                            }}
                            .content p {{
                                margin: 10px 0;
                            }}
                            .button {{
                                display: block;
                                width: 200px;
                                margin: 20px auto;
                                padding: 10px;
                                text-align: center;
                                background-color: #007BFF;
                                color: white;
                                text-decoration: none;
                                text-color: white !important;
                                border-radius: 5px;
                            }}
                            .footer {{
                                text-align: center;
                                padding: 10px 0;
                                font-size: 12px;
                                color: #777777;
                            }}
                        </style>
                    </head>
                    <body>
                        <div class='container'>
                            <div class='header'>
                                <h1>Potwierdzenie Emaila</h1>
                            </div>
                            <div class='content'>
                                
                                <p>Witaj {user.Name} {user.Surname} !</p>
                                <p>Wybrałeś do wynajmu: {car.Name}!</p>
                                <p>Od dnia:{order.StartDate.ToString("yyyy-MM-dd")} do dnia: {order.EndDate.ToString("yyyy-MM-dd")} </p>
                                <p>Twój wynajem zacznie się w placówce {order.StartUnit} </p>
                                <p>Twój wynajem zakończy się w placówce {order.EndUnit} </p>
                                <p>Całkowity koszt: {order.TotalPrice} $</p>       
                                
                                <p> Kliknij w poniższy przycisk by potwierdzić swoje zamówienie:</p>
                                <a href='{confirmationLink}' class='button' >Potwierdź E-mail</a>
                                <p>Jeśli link nie działa, skopiuj i wklej poniższy adres URL do przeglądarki:</p>
                                <p>{confirmationLink}</p>
                            </div>
                            <div class='footer'>
                                <p>© 2024 Oskar Kacała. Wszelkie prawa zastrzeżone.</p>
                            </div>
                        </div>
                    </body>
                    </html>"

                };

                using (var smtp = new SmtpClient())
                {
                    smtp.Connect("smtp.gmail.com", 465, MailKit.Security.SecureSocketOptions.SslOnConnect);
                    smtp.Authenticate("oskarkacala43@gmail.com", "knbipevbwpyiwgqj");
                    smtp.Send(email);
                    smtp.Disconnect(true);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex);

            }
        }
    }
}
