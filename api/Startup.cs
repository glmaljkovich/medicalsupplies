using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.FileProviders;
using AutoMapper;
using MySql.Data.EntityFrameworkCore.Infrastructure;
using ArqNetCore.Configuration;
using ArqNetCore.Services;
using NSwag.AspNetCore;
using Serilog;
using Serilog.Events;

namespace ArqNetCore
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:3000")
                               .AllowAnyHeader()
                               .AllowAnyMethod();
                    });
            });
            services.AddAutoMapper(typeof(Startup));
            string DB_URL = Environment.GetEnvironmentVariable("DB_URL");
            if(DB_URL == null || string.IsNullOrWhiteSpace(DB_URL))
            {
                DB_URL = "localhost";
            }
            string server = DB_URL;
            string DB_PORT = Environment.GetEnvironmentVariable("DB_PORT");
            if(DB_PORT == null || string.IsNullOrWhiteSpace(DB_PORT))
            {
                DB_PORT = "3306";
            }
            string port = DB_PORT;
            string DB_NAME = Environment.GetEnvironmentVariable("DB_NAME");
            if(DB_NAME == null || string.IsNullOrWhiteSpace(DB_NAME))
            {
                throw new Exception("DB_NAME required");
            }
            string database = DB_NAME;
            string DB_USERNAME = Environment.GetEnvironmentVariable("DB_USERNAME");
            if(DB_USERNAME == null)
            {
                throw new Exception("DB_USERNAME required");
            }
            string user = DB_USERNAME;
            string DB_PASSWORD = Environment.GetEnvironmentVariable("DB_PASSWORD");
            if(DB_PASSWORD == null)
            {
                DB_PASSWORD = "";
            }
            string password = DB_PASSWORD;
            Console.WriteLine("probando console" + DB_NAME);
            services.AddDbContext<ArqNetCoreDbContext>(
                (DbContextOptionsBuilder options) => 
                {
                    options.UseMySQL(
                        $"server={server};port={port};database={database};user={user};password={password}", 
                        (MySQLDbContextOptionsBuilder builder) => 
                        {
                            builder.ExecutionStrategy(context => {
                                return new ArqNetDbExecutionStrategy(context);
                        });
                });
            });
            services.AddHttpClient();
            services.AddControllers();
            services.AddSwaggerDocument();
            services.AddHttpContextAccessor();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ISuppliesOrderService, SuppliesOrderService>();
            services.AddScoped<IOrganizationService, OrganizationService>();
            
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            // configure jwt authentication
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer((JwtBearerOptions jwtBearerOptions) =>
            {
                jwtBearerOptions.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context =>
                    {
                        //it is not necesary due token should be signed 
                        // var userService = context.HttpContext.RequestServices.GetRequiredService<IUserService>();
                        // var email = context.Principal.Identity.Name;
                        // var user = userService.GetByEmail(email);
                        // if (user == null)
                        // {
                        //     // return unauthorized if user no longer exists
                        //     // TODO: improve log description 
                        //     context.Fail("Unauthorized");
                        // }
                        return Task.CompletedTask;
                    }
                };
                jwtBearerOptions.RequireHttpsMetadata = false;
                jwtBearerOptions.SaveToken = true;
                jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseSerilogRequestLogging(); 

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            // This one needs to be called before Authorization happens
            app.UseCors();
            app.UseAuthorization();
            app.UseOpenApi();
            app.UseSwaggerUi3((SwaggerUi3Settings settings) =>
            {
                //settings.DocumentPath = "/api.yaml";
                settings.SwaggerRoutes.Add(new SwaggerUi3Route("v1", "/open-api/api.yaml"));
                //settings.SwaggerUiRoute = "/swagger";
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
