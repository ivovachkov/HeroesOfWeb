using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace Store.Services.Models
{
    [DataContract]
    public class MonsterModel
    {
        [DataMember(Name = "id")]
        public int MonsterId { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "meleAttack")]
        public int MeleAttack { get; set; }

        [DataMember(Name = "magicAttack")]
        public int MagicAttack { get; set; }

        [DataMember(Name = "magicDefense")]
        public int MagicDefense { get; set; }

        [DataMember(Name = "meleDefense")]
        public int MeleDefense { get; set; }

        [DataMember(Name = "hp")]
        public int HP { get; set; }

        [DataMember(Name = "mp")]
        public int MP { get; set; }
    }

    [DataContract]
    public class CreatedMonsterModel
    {
        [DataMember(Name = "id")]
        public int Monsterid { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }
    }
}