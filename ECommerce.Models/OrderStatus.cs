using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Models
{
    public enum OrderStatus
    {
        Pending,
        Processing,
        Shipped,
        Completed,
        Cancelled
    }
}
