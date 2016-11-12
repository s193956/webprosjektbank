using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebprosjektBankOblig.DAL;
using WebprosjektBankOblig.Models;

namespace WebprosjektBankOblig.Controllers
{
    public class KundeController : Controller
    {
        private BankDbContext db = new BankDbContext();

        // GET: Kunde
        public ActionResult Index()
        {
            var kunder = db.Kunder;
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

        // GET: Kunde/Details/5
        public ActionResult Details(int? id)
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

        // GET: Kunde/Create
        public ActionResult Create()
        {
            ViewBag.Id = new SelectList(db.Autentiseringer, "Id", "Id");
            return View();
        }

        // POST: Kunde/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Personnummer,Navn,Adresse,Tlf")] Kunde kunde)
        {
            if (ModelState.IsValid)
            {
                db.Kunder.Add(kunde);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Id = new SelectList(db.Autentiseringer, "Id", "Id", kunde.Id);
            return View(kunde);
        }

        // GET: Kunde/Edit/5
        public ActionResult Edit(int? id)
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
            ViewBag.Id = new SelectList(db.Autentiseringer, "Id", "Id", kunde.Id);
            return View(kunde);
        }

        // POST: Kunde/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Personnummer,Navn,Adresse,Tlf")] Kunde kunde)
        {
            if (ModelState.IsValid)
            {
                db.Entry(kunde).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Id = new SelectList(db.Autentiseringer, "Id", "Id", kunde.Id);
            return View(kunde);
        }

        // GET: Kunde/Delete/5
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

        // POST: Kunde/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Kunde kunde = db.Kunder.Find(id);
            db.Kunder.Remove(kunde);
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
