using AlgoApp.Data;
using JWT;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;

namespace AlgoApp
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
            var dbpath = Path.Combine(".", "app.db");
            services.AddDbContext<ApplicationDbContext>(option =>
                option.UseSqlite($"Data Source={dbpath}")
            );

            //services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            //    //.AddCookie(configureOptions => configureOptions.LoginPath = "/Login");
            //    .AddCookie();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtAuthenticationDefaults.AuthenticationScheme;
            }).AddJwt(options =>
                      {
                          // secrets
                          options.Keys = new[] { "GQDstcKsx0NHjPOuXOYg5MbeJ1XT0uFiwDVvVBrk" };

                          // force JwtDecoder to throw exception if JWT signature is invalid
                          options.VerifySignature = true;
                      });

            services.AddRazorPages();
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
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapAreaControllerRoute(
                    name: "Api",
                    areaName: "Api",
                    pattern: "api/{controller}/{action}");
            });
        }
    }
}
