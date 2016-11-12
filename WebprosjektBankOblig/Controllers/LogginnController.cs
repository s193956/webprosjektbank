using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WebprosjektBankOblig.BLL;
using WebprosjektBankOblig.Models;

namespace WebprosjektBankOblig.Controllers
{
    public class LoggInnController : Controller
    {
        private IAuthBLL _authBLL;
        private IKontoBLL _kontBLL;

        public LoggInnController()
        {
            _authBLL = new AuthBLL();
            _kontBLL = new KontoBLL();
        }

        public LoggInnController(IAuthBLL stub, IKontoBLL stub2)
        {
            _authBLL = stub;
            _kontBLL = stub2;
        }
        public ActionResult Index()
        {
            _authBLL.insertTestData();

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

        public ActionResult Admin()
        {
            var adminBLL = new AdminBLL();

            var adminbruker = adminBLL.lagAdminBruker();

            return View(adminbruker);
        }

        [HttpGet]
        public JsonResult AdminLogin(string login, string passord)
        {
            var adminBLL = new AdminBLL();

            if (adminBLL.validerPassord(login, passord))
            {
                Session["admin"] = true;
                return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EtterLogginn()
        {
            var personnummer = (string)Session["Personnummer"];

            if (personnummer != null)
            {
                var kontoer = _kontBLL.hentKontoer();

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

        public ActionResult BankIdBrikke(int id)
        {
            if (id != 0)
            {
                var kunde = _authBLL.hentKunde(id);

                if (kunde != null)
                    return View(kunde);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        [HttpGet]
        public JsonResult Personnummer(string personnummer)
        {
            bool success = false;

            if (_authBLL.kundeEksisterer(personnummer))
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
                if (_authBLL.validerEngangspassord(pnummer, engangspassord))
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
            return Json(_authBLL.hentNesteEngangspassord(pnummer), JsonRequestBehavior.AllowGet);
        }

    }
}