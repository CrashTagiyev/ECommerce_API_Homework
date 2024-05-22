using ECommerce.Domain.ViewModels.ProductViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.ViewModels.OrderViewModels
{
	public class OrderUpdateVM
	{
		public string? OrderNumber { get; set; }
		public string? OrderNote { get; set; }

        public int CustomerId { get; set; }
        public List<int> ProductIds { get; set; }
	}
}
