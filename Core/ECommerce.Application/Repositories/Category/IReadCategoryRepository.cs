using ECommerce.Domain.Entities.Concretes;
using ECommerce.Domain.Enums;
using ECommerce.Domain.ViewModels.CategoryViewModels;
namespace ECommerce.Application.Repositories;

public interface IReadCategoryRepository : IReadGenericRepository<Category>
{
	Task<Category> GetCategoryWithProducts(int id);

	Task<List<CategoryVM>> GetSortedCategories(SortOptions sortOptions,string sortBy);

}
