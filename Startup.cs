using AlgoApp.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.IO;
using System.Text;

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
                //option.UseMySql(Configuration.GetConnectionString("DefaultConnection"))
            );

            services.AddAuthentication()
                .AddCookie(options => {
                    options.LoginPath = "/Index";
                    options.AccessDeniedPath = "/Index";
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,

                        ValidIssuer = Configuration["Jwt:Issuer"],
                        ValidAudience = Configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                    };
                });

            services.AddRazorPages()
                    .AddRazorPagesOptions(o =>
                    {
                        o.Conventions.AddPageRoute("/Admin/Questions/Index", "/Admin/Chapters/{chapterid}/Questions");
                        o.Conventions.AddPageRoute("/Admin/Questions/Edit", "/Admin/Chapters/{chapterid}/Questions/{id}");
                    });

            services.AddMvc()
                    .AddJsonOptions(options => options.JsonSerializerOptions.IgnoreNullValues = true);
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
