using System;
using System.Collections.Generic;
using System.Linq;

using System.Linq.Expressions;
using System.Web;
using Store.Models;
using System.Runtime.Serialization;

namespace Store.Services.Models
{
    [DataContract]
    public class HeroModel
    {
        public static Expression<Func<Hero, HeroModel>> FromHero
        {
            get
            {
                return x => new HeroModel
                {
                    Id = x.HeroId,
                    Name = x.Name,
                    MaxHP = x.MaxHP,
                    HP = x.HP,
                    MaxMP = x.MaxMP,
                    MP = x.MP,
                    MagicAttack = x.MagicAttack,
                    MagicDefense = x.MagicDefense,
                    MeleAttack = x.MeleAttack,
                    MeleDefense = x.MeleDefense,
                    Experience = x.Experience,
                    Level = x.Level,
                    Items = x.Items.AsQueryable().Select(ItemModel.FromItem)
                };
            }
        }

        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "maxHp")]
        public int MaxHP { get; set; }

        [DataMember(Name = "hp")]
        public int HP { get; set; }

        [DataMember(Name = "maxMp")]
        public int MaxMP { get; set; }

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

        [DataMember(Name = "experience")]
        public int Experience { get; set; }

        [DataMember(Name = "level")]
        public int Level { get; set; }

        [DataMember(Name = "items")]
        public IQueryable<ItemModel> Items { get; set; }
    }

    [DataContract]
    public class HeroUpdateModel
    {
        public static Expression<Func<Hero, HeroModel>> FromHero
        {
            get
            {
                return x => new HeroModel
                {
                    Id = x.HeroId,
                    Name = x.Name,
                    MaxHP = x.MaxHP,
                    HP = x.HP,
                    MaxMP = x.MaxMP,
                    MP = x.MP,
                    MagicAttack = x.MagicAttack,
                    MagicDefense = x.MagicDefense,
                    MeleAttack = x.MeleAttack,
                    MeleDefense = x.MeleDefense,
                    Experience = x.Experience,
                    Level = x.Level,
                    Items = x.Items.AsQueryable().Select(ItemModel.FromItem)
                };
            }
        }

        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "maxHp")]
        public int MaxHP { get; set; }

        [DataMember(Name = "hp")]
        public int HP { get; set; }

        [DataMember(Name = "maxMp")]
        public int MaxMP { get; set; }

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

        [DataMember(Name = "experience")]
        public int Experience { get; set; }

        [DataMember(Name = "level")]
        public int Level { get; set; }

        [DataMember(Name = "items")]
        public ICollection<ItemModel> Items { get; set; }
    }

    [DataContract]
    public class HeroCreateModel
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "maxHp")]
        public int MaxHP { get; set; }

        [DataMember(Name = "hp")]
        public int HP { get; set; }

        [DataMember(Name = "maxMp")]
        public int MaxMP { get; set; }

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

        [DataMember(Name = "experience")]
        public int Experience { get; set; }

        [DataMember(Name = "level")]
        public int Level { get; set; }
    }

    [DataContract]
    public class HeroCreateReturnModel
    {
        [DataMember(Name = "id")]
        public int ID { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }
    }
}