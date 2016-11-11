using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WebprosjektBankOblig.BLL;
using WebprosjektBankOblig.DAL;
using WebprosjektBankOblig.Models;

namespace WebprosjektBankOblig.Controllers
{
    public class LoggInnController : Controller
    {
        public ActionResult Index()
        {
            var mBLL = new AuthBLL();

            mBLL.insertTestData();

            return View();
        }

        public ActionResult BliKunde()
        {
            return View();
        }

        public ActionResult OmOss()
        {
            return View();
        }

        public ActionResult Logginn()
        {
            return View();
        }

        public ActionResult EtterLogginn()
        {
            var personnummer = (string)Session["Personnummer"];

            if (personnummer != null)
            {
                var db = new BankDbContext();

                var kontoer = db.Kontoer.Where(x => x.Kunde.Personnummer.Equals(personnummer));

                return View(kontoer.ToList());
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        public ActionResult LoggUt()
        {
            //Setter session lik null
            Session["loggedInn"] = null;
            Session["Personnummer"] = null;
            Session["engangs"] = null;
            Session["admin"] = null;
            Session["loggetUt"] = true;

            //Redirecter kunden til hovedsiden
            return RedirectToAction("Index", "LoggInn");
        }

        public ActionResult BankIdBrikke(int? id)
        {
            var mDAL = new AuthDAL();

            if (id.HasValue)
            {
                var kunde = mDAL.hentKunde(id.Value);

                if (kunde != null)
                    return View(kunde);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        [HttpGet]
        public JsonResult Personnummer(string personnummer)
        {
            var db = new BankDbContext();

            var mBLL = new AuthBLL();

            bool success = false;

            if (mBLL.kundeEksisterer(personnummer))
            {
                Session["Personnummer"] = personnummer;
                success = true;
            }

            return Json(success, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Engangs(int engangspassord)
        {
            var success = false;

            var pnummer = (string)Session["Personnummer"];

            if (pnummer != null)
            {
                var mBLL = new AuthBLL();

                if (mBLL.validerEngangspassord(pnummer, engangspassord))
                {
                    Session["engangs"] = true;
                    success = true;
                }
            }
            return Json(success, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Passord(string passord)
        {
            var success = false;

            bool hasOTP = (bool)Session["engangs"];
            string pnummer = (string)Session["Personnummer"];

            if (hasOTP)
            {
                var mBLL = new AuthBLL();

                if (mBLL.validerPassord(pnummer, passord))
                {
                    Session["loggedInn"] = true;
                    Session["Loggetfullenavn"] = mBLL.hentKundeNavn(pnummer);
                    Session["loggetUt"] = null;
                    success = true;
                }
            }

            return Json(success, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult getNextOTP(string pnummer)
        {
            var mBLL = new AuthBLL();

            return Json(mBLL.hentNesteEngangspassord(pnummer), JsonRequestBehavior.AllowGet);
        }
    }
}