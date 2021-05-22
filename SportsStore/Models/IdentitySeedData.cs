using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace SportsStore.Models
{
	public static class IdentitySeedData
	{
		private const string adminName = "Admin";
		private const string adminPassword = "my54defenSE@2";

		public static async void EnsurePopulated(IApplicationBuilder app)
		{
			AppIdentityDbContext context = app.ApplicationServices
				.CreateScope().ServiceProvider
				.GetRequiredService<AppIdentityDbContext>();

			if (context.Database.GetPendingMigrations().Any())
			{
				context.Database.Migrate();
			}

			UserManager<IdentityUser> userManager = app.ApplicationServices
				.CreateScope().ServiceProvider
				.GetRequiredService<UserManager<IdentityUser>>();

			IdentityUser user = await userManager.FindByIdAsync(adminName);
			if (user == null)
			{
				user = new IdentityUser(adminName);
				user.Email = "admin@gmail.com";
				user.PhoneNumber = "1234-555";
				await userManager.CreateAsync(user, adminPassword);
			}
		}
	}
}
