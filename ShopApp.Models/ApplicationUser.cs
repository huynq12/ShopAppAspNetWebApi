
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopApp.Models
{
	public class ApplicationUser : IdentityUser
    {
		public string Email { get; set; }
		public string? FullName { get; set; }
		public string? PhoneNumber { get; set; }
	}
}
