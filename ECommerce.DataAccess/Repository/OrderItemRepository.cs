using ECommerce.DataAccess.Data;
using ECommerce.DataAccess.Repository.IRepository;
using ECommerce.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.DataAccess.Repository
{
    public class OrderItemRepository : Repository<OrderItem>, IOrderItemRepository
    {
        ApplicationDbContext _db;
        public OrderItemRepository(ApplicationDbContext db) : base(db) 
        {
            _db = db;
        }

        public void Update(OrderItem obj)
        {
            _db.Update(obj);
        }
    }
}
