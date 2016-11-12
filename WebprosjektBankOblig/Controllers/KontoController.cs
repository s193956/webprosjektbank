using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebprosjektBankOblig.DAL;

namespace WebprosjektBankOblig.Controllers
{
    public class KontoController : Controller
    {
        public ActionResult Oversikt(int? kundeId)
        {
            var personnummer = (string)Session["Personnummer"];
            var admin = false;

            if (Session["admin"] != null)
            {
                admin = true;
            }

            if (personnummer != null)
            {
                var db = new BankDbContext();

                var kontoer = db.Kontoer.Where(x => x.Kunde.Personnummer.Equals(personnummer));

                return View(kontoer.ToList());
            }else if (admin)
            {
                var db = new BankDbContext();

                if (kundeId.HasValue)
                {
                    var kontoer = db.Kontoer;

                    return View(kontoer.Where(x => x.Kunde.Id == kundeId.Value).ToList());
                }
                else
                {
                    var kontoer = db.Kontoer;

                    return View(kontoer.ToList());
                }
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }
    }
}