using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Coin_Exchange.Configuration
{
    public class AppConfig
    {
        public void ConfigureServices(IServiceCollection services)
        {
            // Cấu hình JWT Authentication
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = "Trading_coin",
                        ValidAudience = "Trader",
                        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("sgryuodhadfeyuoaskjcdyiepyicayulkl"))
                    };
                });

            // Cấu hình CORS
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                {
                    builder.WithOrigins("http://localhost:5173") 
                           .AllowAnyMethod()
                           .AllowAnyHeader()
                           .AllowCredentials(); 
                });
            });

            // Cấu hình Authorization
            services.AddAuthorization();
        }

        public void Configure(IApplicationBuilder app)
        {
            // Bật CORS
            app.UseCors("CorsPolicy");

            // Cấu hình routing và middleware xác thực
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            // Định nghĩa endpoint
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
