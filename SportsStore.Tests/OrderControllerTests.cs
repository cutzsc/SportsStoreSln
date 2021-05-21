using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using SportsStore.Models;
using Moq;
using SportsStore.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace SportsStore.Tests
{
	public class OrderControllerTests
	{
		[Fact]
		public void Cannot_Checkout_Empty_Cart()
		{
			// Arrange - create a mock repository
			Mock<IOrderRepository> mock = new Mock<IOrderRepository>();
			// - create an empty cart
			CartModel cart = new CartModel();
			// - create the order
			Order order = new Order();
			// - create an instance of the controller
			OrderController controller = new OrderController(mock.Object, cart);

			// Act
			ViewResult result = controller.Checkout(order) as ViewResult;

			// Assert - check that the order hasn't been stored
			mock.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Never);
			// - check that the method is returning the default view
			Assert.True(string.IsNullOrEmpty(result.ViewName));
			// - check that I am passing an invalid model to the view
			Assert.False(result.ViewData.ModelState.IsValid);
		}

		[Fact]
		public void Cannot_Checkout_Invalid_ShippingDetails()
		{
			// Arrange - create a mock order repository
			Mock<IOrderRepository> mock = new Mock<IOrderRepository>();
			// - create a cart with one item
			CartModel cart = new CartModel();
			cart.AddItem(new Product(), 1);
			// - create an instance of the controller
			OrderController controller = new OrderController(mock.Object, cart);
			// - add an error to the model
			controller.ModelState.AddModelError("error", "error");

			// Act - try to checkout
			ViewResult result = controller.Checkout(new Order()) as ViewResult;

			// Assert - check that the order hasn't been passed stored
			mock.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Never);
			// - check that the method is returning the default view
			Assert.True(string.IsNullOrEmpty(result.ViewName));
			// - check that I am passing an invalid model to the view
			Assert.False(result.ViewData.ModelState.IsValid);
		}

		[Fact]
		public void Can_Checkout_And_Submit_Order()
		{
			// Arrange - create a mock order repository
			Mock<IOrderRepository> mock = new Mock<IOrderRepository>();
			// - create a cart with one item
			CartModel cart = new CartModel();
			cart.AddItem(new Product(), 1);
			// - create an instance of the controller
			OrderController target = new OrderController(mock.Object, cart);

			// Act - try to checkout
			RedirectToPageResult result =
				target.Checkout(new Order()) as RedirectToPageResult;

			// Assert - check that the order has been stored
			mock.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Once);
			// - check that the method is redirecting to the Completed action
			Assert.Equal("/Completed", result.PageName);
		}
	}
}
