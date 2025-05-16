using System.Net.Mail;
using System.Net;

namespace Coin_Exchange.Configuration
{
    public class OtpUntil
    {
        public static string GenerateOtp()
        {
            int otpLength = 6;
            Random random = new Random();

            System.Text.StringBuilder otp = new System.Text.StringBuilder();

            for (int i = 1; i <= otpLength; i++)
            {
                otp.Append(random.Next(10));  
            }

            return otp.ToString();
        }

        public static void SendVerificationOtpEmail(string email, string otp)
        {
            try
            {
                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("vtn12032004@gmail.com", "woovbieozzojwwpe"),
                    EnableSsl = true,
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress("vtn12032004@gmail.com"),
                    Subject = "Verify Otp",
                    Body = $"Your verification code is {otp}",
                    IsBodyHtml = true,
                };

                mailMessage.To.Add(email);

                smtpClient.Send(mailMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
                throw new Exception("Error sending email", ex);
            }
        }
        public static long GenerateLongIdFromTicks()
        {
            return DateTime.UtcNow.Ticks;
        }
    }
}
