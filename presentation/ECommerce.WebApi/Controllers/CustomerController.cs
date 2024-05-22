using ECommerce.Application.Repositories;
using ECommerce.Domain.Entities.Concretes;
using ECommerce.Domain.ViewModels;
using ECommerce.Domain.ViewModels.CustomerViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CustomerController : ControllerBase
{
	private readonly IWriteCustomerRepository _writeCustomerRepository;
	private readonly IReadCustomerRepository _readCustomerRepository;

	public CustomerController(IWriteCustomerRepository writeCustomerRepository, IReadCustomerRepository readCustomerRepository)
	{
		_writeCustomerRepository = writeCustomerRepository;
		_readCustomerRepository = readCustomerRepository;
	}




	[HttpGet("CustomersWithOrdersWithProduct")]
	public async Task<IActionResult> CustomersWithOrdersWithProduct()
	{
		var customers= await _readCustomerRepository.GetCustomersWithOrdersWithProducts();
		return Ok(customers);
	}

	[HttpPost("Create")]
	public async Task<IActionResult> Create([FromBody] CustomerCreateVM customerVM)
	{
		if (customerVM is null)
			return BadRequest();

		Customer newCustomerVM = new()
		{
			FirstName = customerVM.FirstName,
			LastName = customerVM.LastName,
			Email = customerVM.Email,
			Address = customerVM.Address,
		};

		await _writeCustomerRepository.AddAsync(newCustomerVM);
		await _writeCustomerRepository.SaveChangeAsync();
		return Ok();

	}

	[HttpGet("GetAll")]
	public async Task<IActionResult> GetAll()
	{
		var customers = await _readCustomerRepository.GetAllAsync();
		List<CustomerVM> customerVMs = customers.Select(x => new CustomerVM
		{
			Id = x.Id,
			FirstName = x.FirstName,
			LastName = x.LastName,
			Email = x.Email,
			Address = x.Address,
		}).ToList();

		return Ok(customerVMs);

	}




	[HttpPut("Update")]
	public async Task<IActionResult> Update(CustomerVM customerVM)
	{
		var customer = await _readCustomerRepository.GetByIdAsync(customerVM.Id);

		if (customer is null)
			return StatusCode(404);

		customer = new()
		{
			FirstName = customerVM.FirstName,
			LastName = customerVM.LastName,
			Email = customerVM.Email,
			Address = customerVM.Address,
		};

		await _writeCustomerRepository.UpdateAsync(customer);
		await _writeCustomerRepository.SaveChangeAsync();

		return Ok();
	}

	[HttpDelete("Delete")]
	public async Task<IActionResult> Delete(int id)
	{
		var customer = await _readCustomerRepository.GetByIdAsync(id);

		if (customer is null)
			return StatusCode(404);

		await _writeCustomerRepository.DeleteAsync(customer);
		await _writeCustomerRepository.SaveChangeAsync();
	
		return Ok();
	}



}

