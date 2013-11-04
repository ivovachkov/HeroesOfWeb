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
    public class CreateBattleModel
    {
        [DataMember(Name = "first")]
        public int FirstUserId { get; set; }

        [DataMember(Name = "second")]
        public int SecondUserId { get; set; }

        [DataMember(Name = "userInTurn")]
        public int UserInTurnId { get; set; }
    }

    [DataContract]
    public class BattleCreateReturnModel
    {
        [DataMember(Name = "id")]
        public int ID { get; set; }
    }
}