using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TheBookShop.Infrastructure;
using TheBookShop.Models;
using TheBookShop.Models.DataModels;
using TheBookShop.Models.Repositories;

namespace TheBookShop
{
    public class Startup
    {
        public Startup(IConfiguration configuration) => Configuration = configuration;

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration["Data:TheBookShopProducts:ConnectionString"]));
            services.AddDbContext<AppIdentityDbContext>(options =>
                options.UseSqlServer(Configuration["Data:TheBookShopIdentity:ConnectionString"]));
            services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<AppIdentityDbContext>()
                .AddDefaultTokenProviders();
            services.AddTransient<IProductRepository, EFProductRepository>();
            services.AddScoped<Cart>(p => SessionCart.GetCart(p));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IOrderRepository, EFOrderRepository>();
            services.AddTransient<IAuthorRepository, EFAuthorRepository>();
            services.AddTransient <IPaymentRepository, EFPaymentRepository>();
            services.AddTransient<IDeliveryAdressRepository, EFDeliveryAddressRepository>();
            services.AddTransient<IDeliveryMethodRepository, EFDeliveryMethodRepository>();
            services.AddTransient<IPaymentMethodRepository, EFPaymentMethodRepository>();
            services.AddTransient<IOpinionRepository, EFOpinionRepository>();
            services.AddTransient<IProductCategoryRepository, EFProductCategoryRepository>();
            services.AddMvc();
            services.AddMemoryCache();
            services.AddSession();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseStatusCodePages();
            app.UseStaticFiles();
            app.UseSession();
            app.UseAuthentication();
            app.UseMvcWithDefaultRoute();
            //app.UseMvc(routes =>
            //{
            //    routes.MapRoute(
            //        name: null,
            //        template: "{category}/Page{productPage:int}",
            //        defaults: new { controller = "Product", action = "List" }
            //    );

            //    routes.MapRoute(
            //        name: null,
            //        template: "Page{productPage:int}",
            //        defaults: new { controller = "Product", action = "List", productPage = 1 }
            //    );

            //    routes.MapRoute(
            //        name: null,
            //        template: "{category}",
            //        defaults: new { controller = "Product", action = "List", productPage = 1 }
            //    );

            //    routes.MapRoute(
            //        name: null,
            //        template: "",
            //        defaults: new { controller = "Product", action = "List", productPage = 1 });

            //    routes.MapRoute(name: null, template: "{controller}/{action}/{id?}");
            //});

            AppIdentityDbContext.CreateAdminAccount(app.ApplicationServices, Configuration).Wait();
            //SeedData.EnsurePopulated(app);
        }
    }
}