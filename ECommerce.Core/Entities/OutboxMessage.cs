using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Core.Entities
{
    public class OutboxMessage
    {
        public Guid Id { get; set; }

        public string Type { get; set; }

        public string Payload { get; set; }

        public bool Processed { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
