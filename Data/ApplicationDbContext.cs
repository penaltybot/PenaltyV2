using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PenaltyV2.Models;

namespace PenaltyV2.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Userscores> Userscores { get; set; }
        public DbSet<Matches> Matches { get; set; }
        public DbSet<Bets> Bets { get; set; }
        public DbSet<Globalconstants> Globalconstants { get; set; }
        public DbSet<Usersinfo> Usersinfo { get; set; }
        public DbSet<Teams> Teams { get; set; }
        public DbSet<Leagues> Leagues { get; set; }
        public DbSet<LegacyScores> LegacyScores { get; set; }
    }

}
