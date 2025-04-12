using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using Microsoft.OpenApi.Models;
using LiteDB;
using PatientSystem.Services;
using PatientSystem.Services.Interfaces;
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
                        ValidateLifetime = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("codegetthekeyfromconfiginsteadofhardcodinghere")),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ClockSkew = TimeSpan.Zero //For some reason only when I add this it validated expire
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
            appBuilder.Services.AddSingleton<IJwtService, JwtService>();
            appBuilder.Services.AddSingleton<ILiteDatabase>(new LiteDatabase("Filename=lite-patientmanagement.db"));
            appBuilder.Services.AddSingleton<IPatientRepository, PatientRepository>();

            return appBuilder;
        }
    }
}
