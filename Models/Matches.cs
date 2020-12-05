using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PenaltyV2.Models
{
    public class Matches
    {
        public int Id { get; set; }
        public Nullable<int> Matchday { get; set; }
        public Nullable<int> Matchnumber { get; set; }
        public string Hometeam { get; set; }
        public Nullable<int> Hometeamgoals { get; set; }
        public string Awayteam { get; set; }
        public Nullable<int> Awayteamgoals { get; set; }
        public Nullable<int> Idawayteam { get; set; }
        public Nullable<int> Idhometeam { get; set; }
        public string Status { get; set; }
        public string Competitionyear { get; set; }
        public Nullable<System.DateTime> UtcDate { get; set; }
        public Nullable<int> IdmatchAPI { get; set; }
        public string Result1 { get; set; }
        public Nullable<decimal> Oddshome { get; set; }
        public Nullable<decimal> Oddsaway { get; set; }
        
    }
}
