
using ECommerce.Application.Repositories;
using ECommerce.Domain.Entities.Concretes;
using ECommerce.Persistence.DbContexts;
using ECommerce.Persistence.Repositories.Common;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ECommerce.Persistence.Repositories;

public class ReadOrderRepository : ReadGenericRepository<Order>, IReadOrderRepository
{
    public ReadOrderRepository(ECommerceDbContext context) : base(context)
    {
    }


	public async Task<IEnumerable<Order>> GetAllIncludingAsync(params Expression<Func<Order, object>>[] includeProperties)
	{
		IQueryable<Order> query = _context.Orders;
		foreach (var includeProperty in includeProperties)
		{
			query = query.Include(includeProperty);
		}
		return await query.ToListAsync();
	}
}
