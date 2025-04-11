using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using Microsoft.OpenApi.Models;
using PatientManagementAPI.Services;
using LiteDB;
using PatientAPI.Services;
namespace PatientAPI
{
    public static class Registrations
    {
        public static WebApplicationBuilder WithMockSecurityProfile(this WebApplicationBuilder appBuilder)
        {
            appBuilder.Services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuerSigningKey = true,//If I did false for this too then can am not doing any validation
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("codegetthekeyfromconfiginsteadofhardcodinghere")),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                    };
                });

            appBuilder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Patient  API", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }});
            });

            return appBuilder;

        }

        public static WebApplicationBuilder WithServices(this WebApplicationBuilder appBuilder)
        {
            appBuilder.Services.AddSingleton<JwtService>();
            appBuilder.Services.AddSingleton<ILiteDatabase>(new LiteDatabase("Filename=:memory:"));
            appBuilder.Services.AddSingleton<DatabaseService>();

            return appBuilder;
        }
    }
}
