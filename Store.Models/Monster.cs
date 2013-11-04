using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Models
{
    public class Monster
    {
        public int MonsterId { get; set; }

        public string Name { get; set; }

        public int MagicAttack { get; set; }

        public int MeleAttack { get; set; }

        public int MagicDefense { get; set; }

        public int MeleDefense { get; set; }

        public int HP { get; set; }

        public int MP { get; set; }
    }
}
