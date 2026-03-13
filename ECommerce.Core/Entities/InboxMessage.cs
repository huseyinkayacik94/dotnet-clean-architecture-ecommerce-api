using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Core.Entities
{
    public class InboxMessage
    {
        public Guid Id { get; set; }

        public string MessageId { get; set; }

        public DateTime ProcessedAt { get; set; }
    }
}
