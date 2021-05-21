using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Moq;
using SportsStore.Models;
using SportsStore.Pages;
using System.Linq;
using System.Text;
using System.Text.Json;
using Xunit;

namespace SportsStore.Tests
{
	public class CartPageTests
	{
		[Fact]
		public void Can_Load_Cart()
		{
			// Arrange
			// - create a mock repository
			Product p1 = new Product { ProductId = 1, Name = "P1" };
			Product p2 = new Product { ProductId = 2, Name = "P2" };
			Mock<IStoreRepository> mockRepo = new Mock<IStoreRepository>();
			mockRepo.Setup(m => m.Products).Returns((new Product[] { p1, p2 }).AsQueryable);

			// - create a cart
			CartModel cartModel = new CartModel();
			cartModel.AddItem(p1, 2);
			cartModel.AddItem(p2, 1);

			// Action
			Cart cart = new Cart(mockRepo.Object, cartModel);
			cart.OnGet("myUrl");

			//Assert
			Assert.Equal(2, cart.CartModel.Lines.Count());
			Assert.Equal("myUrl", cart.ReturnUrl);
		}

		[Fact]
		public void Can_Update_Cart()
		{
			// Arrange
			// - create a mock repository
			Product p1 = new Product { ProductId = 1, Name = "P1" };
			Product p2 = new Product { ProductId = 2, Name = "P2" };
			Mock<IStoreRepository> mockRepo = new Mock<IStoreRepository>();
			mockRepo.Setup(m => m.Products).Returns((new Product[] { p1, p2 }).AsQueryable);

			// - create a cart
			CartModel cartModel = new CartModel();
			cartModel.AddItem(p1, 2);
			cartModel.AddItem(p2, 1);

			// Action
			Cart cart = new Cart(mockRepo.Object, cartModel);
			cart.OnPost(1, "myUrl");
			cart.OnPost(2, "myUrl");

			// Assert
			Assert.Equal(2, cartModel.Lines.Count());
			Assert.Equal("P1", cartModel.Lines.First().Product.Name);
			Assert.Equal(3, cartModel.Lines.First().Quantity);
		}
	}
}
