using ECommerce.DataAccess.Data;
using ECommerce.DataAccess.Repository.IRepository;
using ECommerce.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace ECommerce.DataAccess.Repository
{
    public class CartRepository : Repository<Cart>, ICartRepository
    {
        ApplicationDbContext _db;
        public CartRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        
        public void Update(Cart obj)
        {
            _db.Update(obj);
        }
    }
}
