using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Steamv2.DAL;
using Steamv2.Models;
using PagedList;
using System.IO;
using System.Data.Entity.Migrations;

namespace Steamv2.Controllers
{
    public class GamesController : Controller
    {
        private GameContext db = new GameContext();

        // GET: Games
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var games = db.Games.Include(g => g.GameType);

            if (!String.IsNullOrEmpty(searchString))
            {
                games = games.Where(s => s.Name.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    games = games.OrderByDescending(g => g.Name);
                    break;
                case "Date":
                    games = games.OrderBy(g => g.DataRelase);
                    break;
                case "date_desc":
                    games = games.OrderByDescending(g => g.DataRelase);
                    break;
                default:
                    games = games.OrderBy(g => g.Name);
                    break;
            }

            if (Request.IsAuthenticated)
            {
                Profile profile = db.Profiles.Single(p => p.UserName == User.Identity.Name);
                ViewBag.CurrentFounds = profile.ProfileFunds;
            }

            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(games.ToPagedList(pageNumber, pageSize));
        }

        // GET: Games/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Game game = db.Games.Find(id);
            if (game == null)
            {
                return HttpNotFound();
            }

            Profile profile = db.Profiles.Single(p => p.UserName == User.Identity.Name);

            ViewBag.Liked = db.Profiles.Single(p => p.UserName == User.Identity.Name).FavoriteGamesList.Any(g => g.GameId == id);
            ViewBag.Owned = db.Profiles.Single(p => p.UserName == User.Identity.Name).OwnedGamesList.Any(g => g.GameId == id);
            ViewBag.Voted = db.Ratings.Any(r => r.GameId == id && r.ProfileId == profile.Id);
            ViewBag.VotedResult = db.Ratings.Any(r => r.GameId == id && r.ProfileId == profile.Id && r.Vote == 1);

            var counterAll = db.Ratings.Count(r => r.GameId == id);
            var counterPositive = db.Ratings.Count(r => r.GameId == id && r.Vote == 1);
            var counterNegative = db.Ratings.Count(r => r.GameId == id && r.Vote == 0);
            double counterInProcent = ( (double)counterPositive / (double)counterAll) * 100;
            if(counterAll <= 0)
            {
                ViewData["PositiveProcent"] = null;
            }
            else
            {
                ViewData["PositiveProcent"] = counterInProcent;
            }
            return View(game);
        }

        public ActionResult OwnedDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Game game = db.Games.Find(id);
            var gamesdlc = game.GameDLC;
            if (game == null)
            {
                return HttpNotFound();
            }

