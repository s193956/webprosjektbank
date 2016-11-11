using System;
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
    public class BetalingController : Controller
    {
        private BankDbContext db = new BankDbContext();


        private IBetalingBLL _betBLL;

        public BetalingController()
        {
            _betBLL = new BetalingBLL();
        }

        public BetalingController(IBetalingBLL stub)
        {
            _betBLL = stub;
        }

        public ActionResult Index(int? id)
        {
            //Dersom kunden skriver /Betaling/en eller annen action så vil han/hun
            //bli returnert til hovedsiden hvis de ikke er pålogget
            if (Session["loggedInn"] == null)
            {
                return RedirectToAction("Index", "LoggInn");
            }

            var personnummer = (string)Session["Personnummer"];


            return View(_betBLL.hentBetalinger(personnummer, id, false));

        }

        public ActionResult Utforte(int? id)
        {
            //Dersom kunden skriver /Betaling/en eller annen action så vil han/hun
            //bli returnert til hovedsiden hvis de ikke er pålogget
            if (Session["loggedInn"] == null)
            {
                return RedirectToAction("Index", "LoggInn");
            }

            var personnummer = (string)Session["Personnummer"];

            return View(_betBLL.hentBetalinger(personnummer, id, true));

        }

        //Kan sende med null verdi
        public ActionResult Oversikt(int? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var mDAL = new BetalingRepository();

            var betaling = mDAL.hentBetaling(id.Value);

            if (betaling == null)
            {
                return HttpNotFound();
            }

            return View(betaling);
        }

        public ActionResult Registrer(int? id)
        {
            var personnummer = (string)Session["Personnummer"];

            var mDAL = new KontoRepository();

            var kontoer = mDAL.hentKontoer(personnummer);

            ViewData.Add("Kontoer", kontoer);
            ViewData.Add("SelectedKonto", id);

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registrer([Bind(Include = "Id,frakonto,tilkonto,dato,beløp,melding")] Betaling betaling)
        {
            if (ModelState.IsValid)
            {
                _betBLL.registrerBetaling(betaling);

                return RedirectToAction("Index");
            }

            var personnummer = (string)Session["Personnummer"];
            
            var mDAL = new KontoRepository();

            var kontoer = mDAL.hentKontoer(personnummer);

            ViewData.Add("Kontoer", kontoer);

            return View(betaling);
        }

        public ActionResult Endre(int? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var mDAL = new BetalingRepository();

            var betaling = mDAL.hentBetaling(id.Value);
            
            if (betaling == null)
            {
                return HttpNotFound();
            }

            return View(betaling);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Endre([Bind(Include = "Id,frakonto,tilkonto,dato,beløp,melding")] Betaling betaling)
        {
            if (ModelState.IsValid)
            {
                var mDAL = new BetalingRepository();

                mDAL.endreBetaling(betaling);
                
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

            var mDAL = new BetalingRepository();

            var betaling = mDAL.hentBetaling(id.Value);
            
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
            var mDAL = new BetalingRepository();

            mDAL.slettBetaling(id);

            return RedirectToAction("Index");
        }
    }
}
