using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;
using ForumAPI.Services;
using ForumAPI.Services.Interfaces;
using ForumAPI.Repositories.Models;
using ForumAPI.Repositories.Interfaces;
using ForumAPI.Repositories;
using ForumAPI.Data;
using ForumAPI.DTOs.Mapping;

namespace ForumAPI
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
            string? connectionString = Configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<ForumAPIContext>(options =>
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
            services.AddScoped<IRepositoryBase<Category>, CategoriesRepository>();
            services.AddScoped<IRepositoryBase<Section>, SectionsRepository>();
            services.AddScoped<IRepositoryBase<Subject>, SubjectsRepository>();
            services.AddScoped<IRepositoryBase<Message>, MessagesRepository>();
            services.AddScoped<IServiceBase<Category>, CategoriesService>();
            services.AddScoped<IServiceBase<Section>, SectionsService>();
            services.AddScoped<IServiceBase<Subject>, SubjectsService>();
            services.AddScoped<IServiceBase<Message>, MessagesService>();
            services.AddControllers();

            services.AddIdentity<User, IdentityRole<int>>()
                .AddEntityFrameworkStores<ForumAPIContext>()
                .AddDefaultTokenProviders();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                var secretKey = Configuration["JWT:SecretKey"];
                if (secretKey == null)
                {
                    throw new Exception("Secret key is not set");
                }
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = Configuration["JWT:ValidIssuer"],
                    ValidateAudience = true,
                    ValidAudience = Configuration["JWT:ValidAudience"],
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                    ValidateIssuerSigningKey = true,
                };
            });
            services.AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
            });
            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            });
            services.AddAutoMapper(typeof(MappingProfile));
            // Add any required services here, such as authentication, database context, etc.
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error");
                app.UseHsts();
            }

            // app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
