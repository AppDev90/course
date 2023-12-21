using Course.Core.Common;
using Course.Core.Services;
using Course.Core.Services.Interfaces;
using Course.Core.Utility.Convertors;
using Course.DataLayer.Context;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;


namespace Course
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddMvc(option => option.EnableEndpointRouting = false);
            services.AddControllersWithViews();

            services.AddRazorPages();

            #region Authentication

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;

            }).AddCookie(options =>
            {
                options.LoginPath = ConstValues.LoginPath;
                options.LogoutPath = ConstValues.LogoutPath;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(43200);

            });

            #endregion

            #region DataBase Context

            services.AddDbContext<CourseDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("CourseConnection"));
            }
            );

            #endregion

            #region Ioc

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IViewRenderService, RenderViewToString>();
            services.AddScoped<IWalletService, WalletService>();
            services.AddScoped<IRoleService, RoleService>();

            #endregion

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseAuthorization();
            
            ///atribute routing
            //app.UseMvc();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(

                    name: "MyAreas",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );
                endpoints.MapControllerRoute(

                    name: "Default",
                    pattern: "{controller=Home}/{action=Index}/{id?}"
                );

                endpoints.MapRazorPages();
            });
        }
    }
}
