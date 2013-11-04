using Store.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Linq.Expressions;
using System.Web;

namespace Store.Services.Models
{
    [DataContract]
    public class ItemModel
    {
        public static Expression<Func<Item, ItemModel>> FromItem
        {
            get
            {
                return x => new ItemModel
                {
                    ItemId = x.ItemId,
                    Name = x.Name,
                    HP = x.HP,
                    MP = x.MP,
                    MeleAttack = x.MeleAttack,
                    MagicDefense = x.MagicDefense,
                    MeleDefense = x.MeleDefense,
                    MagicAttack = x.MagicAttack,
                    ItemCategory = x.ItemCategory.Name,
                    Description = x.Description,
                    Equiped = x.Equiped,
                    ImageUrl = x.ImageUrl,
                    Price = x.Price
                };
            }
        }

        [DataMember(Name = "itemId")]
        public int ItemId { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }

        [DataMember(Name = "hp")]
        public int HP { get; set; }

        [DataMember(Name = "mp")]
        public int MP { get; set; }

        [DataMember(Name = "magicAttack")]
        public int MagicAttack { get; set; }

        [DataMember(Name = "meleAttack")]
        public int MeleAttack { get; set; }

        [DataMember(Name = "magicDefense")]
        public int MagicDefense { get; set; }

        [DataMember(Name = "meleDefense")]
        public int MeleDefense { get; set; }

        [DataMember(Name = "itemCategory")]
        public string ItemCategory { get; set; }

        [DataMember(Name = "imageUrl")]
        public string ImageUrl { get; set; }

        [DataMember(Name = "equiped")]
        public bool Equiped { get; set; }

        [DataMember(Name = "price")]
        public decimal Price { get; set; }
    }

    [DataContract]
    public class UpdatingItemModel
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }

        [DataMember(Name = "magicAttack")]
        public int MagicAttack { get; set; }

        [DataMember(Name = "meleAttack")]
        public int MeleAttack { get; set; }

        [DataMember(Name = "magicDefense")]
        public int MagicDefense { get; set; }

        [DataMember(Name = "meleDefense")]
        public int MeleDefense { get; set; }

        [DataMember(Name = "itemCategory")]
        public string ItemCategory { get; set; }

        [DataMember(Name = "imageUrl")]
        public string ImageUrl { get; set; }

        [DataMember(Name = "price")]
        public decimal Price { get; set; }

        [DataMember(Name = "hp")]
        public int HP { get; set; }

        [DataMember(Name = "mp")]
        public int MP { get; set; }
    }
}