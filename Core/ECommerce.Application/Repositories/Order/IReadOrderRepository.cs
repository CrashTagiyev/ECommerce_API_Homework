using ECommerce.Domain.Entities.Concretes;
using System.Linq.Expressions;
namespace ECommerce.Application.Repositories;

public interface IReadOrderRepository : IReadGenericRepository<Order>
{
	public  Task<IEnumerable<Order>> GetAllIncludingAsync(params Expression<Func<Order, object>>[] includeProperties);
}
