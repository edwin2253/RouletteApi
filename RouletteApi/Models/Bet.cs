using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RouletteApi.Models
{
    public class Bet
    {
        public long Id { get; set; }
        public long IdRoullete { get; set; }
        public long IdUser { get; set; }
        public double Quantity { get; set; }
    }
}
