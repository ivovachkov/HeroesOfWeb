using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Models
{
    public class Hero
    {
        public int HeroId { get; set; }

        public string Name { get; set; }

        public int MaxHP { get; set; }

        public int HP { get; set; }

        public int MaxMP { get; set; }

        public int MP { get; set; }

        public int MagicAttack { get; set; }

        public int MeleAttack { get; set; }

        public int MagicDefense { get; set; }

        public int MeleDefense { get; set; }

        public int Experience { get; set; }

        public int Level { get; set; }

        public virtual User User { get; set; }

        public virtual ICollection<Item> Items { get; set; }

        public Hero()
        {
            this.Items = new HashSet<Item>();
        }
    }
}
