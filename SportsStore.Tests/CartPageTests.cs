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

			// - create a mock page context and session
			Mock<ISession> mockSession = new Mock<ISession>();
			byte[] data =
				Encoding.UTF8.GetBytes(JsonSerializer.Serialize(cartModel));
			mockSession.Setup(c => c.TryGetValue(It.IsAny<string>(), out data));
			Mock<HttpContext> mockContext = new Mock<HttpContext>();
			mockContext.SetupGet(c => c.Session).Returns(mockSession.Object);

			// Action
			Cart cart = new Cart(mockRepo.Object)
			{
				PageContext = new PageContext(new ActionContext
				{
					HttpContext = mockContext.Object,
					RouteData = new Microsoft.AspNetCore.Routing.RouteData(),
					ActionDescriptor = new PageActionDescriptor()
				})
			};
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

			// - create a mock page context and session
			Mock<ISession> mockSession = new Mock<ISession>();
			mockSession.Setup(s => s.Set(It.IsAny<string>(), It.IsAny<byte[]>()))
				.Callback<string, byte[]>((key, val) =>
				{
					cartModel = JsonSerializer.Deserialize<CartModel>(Encoding.UTF8.GetString(val));
				});

			Mock<HttpContext> mockContext = new Mock<HttpContext>();
			mockContext.SetupGet(c => c.Session).Returns(mockSession.Object);

			// Action
			Cart cart = new Cart(mockRepo.Object)
			{
				PageContext = new PageContext(new ActionContext
				{
					HttpContext = mockContext.Object,
					RouteData = new Microsoft.AspNetCore.Routing.RouteData(),
					ActionDescriptor = new PageActionDescriptor()
				})
			};
			cart.OnPost(1, "myUrl");
			cart.OnPost(2, "myUrl");

			// Assert
			Assert.Single(cartModel.Lines);
			Assert.Equal("P2", cartModel.Lines.First().Product.Name);
			Assert.Equal(1, cartModel.Lines.First().Quantity);
		}
	}
}
