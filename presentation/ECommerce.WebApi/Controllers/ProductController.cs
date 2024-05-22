using ECommerce.Application.Repositories;
using ECommerce.Domain.Entities.Concretes;
using ECommerce.Domain.ViewModels;
using ECommerce.Domain.ViewModels.ProductViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.Json;
using System.Text.Json;

namespace ECommerce.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
	private readonly IReadProductRepository _readProductRepo;
	private readonly IWriteProductRepository _writeProductRepo;

	public ProductController(IReadProductRepository readProductRepo, IWriteProductRepository writeProductRepo)
	{
		_readProductRepo = readProductRepo;
		_writeProductRepo = writeProductRepo;
	}

	[HttpGet("AllProducts")]
	public async Task<IActionResult> GetAll([FromQuery] PaginationVM paginationVM)
	{
		var products = await _readProductRepo.GetAllAsync();
		var prodcutForPage = products.ToList().
					Skip(paginationVM.Page * paginationVM.PageSize).Take(paginationVM.PageSize).ToList();


		var allProductVm = prodcutForPage.Select(p => new AllProductVM()
		{
			Name = p.Name,
			Price = p.Price,
			Description = p.Description,
			CategoryName = p.Category.Name,
			ImageUrl = p.ImageUrl,
			Stock = p.Stock
		}).ToList();

		return Ok(allProductVm);
	}

	[HttpGet("GetProduct")]
	public async Task<IActionResult> GetProduct([FromQuery] int id)
	{
		var p = await _readProductRepo.GetByIdAsync(id);
		if (p is null)
		{
			return StatusCode(404);
		}
		AllProductVM getProduct = new AllProductVM
		{
			Name = p.Name,
			Price = p.Price,
			Description = p.Description,
			CategoryName = p.Category.Name,
			ImageUrl = p.ImageUrl,
			Stock = p.Stock
		};
		return Ok(getProduct);
	}

	[HttpPost("AddProduct")]
	public async Task<IActionResult> AddProduct([FromBody] AddProductVM productVM)
	{
		if (!ModelState.IsValid)
			return BadRequest(ModelState);

		var product = new Product()
		{
			Name = productVM.Name,
			Price = productVM.Price,
			Description = productVM.Description,
			CategoryId = productVM.CategoryId,
		};

		await _writeProductRepo.AddAsync(product);
		await _writeProductRepo.SaveChangeAsync();

		return StatusCode(201);
	}

	[HttpDelete("DeleteProduct")]
	public async Task<IActionResult> Delete([FromHeader] int id)
	{
		if (!ModelState.IsValid)
			return BadRequest(ModelState);

		await _writeProductRepo.DeleteAsync(id);
		await _writeProductRepo.SaveChangeAsync();

		return StatusCode(202);
	}

	[HttpPut("UpdateProduct")]
	public async Task<IActionResult> Update([FromBody] UpdateProductVM updateProductVM)
	{
		if (!ModelState.IsValid)
			return BadRequest(ModelState);
		var product = await _readProductRepo.GetByIdAsync(updateProductVM.Id);

		product.Name = updateProductVM.Name;
		product.Description = updateProductVM.Description;
		product.Price = updateProductVM.Price;
		product.Stock = updateProductVM.Stock;
		product.ImageUrl = updateProductVM.ImageUrl;

		await _writeProductRepo.UpdateAsync(product);
		await _writeProductRepo.SaveChangeAsync();

		return StatusCode(202);
	}

}
