using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Models
{
    public class User
    {
        public int UserId { get; set; }

        public string Username { get; set; }

        public string AuthCode { get; set; }

        public string DisplayName { get; set; }

        public string SessionKey { get; set; }

        public string ImageURL { get; set; }

        public decimal? Gold { get; set; }

        public bool Busy { get; set; }

        public virtual Hero Hero { get; set; }
    }
}
