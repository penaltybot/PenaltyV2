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
        private readonly ApplicationDbContext _dbContext;

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
            List<Userscores> userscores = GetUserscores();

            return View(userscores);
        }

        [Authorize]
        public ActionResult TeamMatches(int? matchday)
        {

            if (matchday == null)
            {
                //TODO:GetCurrentMatchDay
                matchday = 1;
            }
            ViewBag.Message = "Jornada: " + matchday.ToString();

            List<Matches> qry = GetMatches();


            ViewBag.MatchesDay = (from s in qry orderby s.Matchday select s.Matchday).Distinct();

            var filteredResult = from s in qry
                                 where s.Matchday == matchday
                                 select s;


            return View(filteredResult);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public List<Userscores> GetUserscores()
        {
            List<Userscores> qry = new List<Userscores>();
            qry = (from us in _dbContext.Userscores
                   select new Userscores
                   {
                       Id = us.Id,
                       Username = us.Username,
                       Name = us.Name,
                       Favoriteteam = us.Favoriteteam,
                       Perfects = us.Perfects,
                       Position = us.Position,
                       Competitionyear = us.Competitionyear,
                       Score = us.Score,
                   }).ToList();
            return qry;
        }

        public List<Matches> GetMatches()
        {
            List<Matches> qry = new List<Matches>();

                qry = (from m in _dbContext.Matches
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
    }
}
