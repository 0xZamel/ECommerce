using ECommerce.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IProductRepository Product { get; }
        ICartRepository Cart { get; }
        ICartItemRepository CartItem { get; }
        IOrderRepository Order { get; }
        IOrderItemRepository OrderItem { get; }
        ICategoryRepository Category { get; }
        IReviewRepository Review { get; }
        IWishlistRepository Wishlist { get; }
        IAuditLogRepository AuditLog { get; }
        void Save();
    }
}
