using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.ViewModels.CustomerViewModels
{
	internal class CustomerVM
	{
		public int id {  get; set; }
		public string? FirstName { get; set; }
		public string? LastName { get; set; }
		public string? Address { get; set; }
		public string? Email { get; set; }
	}
}
