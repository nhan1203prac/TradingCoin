using Coin_Exchange.Configuration;
using Coin_Exchange.Models.Enum;
using Coin_Exchange.Models.Modal;
using Coin_Exchange.Models.Response;
using Coin_Exchange.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Coin_Exchange.Models.Request;

namespace Coin_Exchange.Controllers
{
    [Route("api/")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpGet("users/profile")]
        public async Task<IActionResult> getUserProfile([FromHeader(Name = "Authorization")] string jwt)
        {
            string email = JwtProviders.GetEmailFromToken(jwt);
            User user = await _context.Users.FirstOrDefaultAsync(u => u.email == email);
            if (user == null)
            {
                return BadRequest(new { Message = "OTP not found" });
            }


            return Ok(user);
        }


        [HttpPost("user/enable-two-factor/send-otp")]
        public async Task<IActionResult> sendOtpEnable2FA([FromHeader(Name = "Authorization")] string jwt)
        {
            string email = JwtProviders.GetEmailFromToken(jwt);
            User user = await _context.Users.FirstOrDefaultAsync(u => u.email == email);
            if (user == null)
            {
                return BadRequest(new { Message = "User not found" });
            }
            AuthResponse authResponse = new AuthResponse
            {
                jwt = jwt


            };
            TwoFactorOtp twoFactorOtp = await _context.TwoFactorOtps.FirstOrDefaultAsync(u => u.user.id == user.id);
            if (twoFactorOtp != null)
            {
                _context.TwoFactorOtps.Remove(twoFactorOtp);
                await _context.SaveChangesAsync();
            }
            string otp = OtpUntil.GenerateOtp();
            Guid guid = Guid.NewGuid();
            string id = guid.ToString();
            TwoFactorOtp newTwoFactor = new TwoFactorOtp { id = id, otp = otp, user = user, jwt = jwt };
            _context.TwoFactorOtps.Add(newTwoFactor);
            await _context.SaveChangesAsync();
            authResponse.session = newTwoFactor.id;
            authResponse.message = "Verification otp sent successfully";
            OtpUntil.SendVerificationOtpEmail(email, otp);


            return Ok(authResponse);
        }


        [HttpPost("user/enable-two-factor/verify/{otp}")]
        public async Task<ActionResult<ApiResponse>> verifyOtp(string otp, [FromQuery] string id)
        {
            TwoFactorOtp twoFactorOtp = await _context.TwoFactorOtps.FirstOrDefaultAsync(u => u.id == id);
            Boolean verify = twoFactorOtp.otp == otp;
            if (verify)
            {
                User user = await _context.Users.FirstOrDefaultAsync(u => u.id == twoFactorOtp.user.id);
                if (!user.IsEnable2FA)
                {
                    user.IsEnable2FA = true;
                }
                else
                {
                    user.IsEnable2FA = false;
                }
                await _context.SaveChangesAsync();


                return Ok(new ApiResponse { message = "Change status success", status = true });
            }

            return BadRequest(new ApiResponse { message = "Invalid otp", status = false });

        }

        [HttpPatch("user/update-password")]
        public async Task<ActionResult<AuthResponse>> updatePassword([FromBody] NewPasswordRequest newPasswordRequest)
        {

            User user = await _context.Users.FirstOrDefaultAsync(u => u.email == newPasswordRequest.email);
            user.password = newPasswordRequest.password;
            await _context.SaveChangesAsync();


            var roles = new List<string> { USER_ROLE.ROLE_CUSTOMER.ToString() };
            var token = JwtProviders.GenerateToken(user.email, roles);
            return Ok(new AuthResponse { isTwoFatorAuthEnabled = user.IsEnable2FA, jwt = token, message = "Login success" });

        }


    }
}
