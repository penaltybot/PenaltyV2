using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PenaltyV2.Models
{
    public class Bets
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public int Idmatch { get; set; }
        public Nullable<int> GoalsHomeTeam { get; set; }
        public Nullable<int> GoalsAwayTeam { get; set; }
        public string Result { get; set; }
        public Nullable<decimal> Score { get; set; }
        public Nullable<int> Perfect { get; set; }
        public Nullable<int> Matchday { get; set; }
    }
}
