using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebprosjektBankOblig.BLL;
using WebprosjektBankOblig.DAL;

namespace WebprosjektBankOblig.Controllers
{
    public class AdminController : Controller
    {

        AdminBLL adminBLL = new AdminBLL();

        private IAdminBLL _adminBLL;

        public AdminController()
        {
            _adminBLL = new AdminBLL();
        }

        public AdminController(IAdminBLL stub)
        {
            _adminBLL = stub;
        }

        public ActionResult Index()
        {
            var bruker = _adminBLL.lagAdminBruker();
            
            return View(bruker);
        }

        public ActionResult Kunder()
        {
            var kunder = new AdminRepository().hentAlleKunder();

            return View(kunder);
        }

        public ActionResult Kontoer()
        {
            var kontoer = new KontoRepository().hentKontoer();

            return View(kontoer);
        }

        public ActionResult Betalinger()
        {
            var betalinger = new BetalingRepository().hentBetalinger();

            return View(betalinger);
        }

        [HttpGet]
        public JsonResult Login(string login, string passord)
        {
            if(adminBLL.validerPassord(login, passord)){
                Session["admin"] = true;
                return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
            }
            
            return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
        }
    }
}