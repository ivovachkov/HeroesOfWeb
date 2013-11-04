using Store.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.Linq.Expressions;

namespace Store.Services.Models
{
    [DataContract]
    public class UserAdminModel
    {
        public static Expression<Func<User, UserAdminModel>> FromUser
        {
            get
            {
                return x => new UserAdminModel
                {
                    UserId = x.UserId,
                    Username = x.Username,
                    DisplayName = x.DisplayName,
                    Gold = x.Gold ?? 0m,
                    //Hero = new HeroModel
                    //{
                    //    Name = x.Hero.Name,
                    //    HP = x.Hero.HP,
                    //    MP = x.Hero.MP,
                    //    MagicAttack = x.Hero.MagicAttack,
                    //    MagicDefense = x.Hero.MagicDefense,
                    //    MeleAttack = x.Hero.MeleAttack,
                    //    MeleDefense = x.Hero.MeleDefense,
                    //    Experience = x.Hero.Experience,
                    //    Level = x.Hero.Level,
                    //    //Items = x.Hero.Items.AsQueryable().Select(ItemModel.FromItem)
                    //}
                    //Hero = Enumerable.Repeat(x.Hero, 1).AsQueryable().Select(HeroModel.FromHero).First()
                };
            }
        }

        [DataMember(Name = "id")]
        public int UserId { get; set; }

        [DataMember(Name = "username")]
        public string Username { get; set; }

        [DataMember(Name = "displayName")]
        public string DisplayName { get; set; }

        [DataMember(Name = "gold")]
        public decimal? Gold { get; set; }

        [DataMember(Name = "hero")]
        public HeroModel Hero { get; set; }
    }

    [DataContract]
    public class UserAdminSimpleModel
    {
        [DataMember(Name = "userId")]
        public int UserId { get; set; }

        [DataMember(Name = "username")]
        public string Username { get; set; }
    }
}