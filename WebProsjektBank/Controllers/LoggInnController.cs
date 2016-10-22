using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TestOppgave.Models;

namespace TestOppgave.Controllers
{
    public class LoggInnController : Controller
    {
        private BankContext db = new BankContext();

        // GET: LoggInn
        public ActionResult Index()
        {
            return View(db.Logging.ToList());
        }


        // GET: LoggInn/Create
        public ActionResult Logginn()
        {
            return View();
        }

        // POST: LoggInn/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logginn([Bind(Include = "Personnummer,Passord")] LoggInn loggInn)
        {
            if (ModelState.IsValid)
            {
                using (BankContext dv = new BankContext())
                {
                    var v = dv.Kunder.Where(a => a.Personnummer.Equals(loggInn.Personnummer) && a.Passord.Equals(loggInn.Passord)).FirstOrDefault();
                    if (v != null)
                    {
                        Session["LoggetBrukerID"] = v.Fornavn.ToString();
                        Session["Loggetfullenavn"] = v.Etternavn.ToString();
                        return RedirectToAction("EtterLogginn");
                    }
                }
            }
            return View(loggInn);
        }

        public ActionResult EtterLogginn()
        {
            if (Session["LoggetBrukerID"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index");
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
