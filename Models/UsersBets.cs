using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PenaltyV2.Models
{
    public class UsersBets
    {
        public string Username { get; set; }
        public string Iduser { get; set; }
        public Nullable<int> IdMatch { get; set; }
        public Nullable<int> GoalsHomeTeam { get; set; }
        public Nullable<int> GoalsAwayTeam { get; set; }
        public string Result { get; set; }
        public Nullable<decimal> Score { get; set; }
        public Nullable<int> Perfect { get; set; }
        public Nullable<int> Matchday { get; set; }
        public string Name { get; set; }
    }
}
