using System;
using ETicaret.Business.Base;
using ETicaret.Business.Manager;
using ETicaret.Business.Services;
using ETicaret.DataAccess.Base;
using ETicaret.Repositories.Implement.EfCore;
using ETicaret.Repository.Base;
using ETicaret.Repository.Implement.EfCore;
using ETicaret.WebUI.EmailServices;
using ETicaret.WebUI.Identity;
using ETicaret.WebUI.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ETicaret.WebUI
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            //ApplicationIdentityDbContext Config
            services.AddDbContext<ApplicationIdentityDbContext>(options =>
             options.UseSqlServer(Configuration.GetConnectionString("IdentityConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationIdentityDbContext>()
                .AddDefaultTokenProviders();

            //ApplicationUser Yapılandırma
            services.Configure <IdentityOptions>(options =>
            {
                //Password Yapılandırma
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 6;

                //Lockout Yapılandırma
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(60);
                options.Lockout.AllowedForNewUsers = true;

                //User Yapılandırma
                options.User.RequireUniqueEmail = false;

                //SignIn Yapılandırma
                options.SignIn.RequireConfirmedEmail = true;
                options.SignIn.RequireConfirmedPhoneNumber = false;
            });

            //Cookie Yapılandırma
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/account/login";
                options.LogoutPath = "/account/logout";
                options.AccessDeniedPath = "/account/accessdenied";
                options.SlidingExpiration = true;
                options.Cookie = new CookieBuilder
                {
                    HttpOnly = true,
                    Name=".ETicaret.Security.Cookie",
                    SameSite=SameSiteMode.Strict
                };


            });

            //Servislerin bağımlılıklarının inject işlemi
            services.AddScoped<IProductRepository, EfCoreProductRepository>();
            services.AddScoped<IProductService, ProductManager>();

            services.AddScoped<ICategoryRepository, EfCoreCategoryRepository>();
            services.AddScoped<ICategoryService, CategoryManager>();

            services.AddScoped<ICartRepository, EfCoreCartRepository>();
            services.AddScoped<ICartService, CartManager>();

            services.AddScoped<IOrderRepository, EfCoreOrderRepository>();
            services.AddScoped<IOrderService, OrderManager>();

            services.AddTransient<IEmailSender, EmailSender>();

            //Otomatik route özelliğinin false yapılması
            //Aşağıda routeları manuel düzelledik
            services.AddMvc(options => options.EnableEndpointRouting = false).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
        }

        private int IdentityOptions(Action<object> p)
        {
            throw new NotImplementedException();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                SeedDatabase.Seed();
            }

            //Middlewares Yapılandırma
            app.UseStaticFiles();
            app.CustomStaticFiles();

            //Token Üretiminin Aktifleştirilmesi
            app.UseAuthentication();

            //Routeların Yapılandırılması (MVC actived)
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                   name: "adminProducts",
                   template: "admin/products",
                   defaults: new { controller = "Admin", action = "ProductList" }
                 );
                routes.MapRoute(
                   name: "adminProductsEdit",
                   template: "admin/products/{id}",
                   defaults: new { controller = "Admin", action = "EditProduct" }
                 );
                routes.MapRoute(
                   name: "cart",
                   template: "cart",
                   defaults: new { controller = "Cart", action = "Index" }
               );
                routes.MapRoute(
                    name: "orders",
                    template: "orders",
                    defaults: new { controller = "Cart", action = "GetOrders" }
                );
                routes.MapRoute(
                   name: "checkout",
                   template: "checkout",
                   defaults: new { controller = "Cart", action = "Checkout" }
               );
                routes.MapRoute(
                   name: "products",
                   template: "products/{category?}",
                   defaults: new { controller = "Shop", action = "List" }
                 );
                routes.MapRoute(
                   name: "default",
                   template: "{controller=Home}/{action=Index}/{id?}"
                 );
            });

            SeedIdentity.Seed(userManager, roleManager, Configuration).Wait();
        }
    }
}
