using PenaltyV2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PenaltyV2.Data
{
    
   
    public class Database
    {

        public static List<Userscores> GetUserscores(ApplicationDbContext dbContext)
        {
           //TODO: Tenho de ir buscar o competition year
            List<Userscores> qry = new List<Userscores>();
            qry = (from us in dbContext.Userscores
                   where us.Competitionyear == "2020"
                   join ui in dbContext.Usersinfo
                   on us.Username equals ui.Username
                   orderby us.Score descending
                   select new Userscores
                   {
                       Id = us.Id,
                       Username = us.Username,
                       Name = ui.Name,
                       Favoriteteam = ui.Favoriteteam,
                       Perfects = us.Perfects,
                       Position = us.Position,
                       Competitionyear = us.Competitionyear,
                       Score = us.Score,
                   }).ToList();
            
            return qry;
        }

        public static List<Matches> GetMatches(ApplicationDbContext dbContext)
        {
            List<Matches> qry = new List<Matches>();

            qry = (from m in dbContext.Matches
                   select new Matches
                   {
                       Awayteam = m.Awayteam,
                       Awayteamgoals = m.Awayteamgoals,
                       Hometeam = m.Hometeam,
                       Hometeamgoals = m.Hometeamgoals,
                       Id = m.Id,
                       Matchday = m.Matchday,
                       Matchnumber = m.Matchnumber,
                       Idawayteam = m.Idawayteam,
                       Idhometeam = m.Idhometeam,
                       UtcDate = m.UtcDate,
                   }).ToList();

            return qry;
        }

        public static int GetCurrentMatchDay(ApplicationDbContext dbContext)
        {
            List<Matches> qry = new List<Matches>();
            qry = (from m in dbContext.Matches
                   where m.UtcDate > DateTime.Today
                   select new Matches
                   {
                       Matchday = m.Matchday
                   }).ToList();
            Matches match = qry.First();

            return (int)match.Matchday;
        }

        public static int GetCurrentMatchYear(ApplicationDbContext dbContext)
        {
            List<Matches> qry = new List<Matches>();
            qry = (from m in dbContext.Matches
                   where m.UtcDate > DateTime.Today
                   select new Matches
                   {
                       Matchday = m.Matchday
                   }).ToList();
            Matches match = qry.First();

            return (int)match.Matchday;
        }
    }
}
