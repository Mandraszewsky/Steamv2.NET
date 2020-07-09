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

namespace Steamv2.Controllers
{
    public class GameTypesController : Controller
    {
        private GameContext db = new GameContext();

        // GET: GameTypes
        public ActionResult Index()
        {
            return View(db.GameTypes.ToList());
        }

        // GET: GameTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GameType gameType = db.GameTypes.Find(id);
            if (gameType == null)
            {
                return HttpNotFound();
            }
            return View(gameType);
        }

        // GET: GameTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: GameTypes/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] GameType gameType)
        {
            if (ModelState.IsValid)
            {
                db.GameTypes.Add(gameType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(gameType);
        }

        // GET: GameTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GameType gameType = db.GameTypes.Find(id);
            if (gameType == null)
            {
                return HttpNotFound();
            }
            return View(gameType);
        }

        // POST: GameTypes/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] GameType gameType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(gameType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(gameType);
        }

        // GET: GameTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GameType gameType = db.GameTypes.Find(id);
            if (gameType == null)
            {
                return HttpNotFound();
            }
            return View(gameType);
        }

        // POST: GameTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            GameType gameType = db.GameTypes.Find(id);
            db.GameTypes.Remove(gameType);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult ShowGames(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            GameType gameType = db.GameTypes.Find(id);
            if (gameType == null)
            {
                return HttpNotFound();
            }

            var games = db.Games.Where(g => g.GameType.Id == id);
            ViewBag.TypeName = gameType.Name;

            return View(games);
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
