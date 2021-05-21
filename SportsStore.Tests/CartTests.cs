using SportsStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace SportsStore.Tests
{
	public class CartTests
	{
		[Fact]
		public void Can_Add_New_Lines()
		{
			// Arrange
			Product p1 = new Product { ProductId = 1, Name = "P1" };
			Product p2 = new Product { ProductId = 2, Name = "P2" };

			CartModel cart = new CartModel();

			// Act
			cart.AddItem(p1, 1);
			cart.AddItem(p2, 1);

			CartLine[] results = cart.Lines.ToArray();

			// Assert
			Assert.Equal(2, results.Length);
			Assert.Equal(p1, results[0].Product);
			Assert.Equal(p2, results[1].Product);
		}

		[Fact]
		public void Can_Add_Quantity_For_Existing_Lines()
		{
			// Arrange
			Product p1 = new Product { ProductId = 1, Name = "P1" };
			Product p2 = new Product { ProductId = 2, Name = "P2" };

			CartModel cart = new CartModel();

			// Act
			cart.AddItem(p1, 1);
			cart.AddItem(p2, 1);
			cart.AddItem(p1, 10);

			CartLine[] results = cart.Lines.ToArray();

			// Assert
			Assert.Equal(2, results.Length);
			Assert.Equal(11, results[0].Quantity);
			Assert.Equal(1, results[1].Quantity);
		}

		[Fact]
		public void Can_Remove_Line()
		{
			// Arrange
			Product p1 = new Product { ProductId = 1, Name = "P1" };
			Product p2 = new Product { ProductId = 2, Name = "P2" };

			CartModel cart = new CartModel();

			cart.AddItem(p1, 1);
			cart.AddItem(p2, 1);
			cart.AddItem(p1, 10);

			// Act
			cart.RemoveLine(p2);

			// Assert
			Assert.Single(cart.Lines);
			Assert.Empty(cart.Lines.Where(line => line.Product.ProductId == p2.ProductId));
		}

		[Fact]
		public void Calculate_Cart_Total()
		{
			// Arrange
			Product p1 = new Product { ProductId = 1, Name = "P1", Price = 100m };
			Product p2 = new Product { ProductId = 2, Name = "P2", Price = 50m };

			CartModel cart = new CartModel();

			cart.AddItem(p1, 1);
			cart.AddItem(p2, 1);
			cart.AddItem(p1, 10);

			// Act
			decimal total = cart.ComputeTotalValue();

			// Assert
			Assert.Equal(1150, total);
		}

		[Fact]
		public void Can_Clear_Cart()
		{
			// Arrange
			Product p1 = new Product { ProductId = 1, Name = "P1", Price = 100m };
			Product p2 = new Product { ProductId = 2, Name = "P2", Price = 50m };

			CartModel cart = new CartModel();

			cart.AddItem(p1, 1);
			cart.AddItem(p2, 1);
			cart.AddItem(p1, 10);

			// Act
			cart.Clear();

			// Assert
			Assert.Empty(cart.Lines);
		}
	}
}
