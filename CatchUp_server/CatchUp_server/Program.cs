using CatchUp_server.Db;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CatchUp_server.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Net;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using CatchUp_server.Models.UserModels;
using CatchUp_server.Services.AuthServices;
using CatchUp_server.Services.UserServices;
using CatchUp_server.Services.FriendsServices;
using CatchUp_server.Services.UserContentServices;

namespace CatchUp_server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddScoped<AuthService>();
            builder.Services.AddScoped<UserService>();
            builder.Services.AddScoped<EmailService>();
            builder.Services.AddScoped<FriendsService>();
            builder.Services.AddScoped<UserProfileService>();
            builder.Services.AddScoped<MediaFoldersService>();
            builder.Services.AddScoped<PostService>();
            builder.Services.AddScoped<LikeService>();
            builder.Services.AddScoped<CommentService>();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            // options added 
            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("oauth2", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey
                });
                options.OperationFilter<SecurityRequirementsOperationFilter>();
            });
            


            builder.Services.AddDbContext<ApiDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("CatchUpConnection")));

            builder.Services.AddIdentity<User, IdentityRole>(options =>
            {
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 8;
            }).AddEntityFrameworkStores<ApiDbContext>().AddDefaultTokenProviders();

            // add authorization
            //builder.Services.AddAuthorization();

            // added above
            /*builder.Services.AddIdentityApiEndpoints<User>(options =>
            {
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 8;
            }).AddEntityFrameworkStores<ApiDbContext>();*/

            // will need something like this
            //builder.Services.AddAuthentication().AddJwtBearer();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
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



            var app = builder.Build();
            app.UseCors(c => c.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // map identity
            //app.MapIdentityApi<User>();

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseAuthorization();

            // probaby needing this
            //app.UseAuthentication();

            app.MapControllers();

            app.Run();
        }
    }
}
