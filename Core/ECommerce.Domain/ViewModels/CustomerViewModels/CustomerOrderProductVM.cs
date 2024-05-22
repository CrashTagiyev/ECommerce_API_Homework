using ECommerce.Domain.Entities.Concretes;
using ECommerce.Domain.ViewModels.OrderViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.ViewModels.CustomerViewModels
{
	public class CustomerOrderProductVM
	{
		public int id { get; set; }
		public string? FirstName { get; set; }
		public string? LastName { get; set; }
		public string? Address { get; set; }
		public string? Email { get; set; }
		public string? Password { get; set; }

		// Navigation Property
		public virtual List<OrderWithProductVM> Orders { get; set; }
	}
}
