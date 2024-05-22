using ECommerce.Domain.Entities.Concretes;
using ECommerce.Domain.ViewModels.CustomerViewModels;

namespace ECommerce.Application.Repositories;

public interface IReadCustomerRepository : IReadGenericRepository<Customer>
{
	Task<ICollection<CustomerOrderProductVM>> GetCustomersWithOrdersWithProducts();
}
