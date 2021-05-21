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

		public Cart(IStoreRepository repository, CartModel cartService) =>
			(this.repository, CartModel) = (repository, cartService);

		public void OnGet(string returnUrl)
		{
			ReturnUrl = returnUrl ?? "/";
		}

		public IActionResult OnPost(long productId, string returnUrl)
		{
			Product product = repository.Products.FirstOrDefault(p => p.ProductId == productId);

			if (product != null)
			{
				CartModel.AddItem(product, 1);
			}

			return RedirectToPage(new { returnUrl = returnUrl });
		}

		public IActionResult OnPostRemove(long productId, string returnUrl)
		{
			CartModel.RemoveLine(CartModel.Lines.First(p => p.Product.ProductId == productId).Product);
			return RedirectToPage(new { returnUrl = returnUrl });
		}
	}
}
