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
        public double Value { get; set; }
        public int Number { get; set; }
        public string Color { get; set; }
        public long Game { get; set; }
    }
}
