using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PenaltyV2.Models
{
    public class MatchesBets
    {
        

        public int IdMatch{ get; set; }
        public Nullable<int> Matchday { get; set; }
        public Nullable<int> Matchnumber { get; set; }
        public string Hometeam { get; set; }
        public Nullable<int> Hometeamgoals { get; set; }
        public string Awayteam { get; set; }
        public Nullable<int> Awayteamgoals { get; set; }
        public Nullable<int> Idawayteam { get; set; }
        public Nullable<int> Idhometeam { get; set; }
        public string Status { get; set; }
        public DateTime UtcDate { get; set; }
        public int? IdMatchAPI { get; set; }
        public Nullable<decimal> Oddshome { get; set; }
        public Nullable<decimal> Oddsaway { get; set; }
        public Nullable<decimal> Oddsdraw { get; set; }
        public Nullable<int> GoalsHomeTeam { get; set; }
        public Nullable<int> GoalsAwayTeam { get; set; }
        public string Username { get; set; }
        public Nullable<int> BetsidmatchAPI { get; set; }
        public Nullable<int> Betsmatchday { get; set; }
        public decimal? Score { get; set; }
        public int? Perfect { get; set; }
        public string BetResult1 { get; set; }
        public decimal? Multiplier { get; set; }

    }
}
