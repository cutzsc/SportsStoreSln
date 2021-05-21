using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;
using SportsStore.Models;
using SportsStore.Infrastructure;

namespace SportsStore.Pages
{
	public class Cart : PageModel
	{
		private IStoreRepository repository;

		public CartModel CartModel { get; set; }
		public string ReturnUrl { get; set; }

		public Cart(IStoreRepository repository) =>
			this.repository = repository;

		public void OnGet(string returnUrl)
		{
			ReturnUrl = returnUrl ?? "/";
			CartModel = HttpContext.Session.GetJson<CartModel>("cart") ?? new CartModel();
		}

		public IActionResult OnPost(long productId, string returnUrl)
		{
			Product product = repository.Products.FirstOrDefault(p => p.ProductId == productId);

			if (product != null)
			{
				CartModel = HttpContext.Session.GetJson<CartModel>("cart") ?? new CartModel();
				CartModel.AddItem(product, 1);
				HttpContext.Session.SetJson("cart", CartModel);
			}

			return RedirectToPage(new { returnUrl = returnUrl });
		}
	}
}
