using ECommerce.Domain.Entities.Concretes;
using ECommerce.Domain.ViewModels.ProductViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.ViewModels.CategoryViewModels
{
    public class CategoryWithProductsVM
	{
		public int Id { get; set; }
		public string? Name { get; set; }
		public string? Description { get; set; }
		public string? ImageUrl { get; set; }
		public List<AllProductVM>? Products { get; set; }
	}
}
