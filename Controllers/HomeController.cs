using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using PenaltyV2.Data;
using PenaltyV2.Models;

namespace PenaltyV2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public readonly ApplicationDbContext _dbContext;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;

        }

        public void MatchdayViewbags(int matchday)
        {
            ViewBag.Message = "Jornada: " + matchday.ToString();
            ViewBag.JornadaAnt = matchday - 1;
            ViewBag.JornadaSeg = matchday + 1;
            ViewBag.UltimaJornada = Database.GetLastMatchDay(_dbContext);
        }



        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (User.Identity.Name != null)
            {
                ViewData["Ligas"] = Database.LoadUserLeagues(User.Identity.Name, _dbContext);
            }

        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Help()
        {
            return View();
        }

        [Authorize]
        public IActionResult SubmitAutoBets()
        {
            ViewBag.Teams = Database.GetSeasonTeams();

            //Dictionary<int, int> teamHierarchy = new Dictionary<int, int>()
            //{
            //    { 242, 1 },
            //    { 211, 2 },
            //    { 225, 3 },
            //    { 222, 4 },
            //    { 227, 5 },
            //    { 214, 6 },
            //    { 224, 7 },
            //    { 221, 8 },
            //    { 215, 9 },
            //    { 231, 10 },
            //    { 212, 11 },
            //    { 217, 12 },
            //    { 216, 13 },
            //    { 234, 14 },
            //    { 228, 15 },
            //    { 762, 16 },
            //    { 218, 17 },
            //    { 226, 18 }
            //};

            //string username = "tbento";

            //Database.SubmitAutoBets(username, teamHierarchy);

            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult SubmitAutoBets(int[] idTeam)
        {
            Dictionary<int, int> teamHierarchy = new Dictionary<int, int>();
            for (int i = 0; i < idTeam.Length; i++)
            {
                teamHierarchy.Add(idTeam[i], i+1 );
            }

            Database.SubmitAutoBets(User.Identity.Name, teamHierarchy);

            ViewBag.Teams = Database.GetSeasonTeams();

            return View();
        }

        public IActionResult Audit(string IdmatchAPI)
        {
            List<string> auditLogs = new List<string>
            {
                "MD5 Hash: " + Database.GetMd5(IdmatchAPI)
            };

            var auditFields = Database.GetMatchDetails(IdmatchAPI);

            if (DateTime.Now > auditFields.UtcDate && !auditFields.Secret)
            {
                auditLogs.Add("");
                auditLogs.Add("ID,Username,IdmatchAPI,Result;");
                auditLogs.AddRange(Database.GetMatchBets(IdmatchAPI));
            }

            ViewBag.AuditLogs = auditLogs;

            return View();
        }

        public IActionResult SubmitEmailBet(string token)
        {
            EmailBet emailBet = Database.GetEmailBetByToken(token);

            bool success = false;
            MatchDetails matchDetails = null;
            if (emailBet != null)
            {
                matchDetails = Database.GetMatchDetails(emailBet.IdmatchAPI);
                success = Database.InsertBets(emailBet.Username, Convert.ToInt32(emailBet.IdmatchAPI), matchDetails.UtcDate, emailBet.Result);
            }

            ViewBag.output = success ? "Aposta [" + emailBet.Result + "]" + " submetida para " + matchDetails.HomeTeam + "-" + matchDetails.AwayTeam + " com sucesso! Podes fechar esta janela." : "ERRO! APOSTA NÃO SUBMETIDA!";

            return View();
        }

        [Authorize]
        public IActionResult UserScores(string league)
        {
            IEnumerable<string> ligas = ViewBag.Ligas;

            if (string.IsNullOrEmpty(league))
            {
                //Se o nome da liga não vier por query string, automaticamente é selecionada a primeira da lista de ligas do utilizador
                league = ligas.First();
            }
            else
            {
                //Se o nome da liga vier por query string, verifica-se se esse nome está na lista das ligas do utilizador  
                if (!ligas.Contains(league))
                {
                    //Se não estiver "limpa-se" o nome da liga e provavelmente não se irá retornar nada da BD
                    league = "Não pertences aqui!";
                }
            }

            List<Userscores> userscores = Database.GetUserscores(_dbContext, league);
            ViewBag.LigaSelecionada = league;
            ViewBag.JornadaSecreta = Convert.ToInt32(Database.GetGlobalConstant("SECRET_MODE_START")); 
            return View(userscores);
        }

        [Authorize]
        public ActionResult TeamMatches(int? matchday)
        {
            if (matchday == null)
            {
                matchday = Database.GetCurrentMatchDay(_dbContext);
            }
            else if (matchday < 1)
            {
                matchday = 1;
            }
            else if (matchday > Database.GetLastMatchDay(_dbContext))
            {
                matchday = Database.GetLastMatchDay(_dbContext);
            }
            MatchdayViewbags((int)matchday);

            List<Matches> qry = Database.GetMatches(_dbContext);


            ViewBag.MatchesDay = (from s in qry orderby s.Matchday select s.Matchday).Distinct();

            var filteredResult = from s in qry
                                 where s.Matchday == matchday
                                 select s;


            return View(filteredResult);
        }

        [Authorize]
        public ActionResult UserBets(int? matchday)
        {
            if (matchday == null)
            {
                matchday = Database.GetCurrentMatchDay(_dbContext);
            }
            else if (matchday < 1)
            {
                matchday = 1;
            }
            else if (matchday > Database.GetLastMatchDay(_dbContext))
            {
                matchday = Database.GetLastMatchDay(_dbContext);
            }
            MatchdayViewbags((int)matchday);

            List<Matches> qry = Database.GetMatches(_dbContext);
            ViewBag.MatchesDay = (from s in qry orderby s.Matchday select s.Matchday).Distinct();

            List<MatchesBets> list2 = Database.GetBetsByUserAndMatchday(matchday, User.Identity.Name, _dbContext);
            if (list2.Count > 0)
            {
                //Basta um jogo da jornada não ter começado para a jornada não estar fechada
                foreach (var item in list2)
                {
                    if (DateTime.Now > item.UtcDate)
                    {
                        ViewBag.JornadaFechada = true;
                    }
                    else
                    {
                        ViewBag.JornadaFechada = false;
                    }
                }

            }

            return View(list2);
        }

        [Authorize]
        [HttpPost]
        public ActionResult UserBets(string[] rbResult, int[] idmatchAPI, DateTime[] utcdate, int matchday)
        {
            MatchdayViewbags(matchday);
            string username = User.Identity.Name;
            try
            {
                for (int i = 0; i < rbResult.Length; i++)
                {
                    Database.InsertBets(username, idmatchAPI[i], utcdate[i], rbResult[i]);
                }
                ViewBag.Sucesso = "Apostas inseridas com sucesso!";
            }
            catch (Exception exp)
            {
                ViewBag.Error = exp.Message;
                throw;
            }

            List<Matches> qry = Database.GetMatches(_dbContext);
            ViewBag.MatchesDay = (from s in qry orderby s.Matchday select s.Matchday).Distinct();

            List<MatchesBets> list2 = Database.GetBetsByUserAndMatchday(matchday, User.Identity.Name, _dbContext);
            if (list2.Count > 0)
            {
                //Basta um jogo da jornada não ter começado para a jornada não estar fechada
                foreach (var item in list2)
                {
                    if (DateTime.Now > item.UtcDate)
                    {
                        ViewBag.JornadaFechada = true;
                    }
                    else
                    {
                        ViewBag.JornadaFechada = false;
                    }
                }

            }

            return View(list2);

        }

        [Authorize]
        public ActionResult Summary(int? matchday, string league)
        {
            IEnumerable<string> ligas = ViewBag.Ligas;
            int secret_mode = Convert.ToInt32(Database.GetGlobalConstant("SECRET_MODE_START"));



            if (string.IsNullOrEmpty(league))
            {
                league = ligas.First();
            }
            else
            {
                //Se o nome da liga vier por query string, verifica-se se esse nome está na lista das ligas do utilizador  
                if (!ligas.Contains(league))
                {
                    //Se não estiver "limpa-se" o nome da liga e provavelmente não se irá retornar nada da BD
                    league = "Não pertences aqui!";
                }
            }
            ViewBag.LigaSelecionada = league;

            if (matchday == null)
            {
                matchday = Database.GetCurrentMatchDay(_dbContext);
            }else if(matchday < 1)
            {
                matchday = 1;
            }
            else if (matchday > Database.GetLastMatchDay(_dbContext))
            {
                matchday = Database.GetLastMatchDay(_dbContext);
            }
            MatchdayViewbags((int)matchday);



            List<Matches> qry = Database.GetMatches(_dbContext);
            ViewBag.MatchesDay = (from s in qry orderby s.Matchday select s.Matchday).Distinct();

            List<ScoresUserBets> list2 = Database.GetScoresUserBets(matchday, league, User.Identity.Name, _dbContext);
            if (list2.Count > 0)
            {
                List<UsersBets> usersbets = list2.First().Userbets;
                List<string> usernames = new List<string>();
                foreach (var item in usersbets)
                {
                    usernames.Add(item.Name);
                }
                ViewBag.Usernames = usernames;
            }

            //Verificar se não está numa jornada em Segredo
            if (matchday >= secret_mode)
            {
                ViewBag.SecretMode = true;
                return View();
            }else
            {
                ViewBag.SecretMode = false;
                return View(list2);
            }      
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}
