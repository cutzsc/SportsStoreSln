using System;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using SportsStore.Models;

namespace SportsStore.Components
{
	public class NavigationMenuViewComponent : ViewComponent
	{
		private IStoreRepository storeRepository;

		public NavigationMenuViewComponent(IStoreRepository storeRepository) =>
			this.storeRepository = storeRepository;

		public IViewComponentResult Invoke()
		{
			ViewBag.SelectedCategory = RouteData?.Values["category"];
			return View(storeRepository.Products
				.Select(p => p.Category)
				.Distinct()
				.OrderBy(p => p));
		}
	}
}
