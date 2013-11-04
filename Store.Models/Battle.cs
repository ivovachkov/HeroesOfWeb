using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Models
{
    public class Battle
    {
        public int Id { get; set; }

        public virtual User FirstUser { get; set; }

        public virtual User SecondUser { get; set; }

        public virtual User UserInTurn { get; set; }
    }
}
