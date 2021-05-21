using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Models
{
	public class EFStoreRepository : IStoreRepository
	{
		private StoreDbContext context;

		public EFStoreRepository(StoreDbContext context) =>
			this.context = context;

		public IQueryable<Product> Products => context.Products;
	}
}
