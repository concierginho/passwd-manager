using System;
using AutoMapper;
using inz_int.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
