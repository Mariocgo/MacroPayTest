using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MacroPay.Models
{
    public class Contacts
    {
        
        public string id { get; set; }
        
        public string name { get; set; }

        public string phone { get; set; }

        public List<string> addressLines { get; set; }
    }
}
