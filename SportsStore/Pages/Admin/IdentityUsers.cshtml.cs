using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SportsStore.Pages.Admin
{
	[Authorize]
	public class IdentityUsersModel : PageModel
	{
		private UserManager<IdentityUser> userManager;

		public IdentityUsersModel(UserManager<IdentityUser> userManager) =>
			this.userManager = userManager;

		public IdentityUser AdminUser { get; set; }

		public async Task OnGetAsync()
		{
			AdminUser = await userManager.FindByNameAsync("Admin");
		}
	}
}
