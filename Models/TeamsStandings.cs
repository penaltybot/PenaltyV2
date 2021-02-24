using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PenaltyV2.Models
{
    public class TeamsStandings
    {
        public int Id { get; set; }
        public Nullable<int> Rank { get; set; }
        public Nullable<int> Points { get; set; }
        public string Form { get; set; }
        public Nullable<int> MatchesPlayed { get; set; }
        public Nullable<int> Wins { get; set; }
        public Nullable<int> Draws { get; set; }
        public Nullable<int> Losses { get; set; }
        public Nullable<int> GoalsFor { get; set; }
        public Nullable<int> GoalsAgainst { get; set; }
    }
}
