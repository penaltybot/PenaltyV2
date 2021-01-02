using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PenaltyV2.Models
{
    public class Usersinfo
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string Leagues { get; set; }
        public string Favoriteteam { get; set; }
        public bool SendEmailReminder { get; set; }
    }
}
