using ECommerce.DataAccess.Data;
using ECommerce.DataAccess.Repository.IRepository;
using ECommerce.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.DataAccess.Repository
{
    public class AuditLogRepository : Repository<AuditLog> , IAuditLogRepository
    {
        private readonly ApplicationDbContext _db;
        public AuditLogRepository(ApplicationDbContext db) : base (db)
        {
            _db = db;
        }

    }
}
