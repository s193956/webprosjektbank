using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebprosjektBankOblig.Models;

namespace WebprosjektBankOblig.Controllers
{
    public class BetalingController : Controller
    {
        private DBContext db = new DBContext();

        public ActionResult Index(int? id)
        {
            //Dersom kunden skriver /Betaling/en eller annen action så vil han/hun
            //bli returnert til hovedsiden hvis de ikke er pålogget
            if (Session["loggedInn"] == null)
            {
                return RedirectToAction("Index", "LoggInn");
            }

            var personnummer = (string)Session["Personnummer"];

            if (id != null)
            {
                var betalinger = from b in db.Betalinger
                                 where b.Konto.Kunde.Personnummer.Equals(personnummer) &&
                                 b.Konto.Id == (int)id &&
                                 !b.utført
                                 orderby b.dato
                                 select b;

                return View(betalinger.ToList());
            }
            else
            {
                var betalinger = from b in db.Betalinger
                                 where b.Konto.Kunde.Personnummer.Equals(personnummer) &&
                                 !b.utført
                                 orderby b.dato
                                 select b;

                return View(betalinger.ToList());
            }
        }

        public ActionResult Utforte(int? id)
        {
            //Dersom kunden skriver /Betaling/en eller annen action så vil han/hun
            //bli returnert til hovedsiden hvis de ikke er pålogget
            if(Session["loggedInn"] == null)
            {
                return RedirectToAction("Index","LoggInn");
            }

            var personnummer = (string) Session["Personnummer"];

            if (id != null)
            {
                var betalinger = from b in db.Betalinger
                                 where b.Konto.Kunde.Personnummer.Equals(personnummer) &&
                                 b.Konto.Id == (int)id &&
                                 b.utført
                                 orderby b.dato
                                 select b;

                return View(betalinger.ToList());
            }
            else
            {
                var betalinger = from b in db.Betalinger
                                  where b.Konto.Kunde.Personnummer.Equals(personnummer) &&
                                  b.utført
                                  orderby b.dato
                                  select b;

                return View(betalinger.ToList());
            }
        }

        //Kan sende med null verdi
        public ActionResult Oversikt(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Betaling betaling = db.Betalinger.Find(id);
            if (betaling == null)
            {
                return HttpNotFound();
            }
            return View(betaling);
        }

        public ActionResult Registrer(int? id)
        {
            var personnummer = (string)Session["Personnummer"];

            var kontoer = db.Kontoer.Where(x => x.Kunde.Personnummer.Equals(personnummer)).ToList();

            ViewData.Add("Kontoer", kontoer);
            ViewData.Add("SelectedKonto", id);

            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registrer([Bind(Include = "Id,frakonto,tilkono,dato,beløp,melding")] Betaling betaling)
        {
            if (ModelState.IsValid)
            {
                db.Betalinger.Add(betaling);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(betaling);
        }

        public ActionResult Endre(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Betaling betaling = db.Betalinger.Find(id);
            if (betaling == null)
            {
                return HttpNotFound();
            }
            return View(betaling);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Endre([Bind(Include = "Id,frakonto,tilkono,dato,beløp,melding")] Betaling betaling)
        {
            if (ModelState.IsValid)
            {
                db.Entry(betaling).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(betaling);
        }

        public ActionResult Slett(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Betaling betaling = db.Betalinger.Find(id);
            if (betaling == null)
            {
                return HttpNotFound();
            }
            return View(betaling);
        }

        [HttpPost, ActionName("Slett")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Betaling betaling = db.Betalinger.Find(id);
            db.Betalinger.Remove(betaling);
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
