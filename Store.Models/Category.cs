using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Store.Models
{
    public class ItemCategory
    {
        public int ItemCategoryId { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Item> Items { get; set; }

        public ItemCategory()
        {
            this.Items = new HashSet<Item>();
        }
    }
}
