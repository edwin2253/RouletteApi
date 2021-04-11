using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RouletteApi.Models
{
    public class Roulette
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool IsOpen { get; set; }
    }
}
