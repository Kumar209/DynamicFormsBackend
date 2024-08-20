
using DynamicFormsBackend.Models.Entities;
using DynamicFormsBackend.Repository.Authentication;
using DynamicFormsBackend.Repository.FormCreation;
using DynamicFormsBackend.Repository.Response;
using DynamicFormsBackend.RepositoryInterface;
using DynamicFormsBackend.RepositoryInterface.Authentication;
using DynamicFormsBackend.RepositoryInterface.FormCreation;
using DynamicFormsBackend.RepositoryInterface.Response;
using DynamicFormsBackend.Service;
using DynamicFormsBackend.Service.Authentication;
using DynamicFormsBackend.Service.FormCreation;
using DynamicFormsBackend.Service.Response;
using DynamicFormsBackend.ServiceInterface;
using DynamicFormsBackend.ServiceInterface.Authentication;
using DynamicFormsBackend.ServiceInterface.FormCreation;
using DynamicFormsBackend.ServiceInterface.Response;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NLog;
using NLog.Web;
using Swashbuckle.AspNetCore.Filters;
using System.Text;

namespace DynamicFormsBackend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Early init of NLog to allow startup and exception logging, before host is built
            var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
            logger.Debug("init main");

            try
            {
                var builder = WebApplication.CreateBuilder(args);

                // NLog: Setup NLog for Dependency injection
                builder.Logging.ClearProviders();
                builder.Host.UseNLog();



                // Add services to the container.

                builder.Services.AddControllers();
                // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen();

                //Add authentication to Swagger UI
                builder.Services.AddSwaggerGen(options =>
                {
                    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                    {
                        In = ParameterLocation.Header,
                        Name = "Authorization",
                        Type = SecuritySchemeType.ApiKey
                    });

                    options.OperationFilter<SecurityRequirementsOperationFilter>();
                });

                builder.Services.AddCors(options =>
                {
                    options.AddPolicy("MyAllowSpecificOrigins",
                                          policy =>
                                          {
                                              policy.WithOrigins("http://localhost:4200")
                                                               .AllowAnyHeader()
                                                               .AllowAnyMethod();
                                          });
                });



                //For Jwt
                builder.Services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                }).AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidAudience = builder.Configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                    };
                });


                builder.Services.AddDbContext<ApplicationDbContext>(options =>
               options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


                builder.Services.AddTransient<IJwtService, JwtService>();

                builder.Services.AddTransient<IAuthRepository, AuthRepository>();
                builder.Services.AddTransient<IAuthService, AuthService>();


                builder.Services.AddTransient<IFormService, FormService>();
                builder.Services.AddTransient<IFormRepository, FormRepository>();


                builder.Services.AddTransient<IQuestionService, QuestionService>();
                builder.Services.AddTransient<IQuestionRepository,  QuestionRepository>();


                builder.Services.AddTransient<IFormResponseService, FormResponseService>();
                builder.Services.AddTransient<IFormResponseRepository, FormResponseRepository>();




                var app = builder.Build();

                // Configure the HTTP request pipeline.
                if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }

                app.UseHttpsRedirection();

                app.UseCors("MyAllowSpecificOrigins");

                app.UseAuthentication();

                app.UseAuthorization();


                app.MapControllers();

                app.Run();
            }
            catch (Exception exception)
            {
                // NLog: catch setup errors
                logger.Error(exception, "Stopped program because of exception");
                throw;
            }
            finally
            {
                // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
                NLog.LogManager.Shutdown();
            }
        }
    }
}
