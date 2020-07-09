using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Steamv2.DAL;
using Steamv2.Models;
using System.Net.Mail;

namespace Steamv2.Controllers
{
    public class BasketsController : Controller
    {
        private GameContext db = new GameContext();

        // GET: Baskets
        public ActionResult Index()
        {
            Profile profile = db.Profiles.Single(p => p.UserName == User.Identity.Name);
            var price = profile.Basket.GameList.Sum(g => g.Game.Price);
            ViewBag.CurrentPrice = price;
            if(profile.ProfileFunds <= price)
            {
                ViewBag.FoundsBelowZero = true;
            }
            else
            {
                ViewBag.FoundsBelowZero = false;
            }
            return View(profile.Basket.GameList);
        }

        public ActionResult Delete(int id)
        {
            Profile profile = db.Profiles.Single(p => p.UserName == User.Identity.Name);
            AddBasket removeBasket = profile.Basket.GameList.Find(ab => ab.Id == id);
            profile.Basket.GameList.Remove(removeBasket);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult AcceptOrder()
        {
            Profile profile = db.Profiles.Single(p => p.UserName == User.Identity.Name);
            var price = profile.Basket.GameList.Sum(g => g.Game.Price);
            profile.ProfileFunds -= price;
            if (profile.ProfileFunds <= 0)
            {
                return RedirectToAction("Index");
            }


            var list = profile.Basket.GameList;

            foreach (var item in list)
            {
                OwnedGames ownedGame = new OwnedGames { GameId = item.GameId, ProfileId = profile.Id };
                profile.OwnedGamesList.Add(ownedGame);
            }

            profile.Basket.GameList.Clear();
            
            db.SaveChanges();
            SendMail(profile.UserName, "Buying topic", "You acctualy bought game from KozakStimekv2");
            return RedirectToAction("Index");
        }

        public static void SendMail(string to, string subject, string body)
        {
            var message = new System.Net.Mail.MailMessage(ConfigurationManager.AppSettings["sender"], to)
            {
                Subject = subject,
                Body = body
            };
            var smtpClient = new System.Net.Mail.SmtpClient
            {
                Host = ConfigurationManager.AppSettings["smtpHost"],
                Credentials = new NetworkCredential(
                    ConfigurationManager.AppSettings["sender"],
                    ConfigurationManager.AppSettings["passwd"]),
                EnableSsl = true,
                Port = 587
            };
            smtpClient.Send(message);
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
