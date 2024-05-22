using ECommerce.Application.Repositories;
using ECommerce.Domain.Entities.Concretes;
using ECommerce.Domain.ViewModels.CustomerViewModels;
using ECommerce.Domain.ViewModels.OrderViewModels;
using ECommerce.Domain.ViewModels.ProductViewModels;
using ECommerce.Persistence.DbContexts;
using ECommerce.Persistence.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Persistence.Repositories;

public class ReadCustomerRepository : ReadGenericRepository<Customer>, IReadCustomerRepository
{
	public ReadCustomerRepository(ECommerceDbContext context) : base(context)
	{
	}

	public async Task<ICollection<CustomerOrderProductVM>> GetCustomersWithOrdersWithProducts()
	{
		var customers = await _context.Customers.ToListAsync();
		List<CustomerOrderProductVM> customersVMs = customers.Select(c => new CustomerOrderProductVM
		{
			id = c.Id,
			FirstName = c.FirstName,
			LastName = c.LastName,
			Email = c.Email,
			Address = c.Address,
			Password = c.Password,
			Orders = c.Orders.Select(o => new OrderWithProductVM
			{
				Id = o.Id,
				OrderNote = o.OrderNote,
				OrderNumber = o.OrderNumber,
				OrderDate = o.OrderDate,
				Products = o.Products.Select(p => new ProductVM()
				{
					ID = p.Id,
					Name = p.Name,
					Description = p.Description,
					Price = p.Price,
					ImageUrl = p.ImageUrl,
					Stock = p.Stock,
				}).ToList()
			}).ToList()
		}).ToList();

		return customersVMs;
	}



}
