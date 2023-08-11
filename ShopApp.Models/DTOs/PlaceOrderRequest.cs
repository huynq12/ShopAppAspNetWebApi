﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopApp.Models.DTOs
{
	public class PlaceOrderRequest
	{
		public string Address { get; set; }
		public List<CartItem> CartItems { get; set; }
	}
}