            return View(gamesdlc);
        }

        // GET: Games/Create
        public ActionResult Create()
        {
            ViewBag.GameTypeId = new SelectList(db.GameTypes, "Id", "Name");
            return View();
        }

        // POST: Games/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description,Developer,DataRelase,Price,GameTypeId,File,File2,File3,YouTubeLink")] Game game)
        {
            if (ModelState.IsValid)
            {
                HttpPostedFileBase file = Request.Files["File"];
                if (file != null && file.ContentLength > 0)
                {
                    game.Image = file.FileName;
                    string filepath = Path.Combine(Server.MapPath("~/Images"), file.FileName);
                    file.SaveAs(filepath);
                }
                HttpPostedFileBase file2 = Request.Files["File2"];
                if (file2 != null && file.ContentLength > 0)
                {
                    game.Image2 = file2.FileName;
                    string filepath2 = Path.Combine(Server.MapPath("~/Images"), file2.FileName);
                    file2.SaveAs(filepath2);
                }
                HttpPostedFileBase file3 = Request.Files["File3"];
                if (file3 != null && file.ContentLength > 0)
                {
                    game.Image3 = file3.FileName;
                    string filepath3 = Path.Combine(Server.MapPath("~/Images"), file3.FileName);
                    file3.SaveAs(filepath3);
                }
                db.Games.Add(game);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.GameTypeId = new SelectList(db.GameTypes, "Id", "Name", game.GameTypeId);
            return View(game);
        }

        // GET: Games/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Game game = db.Games.Find(id);
            if (game == null)
            {
                return HttpNotFound();
            }
            ViewBag.GameTypeId = new SelectList(db.GameTypes, "Id", "Name", game.GameTypeId);
            return View(game);
        }

        // POST: Games/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,Developer,DataRelase,Price,GameTypeId,File,Image,Image2,Image3,YouTubeLink")] Game game)
        {
            if (ModelState.IsValid)
            {
                db.Entry(game).State = EntityState.Modified;

                HttpPostedFileBase file = Request.Files["File"];
                if (file != null && file.ContentLength > 0)
                {
                    game.Image = file.FileName;
                    string filepath = Path.Combine(Server.MapPath("~/Images"), file.FileName);
                    file.SaveAs(filepath);
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.GameTypeId = new SelectList(db.GameTypes, "Id", "Name", game.GameTypeId);
            return View(game);
        }

        // GET: Games/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Game game = db.Games.Find(id);
            if (game == null)
            {
                return HttpNotFound();
            }
            return View(game);
        }

        // POST: Games/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Game game = db.Games.Find(id);
            db.Games.Remove(game);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Buy(int id)
        {
            Game game = db.Games.Find(id);
            if (game == null)
            {
                return HttpNotFound();
            }

            Profile profile = db.Profiles.Single(p => p.UserName == User.Identity.Name);
            AddBasket addBasket = new AddBasket { GameId = id, BasketId = profile.Basket.Id};
            profile.Basket.GameList.Add(addBasket);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult AddToFavorite(int id)
        {
            Game game = db.Games.Find(id);
            if (game == null)
            {
                return HttpNotFound();
            }

            Profile profile = db.Profiles.Single(p => p.UserName == User.Identity.Name);
            FavoriteGames favoriteGame = new FavoriteGames { GameId = id, ProfileId = profile.Id };
            profile.FavoriteGamesList.Add(favoriteGame);
            db.SaveChanges();

            return RedirectToAction("Details", new { id = id });
        }

        public ActionResult RemoveFromFavorite(int id)
        {
            Game game = db.Games.Find(id);
            if (game == null)
            {
                return HttpNotFound();
            }

            Profile profile = db.Profiles.Single(p => p.UserName == User.Identity.Name);
            FavoriteGames removeFavoriteGame = profile.FavoriteGamesList.Find(fg => fg.GameId == id);
            profile.FavoriteGamesList.Remove(removeFavoriteGame);

            db.SaveChanges();

            return RedirectToAction("Details", new { id = id });
        }

        public ActionResult ShowFavorites()
        {

            Profile profile = db.Profiles.Single(p => p.UserName == User.Identity.Name);

            return View(profile.FavoriteGamesList);
        }

        public ActionResult ShowOwnedGames()
        {

            Profile profile = db.Profiles.Single(p => p.UserName == User.Identity.Name);

            return View(profile.OwnedGamesList);
        }

        public ActionResult AddFounds()
        {

            Profile profile = db.Profiles.Single(p => p.UserName == User.Identity.Name);
            profile.ProfileFunds += 50;
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult AddComment(int id, string content)
        {
            Profile profile = db.Profiles.Single(p => p.UserName == User.Identity.Name);
            Comment comment = new Comment { Content = content, GameId = id, ContentDate = DateTime.Now, UserName = profile.UserName };

            db.Comments.Add(comment);
            db.SaveChanges();
            return RedirectToAction("Details", new { id = id });
        }

        public ActionResult AddRate(int id, int content)
        {
            Profile profile = db.Profiles.Single(p => p.UserName == User.Identity.Name);
            var checkRating = db.Ratings.Any(r => r.GameId == id && r.ProfileId == profile.Id);

            if (checkRating)
            {
                var rate = db.Ratings.Single(r => r.GameId == id && r.ProfileId == profile.Id);
                rate.Vote = content;
            }
            else
            {
                Rating rate = new Rating { Vote = content, GameId = id, ProfileId = profile.Id };
                db.Ratings.Add(rate);
            }

            db.SaveChanges();
            return RedirectToAction("Details", new { id = id });
        }

        public PartialViewResult ShowBasketNumber()
        {
            if (Request.IsAuthenticated)
            {
                Profile profile = db.Profiles.Single(p => p.UserName == User.Identity.Name);
                int? howMany = profile.Basket.GameList.Count;
                return PartialView("_BasketPartial", howMany);
            }
            else
            {
                return PartialView("_ShowBasketNumber");
            }
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
