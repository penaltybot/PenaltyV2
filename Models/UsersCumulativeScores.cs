﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PenaltyV2.Models
{
    public class UsersCumulativeScores
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Matchday { get; set; }
        public Nullable<decimal> Score { get; set; }
        public Nullable<int> CorrectPredictions { get; set; }
        public string LeagueID { get; set; }
    }
}
