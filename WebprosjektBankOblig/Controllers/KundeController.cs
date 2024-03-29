﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebprosjektBankOblig.BLL;
using WebprosjektBankOblig.DAL;
using WebprosjektBankOblig.Models;

namespace WebprosjektBankOblig.Controllers
{
    public class KundeController : Controller
    {
        private BankDbContext db = new BankDbContext();

        public ActionResult Index()
        {
            var kunder = db.Kunder.Where(x => !x.slettet);
            return View(kunder.ToList());
        }

        public ActionResult KontoOversikt(int? id)
        {
            var admin = false;

            if (Session["admin"] != null)
            {
                admin = true;
            }

            if (!admin)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var db = new BankDbContext();

            var kunde = db.Kunder.Find(id.Value);

            Session["Personnummer"] = kunde.Personnummer;
            Session["loggedInn"] = true;

            return RedirectToAction("Oversikt", "Konto");
        }

        public ActionResult Detaljer(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kunde kunde = db.Kunder.Find(id);
            if (kunde == null)
            {
                return HttpNotFound();
            }

            ViewBag.Kontoer = kunde.Kontoer.Count();

            return View(kunde);
        }

        public ActionResult Registrer()
        {
            ViewBag.Id = new SelectList(db.Autentiseringer, "Id", "Id");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registrer([Bind(Include = "Id,Personnummer,Navn,Adresse,Tlf,Passord,GjentaPassord")] Kunde kunde)
        {
            if (ModelState.IsValid)
            {
                var salt = AuthBLL.generateSalt(384);
                var auth = new Autentisering
                {
                    PassordSalt = salt,
                    PassordHash = AuthBLL.Hash(kunde.Passord, salt),
                    engangsSeed = AuthBLL.generateSeed(384),
                    engangsIterasjon = 100000
                };

                kunde.Autentisering = auth;

                db.Kunder.Add(kunde);
                db.SaveChanges();

                if (Session["admin"] != null)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("LoggInn", "LoggInn", new { kundeId = kunde.Id });
                }
            }

            ViewBag.Id = new SelectList(db.Autentiseringer, "Id", "Id", kunde.Id);
            return View(kunde);
        }

        const string displaypass = "--------------";

        public ActionResult Endre(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kunde kunde = db.Kunder.Find(id);
            if (kunde == null)
            {
                return HttpNotFound();
            }

            kunde.Passord = displaypass;
            kunde.GjentaPassord = displaypass;

            ViewBag.Id = new SelectList(db.Autentiseringer, "Id", "Id", kunde.Id);
            return View(kunde);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Endre([Bind(Include = "Id,Personnummer,Navn,Adresse,Tlf,Passord,GjentaPassord")] Kunde kunde)
        {
            if (ModelState.IsValid)
            {
                db.Entry(kunde).State = EntityState.Modified;

                if (kunde.Passord != displaypass)
                {
                    var auth = db.Autentiseringer.Find(kunde.Id);
                    auth.PassordHash = AuthBLL.Hash(kunde.Passord, auth.PassordSalt);

                    db.Entry(auth).State = EntityState.Modified;
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Id = new SelectList(db.Autentiseringer, "Id", "Id", kunde.Id);
            return View(kunde);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kunde kunde = db.Kunder.Find(id);
            if (kunde == null)
            {
                return HttpNotFound();
            }
            return View(kunde);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Kunde kunde = db.Kunder.Find(id);
            kunde.slettet = true;
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
