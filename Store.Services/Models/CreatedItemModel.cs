using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Store.Services.Controllers
{
    [DataContract]
    public class CreatedItemModel
    {
        [DataMember(Name = "id")]
        public int ItemId { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }
    }
}
