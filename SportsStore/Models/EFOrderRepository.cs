using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Models
{
	public class EFOrderRepository : IOrderRepository
	{
		private StoreDbContext context;

		public IQueryable<Order> Orders => context.Orders
			.Include(o => o.Lines)
			.ThenInclude(l => l.Product);

		public EFOrderRepository(StoreDbContext context) =>
			this.context = context;

		public void SaveOrder(Order order)
		{
			context.AttachRange(order.Lines.Select(l => l.Product));
			if (order != null && order.OrderId == 0)
				context.Orders.Add(order);
			context.SaveChanges();
		}
	}
}
