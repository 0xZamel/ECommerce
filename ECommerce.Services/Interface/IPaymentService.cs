using ECommerce.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Services.Interface
{
    public interface IPaymentService
    {
        Task<string> CreateCheckoutSessionAsync(Order order, List<OrderItem> orderItems);
    }
}
