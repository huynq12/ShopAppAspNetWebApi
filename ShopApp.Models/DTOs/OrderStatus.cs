using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopApp.Models.DTOs
{
    public enum OrderStatus
    {
        New,
        Processing,
        Shipping,
        Shipped,
        Complete,
        Cancel,
    }
}
