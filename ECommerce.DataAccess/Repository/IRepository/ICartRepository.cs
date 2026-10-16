using ECommerce.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.DataAccess.Repository.IRepository
{
    public interface ICartRepository : IRepository<Cart>
    {
        void Update(Cart obj);
    }
}
