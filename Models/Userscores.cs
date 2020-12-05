using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PenaltyV2.Models
{
    public class Userscores
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string Favoriteteam { get; set; }
        public Nullable<decimal> Score { get; set; }
        public Nullable<int> Perfects { get; set; }
        public string Competitionyear { get; set; }
        public Nullable<int> Position { get; set; }
    }
}
