using ECommerce.Domain.ViewModels.ProductViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.ViewModels.OrderViewModels
{
	public class OrderWCustomerWProductsVM
	{
		public int Id { get; set; }
		public string? OrderNumber { get; set; }
		public DateTime OrderDate { get; set; }
		public string? OrderNote { get; set; }
		public decimal Total { get; set; }

		public CustomerVM CustomerVM { get; set; }
		public List<ProductVM> Products { get; set; }
	}
}
