using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Entities
{
    public class Operation : BaseEntity
    {
        public string Name { get; set; }
        public decimal Sum { get; set; }
        public OperationType Type { get; set; }
        [ForeignKey("Card")]
        public int CardId { get; set; }
        public virtual Card Card { get; set; } 
    }
}
