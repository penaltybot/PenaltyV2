using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        [Authorize]
        public IActionResult UserScores()
        {

            List<Userscores> userscores = Database.GetUserscores(_dbContext);

            return View(userscores);
        }

        [Authorize]
        public ActionResult TeamMatches(int? matchday)
        {

            if (matchday == null)
            {
                matchday = Database.GetCurrentMatchDay(_dbContext);
            }
            ViewBag.Message = "Jornada: " + matchday.ToString();

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
            ViewBag.Message = "Jornada: " + matchday.ToString();

            List<Matches> qry = Database.GetMatches(_dbContext);
            ViewBag.MatchesDay = (from s in qry orderby s.Matchday select s.Matchday).Distinct();

            List<MatchesBets> list2 = Database.GetBetsByUserAndMatchday(matchday, User.Identity.Name, _dbContext);
            if (list2.Count > 0)
            {
                DateTime UtcDate = (DateTime)list2.First().UtcDate;
                //TODO: Colocar as horas no webconfig
                if (DateTime.Today.AddHours(6.00) > UtcDate)
                {
                    ViewBag.JornadaFechada = true;
                }
                else
                {
                    ViewBag.JornadaFechada = false;
                }
            }

            return View(list2);
        }

        [Authorize]
        [HttpPost]
        public ActionResult UserBets(string[] rbResult, int[] idmatchAPI, int[] matchday)
        {
            ViewBag.Message = "Jornada: " + matchday[0].ToString();
            string username = User.Identity.Name;
            try
            {
                for (int i = 0; i < rbResult.Length; i++)
                {
                    Database.InsertBets(username, idmatchAPI[i], matchday[i], rbResult[i]);
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

            List<MatchesBets> list2 = Database.GetBetsByUserAndMatchday(matchday[0], User.Identity.Name, _dbContext);
            if (list2.Count > 0)
            {
                DateTime UtcDate = (DateTime)list2.First().UtcDate;
                //TODO: Colocar as horas no webconfig
                if (DateTime.Today.AddHours(6) > UtcDate)
                {
                    ViewBag.JornadaFechada = true;
                }
                else
                {
                    ViewBag.JornadaFechada = false;
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
            ViewBag.Message = "Jornada: " + matchday.ToString();

            List<Matches> qry = Database.GetMatches(_dbContext);
            ViewBag.MatchesDay = (from s in qry orderby s.Matchday select s.Matchday).Distinct();

            List<ScoresUserBets> list2 = Database.GetScoresUserBets(matchday,_dbContext);
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
