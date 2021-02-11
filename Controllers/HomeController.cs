using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult SubmitEmailBet(string token)
        {
            EmailBet emailBet = Database.GetEmailBetByToken(token);

            if (emailBet != null)
            {
            //    Database.InsertBets(emailBet.Username, Convert.ToInt32(emailBet.IdmatchAPI), 0, emailBet.Result);
            }

            return View();
        }

        public IActionResult Help()
        {
            return View();
        }

        public void matchdayViewbags(int matchday)
        {
            ViewBag.Message = "Jornada: " + matchday.ToString();
            ViewBag.JornadaAnt = matchday - 1;
            ViewBag.JornadaSeg = matchday + 1;
        }



        [Authorize]
        public IActionResult UserScores(string league)
        {
            //Só para o caso de ser necessario mais tarde: IEnumerable<string> ligas = ((IEnumerable<string>)(from s in qry orderby s.Id select s.LeagueName));

            string username = User.Identity.Name;
            List<string> userLeagues = Database.GetLeagues(username,_dbContext);          
            IEnumerable<string> ligas = (IEnumerable<string>)userLeagues;
            ViewBag.Ligas = ligas;

            if (string.IsNullOrEmpty(league))
            {
                //Se o nome da liga não vier por query string, automaticamente é selecionada a primeira da lista de ligas do utilizador
                league = ligas.First();
            }else
            {
                //Se o nome da liga vier por query string, verifica-se se esse nome está na lista das ligas do utilizador  
                if(!userLeagues.Contains(league))
                {
                    //Se não estiver "limpa-se" o nome da liga e provavelmente não se irá retornar nada da BD
                    league = "Não pertences aqui!"; 
                }              
            }

            List<Userscores> userscores = Database.GetUserscores(_dbContext, league);
            ViewBag.LigaSelecionada = league;
            return View(userscores);
        }

        [Authorize]
        public ActionResult TeamMatches(int? matchday)
        {

            if (matchday == null)
            {
                matchday = Database.GetCurrentMatchDay(_dbContext);
            }
            matchdayViewbags((int)matchday);

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
            matchdayViewbags((int)matchday);

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
            //Ideia: Mandar o utcdate por parametro também e comparar
            matchdayViewbags(matchday);
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
        public ActionResult Summary(int? matchday)
        {

            if (matchday == null)
            {
                matchday = Database.GetCurrentMatchDay(_dbContext);
            }
            matchdayViewbags((int)matchday);

            List<Matches> qry = Database.GetMatches(_dbContext);
            ViewBag.MatchesDay = (from s in qry orderby s.Matchday select s.Matchday).Distinct();

            List<ScoresUserBets> list2 = Database.GetScoresUserBets(matchday, _dbContext);
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


            return View(list2);

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}
