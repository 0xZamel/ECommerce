using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Models
{
    public class AuditLog
    {
        public int Id { get; set; }

        public string UserId { get; set; } = null!;

        public string Action { get; set; } = null!;

        public string EntityName { get; set; } = null!;

        public int? EntityId { get; set; }

        public string? Details { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ApplicationUser User { get; set; } = null!;
    }
}
