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

        public static List<MatchesBets> GetBetsByUserAndMatchday(int? matchday, string username, ApplicationDbContext dbContext)
        {

            List<MatchesBets> qry = new List<MatchesBets>();
            
                qry = (from m2 in
                           (from m in dbContext.Matches
                            where m.Matchday == matchday
                            select m)
                       join b2 in
                           (from b in dbContext.Bets
                            where b.Username == username
                            select b)
                       on m2.Id equals b2.Idmatch into bGroup
                       from b2 in bGroup.DefaultIfEmpty()
                       select new MatchesBets
                       {
                           Awayteam = m2.Awayteam,
                           Awayteamgoals = m2.Awayteamgoals,
                           Hometeam = m2.Hometeam,
                           Hometeamgoals = m2.Hometeamgoals,
                           IdMatch = m2.Id,
                           Matchday = m2.Matchday,
                           Matchnumber = m2.Matchnumber,
                           Idawayteam = m2.Idawayteam,
                           Idhometeam = m2.Idhometeam,
                           Status = m2.Status,
                           Oddsaway = m2.Oddsaway,
                           Oddsdraw = m2.Oddsdraw,
                           Oddshome = m2.Oddshome,
                           UtcDate = (DateTime)m2.UtcDate,
                           Username = b2.Username,
                           GoalsAwayTeam = b2.GoalsAwayTeam,
                           GoalsHomeTeam = b2.GoalsHomeTeam,
                           Score = b2.Score,
                           Perfect = b2.Perfect,
                           Betsidmatch = b2.Idmatch,
                           Betsmatchday = b2.Matchday,

                       }).ToList();
            

            return qry;
        }

        //INACABADO
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
