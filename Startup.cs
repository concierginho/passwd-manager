using System;
using System.Text;
using AutoMapper;
using inz_int.Authentication;
using inz_int.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace inz_int
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public string _userApiDbPasswd = null;
        public string _passwordApiDbPasswd = null;

        public void ConfigureServices(IServiceCollection services)
        {
            _userApiDbPasswd = Configuration["UserAPI:Passwd"];
            var userConnectionString = String.Concat(Configuration["ConnectionStrings:UserConnection"], "Password=", _userApiDbPasswd, ";");
            _passwordApiDbPasswd = Configuration["PasswordAPI:Passwd"];
            var passwordConnectionString = String.Concat(Configuration["ConnectionStrings:PasswordConnection"], "Password=", _passwordApiDbPasswd, ";");

            services.AddDbContext<UserContext>(opt => opt.UseSqlServer(userConnectionString));
            services.AddDbContext<PasswdContext>(opt => opt.UseSqlServer(passwordConnectionString));

            services.AddControllers();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddScoped<IUserRepository, SqlUserRepository>();
            services.AddScoped<IPasswdRepository, SqlPasswdRepository>();
            services.AddScoped<IJwtAuthenticationManager, JwtAuthenticationManager>();
            services.AddScoped<ValidUsersContext, ValidUsersContext>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                    {
                        options.RequireHttpsMetadata = false;
                        options.SaveToken = true;
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            ValidIssuer = Configuration["Jwt:Issuer"],
                            ValidAudience = Configuration["Jwt:Audience"],
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:SecretKey"])),
                            ClockSkew = TimeSpan.Zero
                        };
                    });

            services.AddAuthorization(config =>
            {
                config.AddPolicy(Policies.Admin, Policies.AdminPolicy());
                config.AddPolicy(Policies.User, Policies.UserPolicy());
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "inz_int", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "inz_int v1"));
            }

            app.UseHttpsRedirection();

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
