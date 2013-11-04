using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Models
{
    public class Admin
    {
        public int AdminId { get; set; }

        public string Username { get; set; }

        public string AuthCode { get; set; }

        public string DisplayName { get; set; }

        public string SessionKey { get; set; }

        public string ImageURL { get; set; }
    }
}
