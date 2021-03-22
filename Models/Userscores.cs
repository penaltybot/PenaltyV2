using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PenaltyV2.Models
{
    public class Userscores
    {
        public string Username { get; set; }
        public string Name { get; set; }
        public string Favoriteteam { get; set; }
        public decimal Score { get; set; }
        public Nullable<int> Perfects { get; set; }
        public string Competitionyear { get; set; }
    }
}
