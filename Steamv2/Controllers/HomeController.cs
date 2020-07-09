using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Steamv2.DAL;
using Steamv2.Models;
using Steamv2.ViewModels;

namespace Steamv2.Controllers
{
    public class HomeController : Controller
    {
        private GameContext db = new GameContext();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            var games = db.Games.GroupBy(g => g.DataRelase.Year).Select(g => new GamesDateGroup
            {
                EnrollmentDate = g.Key,
                GameCount = g.Count()
            });

            return View(games.ToList());
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}