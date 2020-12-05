using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
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
            List<Userscores> userscores = GetUserscores();

            return View(userscores);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public List<Userscores> GetUserscores()
        {
            List<Userscores> qry = new List<Userscores>();
            qry = (from s in _dbContext.Userscores
                   select new Userscores
                   {
                       Id = s.Id,
                       Username = s.Username,
                       Name = s.Name,
                       Favoriteteam = s.Favoriteteam,
                       Perfects = s.Perfects,
                       Position = s.Position,
                       Competitionyear = s.Competitionyear,
                       Score = s.Score,
                   }).ToList();
            return qry;
        }
    }
}
