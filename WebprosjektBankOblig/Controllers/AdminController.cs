﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebprosjektBankOblig.BLL;

namespace WebprosjektBankOblig.Controllers
{
    public class AdminController : Controller
    {

        private IAdminBLL _adminBLL;
        private IAuthBLL _authBLL;
        private IKontoBLL _kontBLL;
        private IBetalingBLL _betBLL;

        public AdminController()
        {
            _adminBLL = new AdminBLL();
            _authBLL = new AuthBLL();
            _kontBLL = new KontoBLL();
            _betBLL = new BetalingBLL();
        }

        public AdminController(IAdminBLL stub, IAuthBLL stub2, IKontoBLL stub3, IBetalingBLL stub4)
        {
            _adminBLL = stub;
            _authBLL = stub2;
            _kontBLL = stub3;
            _betBLL = stub4;
        }

        public ActionResult Index()
        {
            var bruker = _adminBLL.lagAdminBruker();
            
            return View(bruker);
        }

        public ActionResult Kunder()
        {
            var kunder = _adminBLL.hentAlleKunder();

            return View(kunder);
        }

        public ActionResult Kontoer()
        {
            var kontoer = _kontBLL.hentKontoer();

            return View(kontoer);
        }

        public ActionResult Betalinger()
        {
            var betalinger = _betBLL.hentBetalinger();

            return View(betalinger);
        }

        [HttpGet]
        public JsonResult Login(string login, string passord)
        {
            if(_adminBLL.validerPassord(login, passord)){
                Session["admin"] = true;
                return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
            }
            
            return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
        }
    }
}