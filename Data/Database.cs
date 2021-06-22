using MySql.Data.MySqlClient;
using PenaltyV2.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PenaltyV2.Data
{
    public class EmailBet
    {
        public string Username { get; set; }
        public string IdmatchAPI { get; set; }
        public string Result { get; set; }
    }

    public class MatchDetails
    {
        public string HomeTeam { get; set; }
        public string AwayTeam { get; set; }
        public DateTime UtcDate { get; set; }
        public bool Secret { get; set; }
    }

    public class Database
    {
        public static List<Userscores> GetUserscores(ApplicationDbContext dbContext, string league)
        {
            //TODO: Tenho de ir buscar o competition year
            string league_id = GetGlobalConstant("LEAGUE_ID");
            string acumulative_matchday = GetGlobalConstant("SECRET_MODE_START");
            List<Userscores> qry = new List<Userscores>();
            //Warning: Cuidado com o contains, se houver 2 ligas chamadas FCT e FCT2 por exemplo, quem tiver na FCT vai poder ver FCT2
            qry = (from ucs in dbContext.UsersCumulativeScores
                   where ucs.LeagueID == league_id && ucs.Matchday == acumulative_matchday
                   join ui in dbContext.Usersinfo
                   on ucs.Username equals ui.Username
                   where ui.Leagues.Contains(league)
                   orderby ucs.Score descending
                   select new Userscores
                   {
                       Username = ucs.Username,
                       Name = ui.Name,
                       Favoriteteam = ui.Favoriteteam,
                       Perfects = ucs.CorrectPredictions,
                       Score = (decimal)ucs.Score,
                       UserImg = ui.UserImg
                   }).ToList();

            return qry;
        }
        internal static IEnumerable<string> LoadUserLeagues(string username, ApplicationDbContext dbContext)
        {
            //Só para o caso de ser necessario mais tarde: IEnumerable<string> ligas = ((IEnumerable<string>)(from s in qry orderby s.Id select s.LeagueName));
            try
            {
                List<string> userLeagues = Database.GetLeagues(username, dbContext);
                return (IEnumerable<string>)userLeagues;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal static List<string> GetLeagues(string username, ApplicationDbContext dbContext)
        {
            //Melhorar isto para ir antes à tabela dos users
            Usersinfo qry = new Usersinfo();
            List<String> userLeagues = new List<string>();
            qry = (from u in dbContext.Usersinfo
                   where u.Username == username
                   select new Usersinfo
                   {
                       Leagues = u.Leagues
                   }).FirstOrDefault();
            if (qry.Leagues != null)
            {
                userLeagues = qry.Leagues.Split(';').ToList();
            }
            else
            {
                userLeagues.Add("---");
            }


            return userLeagues;

        }

        public static List<Teams> GetTeams(ApplicationDbContext dbContext)
        {
            List<Teams> qry = new List<Teams>();

            qry = (from t in dbContext.Teams
                   select new Teams
                   {
                       TeamId = t.TeamId,
                       Name = t.Name
                   }).ToList();

            return qry;
        }

        public static string GetGlobalConstant(string constant)
        {
            MySqlConnection connection = new MySqlConnection(GetConnectionString());
            connection.Open();
            MySqlCommand globalConstantCommand = new MySqlCommand("GetGlobalConstants", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            MySqlDataReader globalConstantReader = globalConstantCommand.ExecuteReader();

            while (globalConstantReader.Read())
            {
                if (globalConstantReader["Constant"].ToString().Equals(constant))
                {
                    return globalConstantReader["Value"].ToString();
                }
            }
            connection.Close();
            return null;
        }

        internal static void SubmitAutoBets(string username, Dictionary<int, int> teamHierarchy)
        {
            MySqlConnection connection = new MySqlConnection(GetConnectionString());
            connection.Open();

            string leagueId = GetGlobalConstant("LEAGUE_ID");

            MySqlCommand getAvailableFixturesForAutoBetsCommand = new MySqlCommand("GetAvailableFixturesForAutoBets", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            getAvailableFixturesForAutoBetsCommand.Parameters.Add(new MySqlParameter("LeagueID", leagueId));

            MySqlDataReader getAvailableFixturesForAutoBetsReader = getAvailableFixturesForAutoBetsCommand.ExecuteReader();

            while (getAvailableFixturesForAutoBetsReader.Read())
            {
                int idmatchAPI = getAvailableFixturesForAutoBetsReader.GetInt32("IdmatchAPI");
                DateTime utcDate = getAvailableFixturesForAutoBetsReader.GetDateTime("UtcDate");
                int idhometeam = getAvailableFixturesForAutoBetsReader.GetInt32("Idhometeam");
                int idawayteam = getAvailableFixturesForAutoBetsReader.GetInt32("Idawayteam");

                string result = ComputeAutoBet(teamHierarchy, idhometeam, idawayteam);

                if (!result.Equals("X"))
                {
                    InsertBets(username, idmatchAPI, utcDate, result);
                }
            }

            getAvailableFixturesForAutoBetsReader.Close();
            connection.Close();
        }

        private static string ComputeAutoBet(Dictionary<int, int> teamHierarchy, int idhometeam, int idawayteam)
        {
            if (!teamHierarchy.TryGetValue(idhometeam, out int homeTeamPlace))
            {
                return "X";
            }
            if (!teamHierarchy.TryGetValue(idawayteam, out int awayTeamPlace))
            {
                return "X";
            }

            if (homeTeamPlace > awayTeamPlace)
            {
                return "A";
            }
            else if (homeTeamPlace < awayTeamPlace)
            {
                return "H";
            }
            else if (homeTeamPlace == awayTeamPlace)
            {
                return "D";
            }
            else
            {
                return "X";
            }
        }

        public static List<Teams> GetSeasonTeams()
        {
            MySqlConnection connection = new MySqlConnection(GetConnectionString());
            connection.Open();

            string leagueId = GetGlobalConstant("LEAGUE_ID");

            MySqlCommand getSeasonTeamsCommand = new MySqlCommand("GetSeasonTeams", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            getSeasonTeamsCommand.Parameters.Add(new MySqlParameter("LeagueID", leagueId));

            MySqlDataReader getSeasonTeamsReader = getSeasonTeamsCommand.ExecuteReader();
            List<Teams> teams = new List<Teams>();
            while (getSeasonTeamsReader.Read())
            {
                teams.Add(new Teams()
                {
                    TeamId = getSeasonTeamsReader.GetString("TeamId"),
                    Name = getSeasonTeamsReader.GetString("Name"),
                    LogoUri = getSeasonTeamsReader.GetString("LogoUri")
                });

            }

            getSeasonTeamsReader.Close();
            connection.Close();

            return teams;
        }

        public static List<Matches> GetMatches(ApplicationDbContext dbContext)
        {
            List<Matches> qry = new List<Matches>();

            qry = (from m in dbContext.Matches
                   orderby m.UtcDate
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

        public static List<TStands> GetTeamsStandings(ApplicationDbContext dbContext)
        {
            List<TStands> qry = new List<TStands>();

            qry = (from ts in dbContext.TeamsStandings
                   orderby ts.Rank
                   join t in dbContext.Teams
                   on ts.TeamID equals t.TeamId
                   select new TStands
                   {
                       Team = new Teams 
                       { 
                           Id = t.Id,
                           Name = t.Name,
                           LogoUri = t.LogoUri,
                           TeamId = t.TeamId                           
                       },
                       Stand = new TeamsStandings
                       {
                           Id = ts.Id,
                           Rank = ts.Rank,
                           Points = ts.Points,
                           TeamID = ts.TeamID,
                           MatchesPlayed = ts.MatchesPlayed,
                           Wins = ts.Wins,
                           Draws = ts.Draws,
                           Losses = ts.Losses,
                           GoalsFor = ts.GoalsFor,
                           GoalsAgainst = ts.GoalsAgainst,
                           Form = ts.Form
                       }
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
            //Quando ja nao ha jornadas
            if(qry.Count == 0)
            {
                return 1;
            }
            Matches match = qry.First();

            return (int)match.Matchday;
        }

        public static int GetLastMatchDay(ApplicationDbContext dbContext)
        {
            List<Matches> qry = new List<Matches>();
            qry = (from m in dbContext.Matches 
                   orderby m.Matchday descending
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
                       Multiplier = m2.Multiplier,
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

        public static bool InsertBets(string username, int idMatchAPI, DateTime utcdate, string result1)
        {
            if (DateTime.Now < utcdate)
            {
                MySqlConnection connection = new MySqlConnection(GetConnectionString());
                connection.Open();

                MySqlCommand command = new MySqlCommand("InsertUpdateBets", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.Add(new MySqlParameter("Username", username));
                command.Parameters.Add(new MySqlParameter("IdmatchAPI", idMatchAPI));
                command.Parameters.Add(new MySqlParameter("Result", result1));

                command.ExecuteNonQuery();

                connection.Close();

                return true;
            }

            return false;
        }

        public static EmailBet GetEmailBetByToken(string token)
        {
            MySqlConnection connection = new MySqlConnection(GetConnectionString());
            connection.Open();

            MySqlCommand getEmailBetByTokenCommand = new MySqlCommand("GetEmailBetByToken", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            getEmailBetByTokenCommand.Parameters.Add(new MySqlParameter("Token", token));

            MySqlDataReader getEmailBetByTokenReader = getEmailBetByTokenCommand.ExecuteReader();

            EmailBet emailBet = null;
            if (getEmailBetByTokenReader.HasRows)
            {
                getEmailBetByTokenReader.Read();

                emailBet = new EmailBet()
                {
                    Username = Convert.ToString(getEmailBetByTokenReader["Username"]),
                    IdmatchAPI = Convert.ToString(getEmailBetByTokenReader["IdmatchAPI"]),
                    Result = Convert.ToString(getEmailBetByTokenReader["Result"])
                };
            }

            getEmailBetByTokenReader.Close();

            return emailBet;
        }

        public static MatchDetails GetMatchDetails(string IdmatchAPI)
        {
            MySqlConnection connection = new MySqlConnection(GetConnectionString());
            connection.Open();

            MySqlCommand getMatchDetailsCommand = new MySqlCommand("GetMatchDetails", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            getMatchDetailsCommand.Parameters.Add(new MySqlParameter("IdmatchAPI", IdmatchAPI));

            MySqlDataReader getMatchDetailsReader = getMatchDetailsCommand.ExecuteReader();

            MatchDetails matchDetails = null;
            if (getMatchDetailsReader.HasRows)
            {
                getMatchDetailsReader.Read();

                matchDetails = new MatchDetails()
                {
                    HomeTeam = Convert.ToString(getMatchDetailsReader["Hometeam"]),
                    AwayTeam = Convert.ToString(getMatchDetailsReader["Awayteam"]),
                    UtcDate = Convert.ToDateTime(getMatchDetailsReader["UtcDate"]),
                    Secret = Convert.ToBoolean(getMatchDetailsReader["Secret"])
                };
            }

            getMatchDetailsReader.Close();

            return matchDetails;
        }

        public static DateTime GetMatchTime(string IdmatchAPI)
        {
            MySqlConnection connection = new MySqlConnection(GetConnectionString());
            connection.Open();

            MySqlCommand getMatchTimeCommand = new MySqlCommand("GetMatchTime", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            getMatchTimeCommand.Parameters.Add(new MySqlParameter("IdmatchAPI", IdmatchAPI));

            MySqlDataReader getMatchTimeReader = getMatchTimeCommand.ExecuteReader();

            DateTime matchTime = DateTime.MaxValue;
            if (getMatchTimeReader.HasRows)
            {
                getMatchTimeReader.Read();

                matchTime = Convert.ToDateTime(getMatchTimeReader["UtcDate"]);
            }

            getMatchTimeReader.Close();

            return matchTime;
        }

        public static string GetMd5(string IdmatchAPI)
        {
            MySqlConnection connection = new MySqlConnection(GetConnectionString());
            connection.Open();

            MySqlCommand getMd5MatchBetsCommand = new MySqlCommand("GetMd5MatchBets", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            getMd5MatchBetsCommand.Parameters.Add(new MySqlParameter("IdmatchAPI", IdmatchAPI));

            MySqlDataReader getMd5MatchBetsReader = getMd5MatchBetsCommand.ExecuteReader();

            string md5 = null;
            if (getMd5MatchBetsReader.HasRows)
            {
                getMd5MatchBetsReader.Read();

                md5 = Convert.ToString(getMd5MatchBetsReader["MD5"]);
            }

            getMd5MatchBetsReader.Close();

            return md5;
        }

        public static List<string> GetMatchBets(string IdmatchAPI)
        {
            MySqlConnection connection = new MySqlConnection(GetConnectionString());
            connection.Open();

            MySqlCommand getMatchBetsCommand = new MySqlCommand("GetMatchBets", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            getMatchBetsCommand.Parameters.Add(new MySqlParameter("IdmatchAPI", IdmatchAPI));

            MySqlDataReader getMatchBetsReader = getMatchBetsCommand.ExecuteReader();

            List<string> matchLogs = new List<string>();
            while (getMatchBetsReader.Read())
            {
                matchLogs.Add(getMatchBetsReader.GetString("MatchLogs"));
            }

            getMatchBetsReader.Close();

            return matchLogs;
        }

        internal static List<ScoresUserBets> GetScoresUserBets(int? matchday, string league, string username, ApplicationDbContext dbContext)
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
                       UtcDate = (DateTime)m.UtcDate,
                       Result1 = m.Result1,
                   }).ToList();


            foreach (var item in qry)
            {
                List<UsersBets> betslist = new List<UsersBets>();

                //Warning: Cuidado com o contains, se houver 2 ligas chamadas FCT e FCT2 por exemplo, quem tiver na FCT vai poder ver FCT2
                betslist = (from ud2 in
                 (from ud in dbContext.Usersinfo
                  where ud.Leagues.Contains(league)
                  select ud)
                            join b2 in
                             (from b in dbContext.Bets
                              where b.IdmatchAPI == item.IdMatchAPI
                              select b)
                           on ud2.Username equals b2.Username into bGroup
                            from b2 in bGroup.DefaultIfEmpty()
                            orderby (ud2.Name) ascending
                            orderby (b2.Username == username) descending
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

        internal static Usersinfo GetUserInfo(string username, ApplicationDbContext dbContext)
        {
            Usersinfo qry = new Usersinfo();
            List<String> userLeagues = new List<string>();
            qry = (from u in dbContext.Usersinfo
                   where u.Username == username
                   select new Usersinfo
                   {
                       Id = u.Id,
                       Name = u.Name,
                       Username = u.Username,
                       Favoriteteam = u.Favoriteteam,
                       Notifications = u.Notifications,
                       Leagues = u.Leagues,
                       UserImg = u.UserImg
                   }).FirstOrDefault();
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
