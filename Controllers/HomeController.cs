﻿using System;
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}
