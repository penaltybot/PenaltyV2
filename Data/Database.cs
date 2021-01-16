﻿using MySql.Data.MySqlClient;
using PenaltyV2.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
                   orderby m.UtcDate
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
                       on m2.IdmatchAPI equals b2.IdmatchAPI into bGroup
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
                           IdMatchAPI = m2.IdmatchAPI,
                           Oddsaway = m2.Oddsaway,
                           Oddsdraw = m2.Oddsdraw,
                           Oddshome = m2.Oddshome,
                           UtcDate = (DateTime)m2.UtcDate,
                           Username = b2.Username,
                           GoalsAwayTeam = b2.GoalsAwayTeam,
                           GoalsHomeTeam = b2.GoalsHomeTeam,
                           Score = b2.Score,
                           Perfect = b2.Perfect,
                           BetsidmatchAPI = b2.IdmatchAPI,
                           Betsmatchday = b2.Matchday,
                           BetResult1 = b2.Result

                       }).OrderBy(m2 => m2.UtcDate).ToList();
            

            return qry;
        }

        public static void InsertBets(string username, int idMatchAPI, int matchday, string result1)
        {
            MySqlConnection connection = new MySqlConnection(GetConnectionString());
            connection.Open();

            MySqlCommand command = new MySqlCommand();

            command = new MySqlCommand("InsertUpdateBets", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.Add(new MySqlParameter("Username", username));
            command.Parameters.Add(new MySqlParameter("IdmatchAPI", idMatchAPI));
            command.Parameters.Add(new MySqlParameter("Matchday", matchday));
            command.Parameters.Add(new MySqlParameter("Result", result1));

            command.ExecuteNonQuery();

            connection.Close();
        }

        internal static List<ScoresUserBets> GetScoresUserBets(int? matchday, ApplicationDbContext dbContext)
        {
            List<ScoresUserBets> qry = new List<ScoresUserBets>();



            qry = (from m in dbContext.Matches
                   where m.Matchday == matchday
                   orderby m.UtcDate
                   select new ScoresUserBets
                   {
                       Awayteam = m.Awayteam,
                       Awayteamgoals = m.Awayteamgoals,
                       Hometeam = m.Hometeam,
                       Hometeamgoals = m.Hometeamgoals,
                       IdMatchAPI = m.IdmatchAPI,
                       Result1 = m.Result1,
                   }).ToList();


            foreach (var item in qry)
            {
                List<UsersBets> betslist = new List<UsersBets>();
                

                    betslist = (from ud2 in
                     (from ud in dbContext.Usersinfo
                      select ud)
                                join b2 in
                                 (from b in dbContext.Bets
                                  where b.IdmatchAPI == item.IdMatchAPI
                                  select b)
                               on ud2.Username equals b2.Username into bGroup
                                from b2 in bGroup.DefaultIfEmpty()
                                select new UsersBets
                                {
                                    Name = ud2.Name,
                                    IdMatch = b2.IdmatchAPI,
                                    GoalsHomeTeam = b2.GoalsHomeTeam,
                                    GoalsAwayTeam = b2.GoalsAwayTeam,
                                    Username = b2.Username,
                                    Result = b2.Result,
                                    Perfect = b2.Perfect,
                                    Score = b2.Score,
                                    Matchday = b2.Matchday
                                }).ToList();

                               item.Userbets = betslist;
            }
            return qry;

        }

        public static string GetConnectionString()
        {
            string connectionString = string.Format(
                "server={0};user={1};password={2};port={3};database={4}",
                new string[]
                {
                    Environment.GetEnvironmentVariable("DB_URL"),
                    Environment.GetEnvironmentVariable("DB_USER"),
                    Environment.GetEnvironmentVariable("DB_PASSWORD"),
                    Environment.GetEnvironmentVariable("DB_PORT"),
                    Environment.GetEnvironmentVariable("DB_DATABASE")
                });

            return connectionString;
            
            
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
