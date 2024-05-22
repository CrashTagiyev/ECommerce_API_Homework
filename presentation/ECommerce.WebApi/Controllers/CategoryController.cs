using ECommerce.Application.Repositories;
using ECommerce.Domain.Entities.Concretes;
using ECommerce.Domain.Enums;
using ECommerce.Domain.ViewModels;
using ECommerce.Domain.ViewModels.CategoryViewModels;
using ECommerce.Domain.ViewModels.ProductViewModels;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoryController : ControllerBase
{
	private readonly IWriteCategoryRepository _writeCategoryRepo;
	private readonly IReadCategoryRepository _readCategoryRepo;

	public CategoryController(IWriteCategoryRepository writeCategoryRepo, IReadCategoryRepository readCategoryRepo)
	{
		_writeCategoryRepo = writeCategoryRepo;
		_readCategoryRepo = readCategoryRepo;
	}
	[HttpGet("GetAllCategoriesSorted")]
	public async Task<IActionResult> GetAllCategoriesSorted(SortOptions sortOptions, string SortBy)
	{
		var categories = await _readCategoryRepo.GetSortedCategories(sortOptions, SortBy);
		if (categories is null)
			return StatusCode(404);
		return Ok(categories);
	}

	[HttpGet("GetAllCategories")]
	public async Task<IActionResult> GetAllCategories()
	{
		var categories = await _readCategoryRepo.GetAllAsync();
		List<CategoryVM> categoryModels = categories.Select(c => new CategoryVM()
		{
			Id = c.Id,
			Name = c.Name,
			Description = c.Description,
			ImageUrl = c.ImageUrl,
		}).ToList();
		return Ok(categoryModels);
	}


	[HttpGet("GetCategoryWithProducts")]
	public async Task<IActionResult> GetAllCategoriesWithProducts([FromQuery] int id)
	{
		var category = await _readCategoryRepo.GetCategoryWithProducts(id);
		if (category is null)
			return StatusCode(404);

		CategoryWithProductsVM categoryModels = new()
		{
			Id = category.Id,
			Name = category.Name,
			Description = category.Description,
			ImageUrl = category.ImageUrl,
			Products = category.Products.Select(p => new AllProductVM()
			{
				Name = p.Name,
				Description = p.Description,
				Price = p.Price,
				Stock = p.Stock,
				ImageUrl = p.ImageUrl
			}).ToList()
		};
		return Ok(categoryModels);
	}



	[HttpPost("AddCategory")]
	public async Task<IActionResult> AddCategory([FromBody] AddCategoryVM categoryVM)
	{
		if (!ModelState.IsValid)
			return BadRequest(ModelState);

		var category = new Category()
		{
			Name = categoryVM.Name,
			Description = categoryVM.Description,
		};

		await _writeCategoryRepo.AddAsync(category);
		await _writeCategoryRepo.SaveChangeAsync();

		return StatusCode(201);
	}

	[HttpDelete("DeleteCategory")]
	public async Task<IActionResult> AddCategory([FromQuery] int id)
	{
		if (!ModelState.IsValid)
			return BadRequest(ModelState);
		var c = await _readCategoryRepo.GetByIdAsync(id);

		if (c is not null)
		{
			await _writeCategoryRepo.DeleteAsync(id);
			await _writeCategoryRepo.SaveChangeAsync();
			return StatusCode(201);
		}
		return StatusCode(404);
	}

	[HttpPut("UpdateCategory")]
	public async Task<IActionResult> UpdateCategory([FromBody] UpdateCategoryVM updateCategoryVM)
	{
		if (!ModelState.IsValid)
			return BadRequest(ModelState);
		var c = await _readCategoryRepo.GetByIdAsync(updateCategoryVM.Id);

		if (c is not null)
		{
			var updatedCategory = new Category
			{
				Name = c.Name,
				Description = c.Description,
			};
			await _writeCategoryRepo.UpdateAsync(updatedCategory);
			await _writeCategoryRepo.SaveChangeAsync();
			return StatusCode(203);
		}
		return StatusCode(404);
	}
}
