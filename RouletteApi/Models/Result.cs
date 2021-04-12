using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RouletteApi.Models
{
    public class Result
    {
        public long IdRoulette { get; set; }
        public int Number { get; set; }
        public string Color { get; set; }
        public List<User> Users { get; set; }
    }
}
