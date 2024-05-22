using ECommerce.Application.Repositories;
using ECommerce.Domain.Entities.Concretes;
using ECommerce.Domain.Enums;
using ECommerce.Domain.ViewModels.CategoryViewModels;
using ECommerce.Persistence.DbContexts;
using ECommerce.Persistence.Repositories.Common;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ECommerce.Persistence.Repositories;

public class ReadCategoryRepository : ReadGenericRepository<Category>, IReadCategoryRepository
{
	public ReadCategoryRepository(ECommerceDbContext context) : base(context)
	{
	}

	public async Task<Category> GetCategoryWithProducts(int id)
	{
		var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
		if (category != null)
			return category;
		return null!;
	}

	public async Task<List<CategoryVM>> GetSortedCategories(SortOptions sortOptions, string sortBy)
	{
		List<Category> categories = new();
		List<CategoryVM> categoryVMs = new();
		var sortedByProperty = typeof(CategoryVM).GetProperty(sortBy);

		if (sortedByProperty is not null)
			switch (sortOptions)
			{
				case SortOptions.Ascending:
					switch (sortedByProperty.Name)
					{
						case "Id": categories = _context.Categories.OrderBy(c=>c.Id).ToList(); break;
						case "Name": categories = _context.Categories.OrderBy(c => c.Name).ToList(); break;
					}
					categoryVMs = categories.Select(c => new CategoryVM
					{
						Id = c.Id,
						Name = c.Name,
						Description = c.Description,
						ImageUrl = c.ImageUrl,
					}).ToList();
					return await Task.FromResult(categoryVMs);

				case SortOptions.Descending:
					switch (sortedByProperty.Name)
					{
						case "Id": categories = _context.Categories.OrderByDescending(c => c.Id).ToList(); break;
						case "Name": categories = _context.Categories.OrderByDescending(c => c.Name).ToList(); break;
					}
					categoryVMs = _context.Categories.OrderByDescending(c => c.Name).Select(c => new CategoryVM
					{
						Id = c.Id,
						Name = c.Name,
						Description = c.Description,
						ImageUrl = c.ImageUrl,
					}).ToList();
					return await Task.FromResult(categoryVMs);
			}
		return null!;
	}

}
