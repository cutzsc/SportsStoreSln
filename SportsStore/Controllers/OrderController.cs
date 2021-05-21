using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Controllers
{
	public class OrderController : Controller
	{
		IOrderRepository repository;
		CartModel cart;

		public OrderController(IOrderRepository repository, CartModel cart) =>
			(this.repository, this.cart) = (repository, cart);

		public IActionResult Checkout()
		{
			return View(new Order());
		}

		[HttpPost]
		public IActionResult Checkout(Order order)
		{
			if (cart.Lines.Count == 0)
			{
				ModelState.AddModelError("", "Sorry, your cart is empty!");
			}
			if (ModelState.IsValid)
			{
				order.Lines = cart.Lines.ToArray();
				repository.SaveOrder(order);
				cart.Clear();
				return RedirectToPage("/Completed", new { orderId = order.OrderId });
			}
			else
				return View();
		}
	}
}
