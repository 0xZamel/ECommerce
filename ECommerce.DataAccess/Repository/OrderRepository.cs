using ECommerce.DataAccess.Data;
using ECommerce.DataAccess.Repository.IRepository;
using ECommerce.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.DataAccess.Repository
{
    public class OrderRepository : Repository<Order> , IOrderRepository 
    {
        ApplicationDbContext _db;
        public OrderRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Order obj)
        {
            _db.Update(obj);
        }
    }
}
