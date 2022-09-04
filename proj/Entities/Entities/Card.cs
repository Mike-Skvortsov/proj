using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Entities
{
    public class Card : BaseEntity
    {
        public string NumberCard { get; set; }
        public decimal CardAmount { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public virtual List<Operation> Operations { get; private set; }
    }
}
