using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SportsStore.Models;
using Microsoft.EntityFrameworkCore;

namespace SportsStore
{
	public class Startup
	{
		IConfiguration Configuration { get; set; }

		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllersWithViews();
			services.AddDbContext<StoreDbContext>(options =>
			{
				options.UseSqlServer(
					Configuration["ConnectionStrings:SportsStoreConnection"]);
			});
			services.AddScoped<IStoreRepository, EFStoryRepository>();
			services.AddRazorPages();
			services.AddDistributedMemoryCache();
			services.AddSession();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseStatusCodePages();
			app.UseStaticFiles();
			app.UseSession();

			app.UseRouting();
			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute("catpage",
					"{category}/Page{productPage}",
					new { Controller = "Home", action = "Index" });

				endpoints.MapControllerRoute("page",
					"Page{productPage}",
					new { Controller = "Home", action = "Index", productPage = 1 });

				endpoints.MapControllerRoute("category",
					"{category}",
					new { Controller = "Home", action = "Index", productPage = 1 });

				endpoints.MapControllerRoute("pagination",
					"Products/Page{productPage}",
					new { Controller = "Home", action = "Index", productPage = 1 });

				endpoints.MapDefaultControllerRoute();
				endpoints.MapRazorPages();
			});

			SeedData.EnsurePopulated(app);
		}
	}
}
