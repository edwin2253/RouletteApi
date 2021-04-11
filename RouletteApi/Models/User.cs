using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RouletteApi.Models
{
    public class User
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public double Money { get; set; }
    }
}
