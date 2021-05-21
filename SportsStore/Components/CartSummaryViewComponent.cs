using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;

namespace SportsStore.Components
{
	public class CartSummaryViewComponent : ViewComponent
	{
		CartModel cart;

		public CartSummaryViewComponent(CartModel cartService) =>
			this.cart = cartService;

		public IViewComponentResult Invoke()
		{
			return View(cart);
		}
	}
}
