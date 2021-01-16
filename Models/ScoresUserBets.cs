using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PenaltyV2.Models
{
    public class ScoresUserBets
    {
        public Nullable<int> IdMatchAPI { get; set; }
        public string Hometeam { get; set; }
        public Nullable<int> Hometeamgoals { get; set; }
        public string Awayteam { get; set; }
        public Nullable<int> Awayteamgoals { get; set; }
        public string Result1 { get; set; }
        public List<UsersBets> Userbets { get; set; }
    }
}
