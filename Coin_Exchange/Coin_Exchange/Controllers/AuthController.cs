using Coin_Exchange.Configuration;
using Coin_Exchange.Models;
using Coin_Exchange.Models.Enum;
using Coin_Exchange.Models.Modal;

using Coin_Exchange.Models.Response;
using Coin_Exchange.Models.Request;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Coin_Exchange.Controllers
{

    [Route("auth/")]
    [ApiController]
    public class AuthController:ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AuthController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        [HttpPost("signin")]
        public async Task<IActionResult> Login([FromBody] Models.Request.LoginRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.email == request.email);
            if (user == null || user.password != request.password)
            {
                return Unauthorized(new { Message = "Invalid credentials" });
            }

            var roles = new List<string> { USER_ROLE.ROLE_CUSTOMER.ToString() };

            var token = JwtProviders.GenerateToken(request.email, roles);
            AuthResponse authResponse = new AuthResponse
            {
                jwt = token,
                message = "Login success",
               

            };
            if (user.IsEnable2FA)
            {
                string otp = OtpUntil.GenerateOtp();
                authResponse.isTwoFatorAuthEnabled = true;



                TwoFactorOtp oldTwoFactor = await _context.TwoFactorOtps.FirstOrDefaultAsync(u => u.user.id == user.id);

                if (oldTwoFactor != null)
                {
                    _context.TwoFactorOtps.Remove(oldTwoFactor);
                }
                Guid guid = Guid.NewGuid();
                string id = guid.ToString();
                TwoFactorOtp newTwoFactor = new TwoFactorOtp { id = id, otp = otp, user = user, jwt = token };
                _context.TwoFactorOtps.Add(newTwoFactor);
                await _context.SaveChangesAsync();
                authResponse.session = newTwoFactor.id;

                OtpUntil.SendVerificationOtpEmail(request.email, otp);
               
            }




            return Ok(authResponse);
        }

        [HttpPost("signup")]
        public async Task<IActionResult> Signup([FromBody] User request)
        {

            var user = await _context.Users.FirstOrDefaultAsync(u => u.email == request.email);
            if (user != null)
            {

                return BadRequest(new { Message = "Email already exists. Please use a different email." });
            }

            var newUser = new User
            {
                fullName = request.fullName,
                email = request.email,
                password = request.password,
            };
            

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            var watchlist = new Watchlist
            {
                user = newUser
            };
            _context.Watchlists.Add(watchlist);
            await _context.SaveChangesAsync();


            var wallet = new Wallet
            {
                user = newUser

            };
            _context.Wallets.Add(wallet);
            await _context.SaveChangesAsync();

            var roles = new List<string> { USER_ROLE.ROLE_CUSTOMER.ToString() };
            var token = JwtProviders.GenerateToken(newUser.email, roles);

            AuthResponse authResponse = new AuthResponse
            {
                jwt = token,
                message = "Register success",
                

            };

            return Ok(authResponse);
        }


        [HttpPost("two-factor/otp/{otp}")]
        public async Task<ActionResult<AuthResponse>> VerifySigningOtp(string otp, [FromQuery] string id)
        {
            var twoFactorOtp = await _context.TwoFactorOtps.FirstOrDefaultAsync(otp => otp.id == id);
            if (twoFactorOtp == null)
            {
                return NotFound(new { Message = "OTP not found" });
            }

            bool isValidOtp = twoFactorOtp.otp == otp;
            if (isValidOtp)
            {
                var response = new AuthResponse
                {
                    message = "Two factor authentication verified",
                    jwt = twoFactorOtp.jwt,
                    isTwoFatorAuthEnabled = true
                };
                return Ok(response);
            }

            return BadRequest(new { Message = "Invalid otp" });
        }




        [HttpPost("reset-password/send-otp")]
        public async Task<ActionResult<AuthResponse>> SendOtp([FromBody] SendOtpRequest req)
        {
            if (string.IsNullOrWhiteSpace(req?.sendTo))
            {
                return BadRequest(new AuthResponse { message = "Email is required" });
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.email == req.sendTo);
            if (user == null)
            {
                return NotFound(new AuthResponse { message = "User not found" });
            }

            string otp = OtpUntil.GenerateOtp();
            string id = Guid.NewGuid().ToString();

            var token = await _context.ForgotPasswordTokens.FirstOrDefaultAsync(u => u.user.id == user.id);
            if (token == null)
            {
                token = new ForgotPasswordToken
                {
                    id = id,
                    otp = otp,
                    user = user,
                    sendTo = req.sendTo
                };
                _context.ForgotPasswordTokens.Add(token);
            }
            else
            {
                token.otp = otp;
                token.sendTo = req.sendTo;
                _context.ForgotPasswordTokens.Update(token);
            }

            await _context.SaveChangesAsync();

            try
            {
                OtpUntil.SendVerificationOtpEmail(req.sendTo, otp);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, new AuthResponse { message = "Failed to send OTP email" });
            }

            return Ok(new AuthResponse
            {
                message = "Password reset OTP sent successfully",
                session = token.id
            });
        }


        [HttpPost("reset-password/verify-otp/{otp}")]
        public async Task<ActionResult<AuthResponse>> verifyOtp(string otp, [FromQuery] string id)
        {
            ForgotPasswordToken forgotPasswordToken = _context.ForgotPasswordTokens.FirstOrDefault(u => u.id == id);
            Boolean verify = forgotPasswordToken.otp == otp;
            if (verify)
            {
                return Ok(new ApiResponse { message = "Verify otp success", status = true });
            }

            return BadRequest(new ApiResponse { message = "Invalid otp", status = false });

        }
    }
}
