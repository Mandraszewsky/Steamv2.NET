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
    public class GameDLCsController : Controller
    {
        private GameContext db = new GameContext();

        // GET: GameDLCs
        public ActionResult Index()
        {
            var gameDLCs = db.GameDLCs.Include(g => g.Game);
            return View(gameDLCs.ToList());
        }

        // GET: GameDLCs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GameDLC gameDLC = db.GameDLCs.Find(id);
            if (gameDLC == null)
            {
                return HttpNotFound();
            }
            return View(gameDLC);
        }

        // GET: GameDLCs/Create
        public ActionResult Create()
        {
            ViewBag.GameId = new SelectList(db.Games, "Id", "Name");
            return View();
        }

        // POST: GameDLCs/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Price,GameId")] GameDLC gameDLC)
        {
            if (ModelState.IsValid)
            {
                db.GameDLCs.Add(gameDLC);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.GameId = new SelectList(db.Games, "Id", "Name", gameDLC.GameId);
            return View(gameDLC);
        }

        // GET: GameDLCs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GameDLC gameDLC = db.GameDLCs.Find(id);
            if (gameDLC == null)
            {
                return HttpNotFound();
            }
            ViewBag.GameId = new SelectList(db.Games, "Id", "Name", gameDLC.GameId);
            return View(gameDLC);
        }

        // POST: GameDLCs/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Price,GameId")] GameDLC gameDLC)
        {
            if (ModelState.IsValid)
            {
                db.Entry(gameDLC).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.GameId = new SelectList(db.Games, "Id", "Name", gameDLC.GameId);
            return View(gameDLC);
        }

        // GET: GameDLCs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GameDLC gameDLC = db.GameDLCs.Find(id);
            if (gameDLC == null)
            {
                return HttpNotFound();
            }
            return View(gameDLC);
        }

        // POST: GameDLCs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            GameDLC gameDLC = db.GameDLCs.Find(id);
            db.GameDLCs.Remove(gameDLC);
            db.SaveChanges();
            return RedirectToAction("Index");
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
