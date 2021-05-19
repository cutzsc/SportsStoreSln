using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Xunit;
using SportsStore.Controllers;
using SportsStore.Models;
using SportsStore.Infrastructure;
using SportsStore.Models.ViewModels;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;
using SportsStore.Components;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SportsStore.Tests
{
	public class NavigationMenuViewComponentTests
	{
		[Fact]
		public void Can_Select_Categories()
		{
			// Arrange
			Mock<IStoreRepository> mock = new Mock<IStoreRepository>();
			mock.Setup(m => m.Products).Returns((new Product[] {
				new Product {ProductId = 1, Name = "P1", Category = "Apples"},
				new Product {ProductId = 2, Name = "P2", Category = "Apples"},
				new Product {ProductId = 3, Name = "P3", Category = "Plums"},
				new Product {ProductId = 4, Name = "P4", Category = "Oranges"}
			}).AsQueryable);

			NavigationMenuViewComponent target = new NavigationMenuViewComponent(mock.Object);

			// Act
			string[] result = ((target.Invoke() as ViewViewComponentResult).ViewData.Model as IEnumerable<string>).ToArray();

			// Assert
			Assert.True(Enumerable.SequenceEqual(new string[] { "Apples", "Oranges", "Plums" }, result));
		}

		[Fact]
		public void Indicates_Selected_Category()
		{
			// Arrange
			string categoryToSelect = "Apples";
			Mock<IStoreRepository> mock = new Mock<IStoreRepository>();
			mock.Setup(m => m.Products).Returns((new Product[] {
				new Product {ProductId = 1, Name = "P1", Category = "Apples"},
				new Product {ProductId = 2, Name = "P2", Category = "Apples"},
				new Product {ProductId = 3, Name = "P3", Category = "Plums"},
				new Product {ProductId = 4, Name = "P4", Category = "Oranges"}
			}).AsQueryable);

			NavigationMenuViewComponent target = new NavigationMenuViewComponent(mock.Object);
			target.ViewComponentContext = new ViewComponentContext
			{
				ViewContext = new ViewContext
				{
					RouteData = new Microsoft.AspNetCore.Routing.RouteData()
				}
			};
			target.RouteData.Values["category"] = categoryToSelect;

			// Act
			string result = (string)((ViewViewComponentResult)target.Invoke()).ViewData["SelectedCategory"];

			// Assert
			Assert.Equal(categoryToSelect, result);
		}
	}
}
