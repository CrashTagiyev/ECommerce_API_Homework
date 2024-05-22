using ECommerce.Application.Repositories;
using ECommerce.Domain.Entities.Concretes;
using ECommerce.Domain.ViewModels;
using ECommerce.Domain.ViewModels.OrderViewModels;
using ECommerce.Domain.ViewModels.ProductViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace ECommerce.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrderController : ControllerBase
{
	private readonly IReadOrderRepository _readOrderRepository;
	private readonly IWriteOrderRepository _writeOrderRepository;
	private readonly IReadProductRepository _readProductRepository;
	private readonly IReadCustomerRepository _readCustomerRepository;
	private readonly IWriteCustomerRepository _writeCustomerRepository;

	public OrderController(IReadOrderRepository readOrderRepository, IWriteOrderRepository writeOrderRepository, IReadProductRepository readProductRepository, IReadCustomerRepository readCustomerRepository, IWriteCustomerRepository writeCustomerRepository = null)
	{
		_readOrderRepository = readOrderRepository;
		_writeOrderRepository = writeOrderRepository;
		_readProductRepository = readProductRepository;
		_readCustomerRepository = readCustomerRepository;
		_writeCustomerRepository = writeCustomerRepository;
	}


	[HttpPost("CreateOrder")]
	public async Task<IActionResult> CreateOrder([FromBody] OrderCreateVM orderCreateVM)
	{
		var orderProducts = await _readProductRepository.GetAllAsync();
		var total = orderProducts.Where(p => orderCreateVM.ProductIds.Any(op => op == p.Id)).Sum(p => p.Price);
		var newOrder = new Order
		{
			OrderNote = orderCreateVM.OrderNote,
			OrderNumber = orderCreateVM.OrderNumber,
			OrderDate = orderCreateVM.OrderDate,
			CustomerId = orderCreateVM.CustomerId,
			Total = (decimal)total!,
			Products = orderProducts.Where(p => orderCreateVM.ProductIds.Any(op => op == p.Id)).ToList()
		};

		await _writeOrderRepository.AddAsync(newOrder);
		await _writeOrderRepository.SaveChangeAsync();
		return Ok();
	}

	[HttpGet("AllOrders")]
	public async Task<IActionResult> AllOrders()
	{
		var orders = await _readOrderRepository.GetAllIncludingAsync(o => o.Customer, o => o.Products);
		if (orders == null || !orders.Any())
		{
			return StatusCode(404);
		}

		List<OrderWCustomerWProductsVM> orderVMs = orders.Select(o => new OrderWCustomerWProductsVM
		{
			Id = o.Id,
			OrderNote = o.OrderNote,
			OrderDate = o.OrderDate,
			OrderNumber = o.OrderNumber,
			Total = o.Total,
			CustomerVM = new CustomerVM
			{
				Id = o.Id,
				FirstName = o.Customer.FirstName,
				LastName = o.Customer.LastName,
				Email = o.Customer.Email,
				Address = o.Customer.Address,
			},
			Products = o.Products.Select(p => new ProductVM
			{
				ID = p.Id,
				Name = p.Name,
				Description = p.Description,
				Price = p.Price,
				Stock = p.Stock,
				ImageUrl = p.ImageUrl,
			}).ToList()
		}).ToList();

		return Ok(orderVMs);

	}


	[HttpPut("UpdateOrder/{id}")]
	public async Task<IActionResult> UpdateOrder(int id, [FromBody] OrderUpdateVM updateVM)
	{
		if (updateVM == null)
		{
			return BadRequest("Invalid order data.");
		}

		var orders = await _readOrderRepository.GetAllIncludingAsync(o => o.Customer, o => o.Products);
		var order = orders.FirstOrDefault(o => o.Id == id);
		if (order == null)
		{
			return StatusCode(404);
		}

		order.OrderNote = updateVM.OrderNote;
		order.OrderNumber = updateVM.OrderNumber;


		if (updateVM.CustomerId > 0)
		{
			var customer = await _readCustomerRepository.GetByIdAsync(updateVM.CustomerId);
			if (customer is not null)
				order.Customer = customer;
		}

		foreach (var productId in updateVM.ProductIds)
		{
			var product = await _readProductRepository.GetByIdAsync(productId);
			if (product is not null)
				order.Products.Add(product);
		}


		await _writeOrderRepository.UpdateAsync(order);
		await _writeOrderRepository.SaveChangeAsync();

		return Ok();
	}


	[HttpDelete("DeleteOrder")]
	public async Task<ActionResult> DeleteOrder(int id)
	{
		var order = await _readOrderRepository.GetByIdAsync(id);
		if (order is null)
			return StatusCode(404);

		await _writeOrderRepository.DeleteAsync(id);
		await _writeOrderRepository.SaveChangeAsync();

		return Ok(order);
	}
}