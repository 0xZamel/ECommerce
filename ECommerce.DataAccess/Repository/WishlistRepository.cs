using ECommerce.DataAccess.Data;
using ECommerce.DataAccess.Repository.IRepository;
using ECommerce.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.DataAccess.Repository
{
    public class WishlistRepository : Repository<Wishlist>,IWishlistRepository
    {
        ApplicationDbContext _db;
        public WishlistRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
