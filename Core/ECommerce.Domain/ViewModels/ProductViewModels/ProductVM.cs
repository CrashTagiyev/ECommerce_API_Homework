using ECommerce.Domain.Entities.Concretes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.ViewModels.ProductViewModels
{
	public class ProductVM
	{
		public int ID { get; set; }
		public string? Name { get; set; }
		public string? Description { get; set; }
		public decimal? Price { get; set; }
		public int? Stock { get; set; }
		public string? ImageUrl { get; set; }

	}
}
