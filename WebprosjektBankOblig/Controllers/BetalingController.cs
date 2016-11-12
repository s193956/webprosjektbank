using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebprosjektBankOblig.BLL;
using WebprosjektBankOblig.Models;

namespace WebprosjektBankOblig.Controllers
{
    public class BetalingController : Controller
    {
        private IBetalingBLL _betBLL;
        private IKontoBLL _kontBLL;

        public BetalingController()
        {
            _betBLL = new BetalingBLL();
            _kontBLL = new KontoBLL();
        }

        public BetalingController(IBetalingBLL stub, IKontoBLL stub2)
        {
            _betBLL = stub;
            _kontBLL = stub2;
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

            var betaling = _betBLL.hentBetaling(id.Value);

            if (betaling == null)
            {
                return HttpNotFound();
            }

            return View(betaling);
        }

        public ActionResult Registrer(int? id)
        {
            var personnummer = (string)Session["Personnummer"];


            var kontoer = _kontBLL.hentKontoer(personnummer);

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
            
            var kontoer = _kontBLL.hentKontoer(personnummer);

            ViewData.Add("Kontoer", kontoer);

            return View(betaling);
        }

        public ActionResult Endre(int? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var betaling = _betBLL.hentBetaling(id.Value);
            
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
                _betBLL.endreBetaling(betaling);
                
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

            var betaling = _betBLL.hentBetaling(id.Value);
            
            if (betaling == null)
            {
                return HttpNotFound();
            }
            return View(betaling);
        }

        [HttpPost, ActionName("Slett")]
        public ActionResult DeleteConfirmed(int id)
        {
            _betBLL.slettBetaling(id);

            return RedirectToAction("Index");
        }
    }
}
