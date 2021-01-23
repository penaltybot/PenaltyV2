using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PenaltyV2.Models
{
    public class LegacyScores
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Season { get; set; }
        public decimal Score { get; set; }
        public int Position { get; set; }
        public string Name { get; set; }

    }
}
