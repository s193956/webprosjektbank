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
        
        public ActionResult Index()
        {
            var bruker = adminBLL.lagAdminBruker();
            
            return View(bruker);
        }

        public ActionResult Kunder()
        {
            var kunder = new AdminDAL().hentAlleKunder();

            return View(kunder);
        }

        public ActionResult Kontoer()
        {
            var kontoer = new KontoDAL().hentKontoer();

            return View(kontoer);
        }

        public ActionResult Betalinger()
        {
            var betalinger = new BetalingDAL().hentBetalinger();

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