using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebprosjektBankOblig.DAL;
using WebprosjektBankOblig.Models;

namespace WebprosjektBankOblig.Controllers
{
    public class KontoController : Controller
    {
        public ActionResult Oversikt(int? kundeId)
        {
            var personnummer = (string)Session["Personnummer"];
            var admin = false;

            if (Session["admin"] != null)
            {
                admin = true;
            }

            if (personnummer != null)
            {
                var db = new BankDbContext();

                var kontoer = db.Kontoer.Where(x => x.Kunde.Personnummer.Equals(personnummer) && !x.slettet);

                return View(kontoer.ToList());
            }
            else if (admin)
            {
                var db = new BankDbContext();

                if (kundeId.HasValue)
                {
                    var kontoer = db.Kontoer;

                    return View(kontoer.Where(x => x.Kunde.Id == kundeId.Value && !x.slettet).ToList());
                }
                else
                {
                    var kontoer = db.Kontoer.Where(x => !x.slettet);

                    return View(kontoer.ToList());
                }
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }


        private BankDbContext db = new BankDbContext();

        public ActionResult Registrer()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registrer([Bind(Include = "Id,kontotype,kontonr,saldo")] Konto konto)
        {
            if (ModelState.IsValid)
            {
                var personnummer = (string)Session["Personnummer"];

                var kunde = db.Kunder.FirstOrDefault(x => x.Personnummer == personnummer);

                konto.Kunde = kunde;

                db.Kontoer.Add(konto);
                db.SaveChanges();
                return RedirectToAction("Oversikt");
            }

            return View(konto);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Konto konto = db.Kontoer.Find(id);
            if (konto == null)
            {
                return HttpNotFound();
            }
            return View(konto);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Konto konto = db.Kontoer.Find(id);
            konto.slettet = true;
            db.SaveChanges();
            return RedirectToAction("Oversikt");
        }
    }
}