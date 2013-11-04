using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Store.Models
{
    public class Item
    {
        public int ItemId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int HP { get; set; }

        public int MP { get; set; }

        public int MagicAttack { get; set; }

        public int MeleAttack { get; set; }

        public int MagicDefense { get; set; }

        public int MeleDefense { get; set; }

        public string ImageUrl { get; set; }

        public bool Equiped { get; set; }

        public decimal Price { get; set; }

        public virtual ItemCategory ItemCategory { get; set; }

        public virtual ICollection<Hero> Heroes { get; set; }

        public Item()
        {
            this.Heroes = new HashSet<Hero>();
        }
    }
}
