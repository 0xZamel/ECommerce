using ECommerce.DataAccess.Data;
using ECommerce.DataAccess.Repository.IRepository;
using ECommerce.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        public IProductRepository Product { get; private set; }
        public ICartRepository Cart { get; private set; }
        public ICartItemRepository CartItem { get; private set; }
        public IOrderRepository Order { get; private set; }
        public IOrderItemRepository OrderItem { get; private set; }
        public ICategoryRepository Category { get; private set; }
        public IReviewRepository Review { get; private set; }
        public IWishlistRepository Wishlist { get; private set; }
        public IAuditLogRepository AuditLog { get; private set; }
        
        private readonly ApplicationDbContext _db;
        public UnitOfWork(
        ApplicationDbContext db)
        {
            _db = db;   
            Product = new ProductRepository(_db);
            Cart = new CartRepository(_db);
            CartItem = new CartItemRepository(_db);
            Order = new OrderRepository(_db);
            OrderItem = new OrderItemRepository(_db);
            Category = new CategoryRepository(_db);
            Review = new ReviewRepository(_db);
            Wishlist = new WishlistRepository(_db);
            AuditLog = new AuditLogRepository(_db);
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
