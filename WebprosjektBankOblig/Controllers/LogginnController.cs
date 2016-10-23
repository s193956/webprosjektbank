using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WebprosjektBankOblig.Models;

namespace WebprosjektBankOblig.Controllers
{
    public class LoggInnController : Controller
    {

        private DBContext db = new DBContext();

        private static RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider();

        public ActionResult Index()
        {
            insertTestData();
            
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

        public ActionResult EtterLogginn()
        {
            var personnummer = (string) Session["Personnummer"];

            if (personnummer != null)
            {
                var kontoer = db.Kontoer.Where(x => x.Kunde.Personnummer.Equals(personnummer));

                return View(kontoer.ToList());
            }else
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
            Session["loggetUt"] = true;
            //Redirecter kunden til hovedsiden
            return RedirectToAction("Index", "LoggInn");
        }

        public ActionResult Logginn()
        {
            return View();
        }

        public ActionResult BankIdBrikke(int? id)
        {
            if(id != null)
            {
                var Kunde = db.Kunder.FirstOrDefault(x => x.Id == id);

                return View(Kunde);
            }else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

        }

        private void insertTestData()
        {
            using (var db = new DBContext())
            {
                var nyPoststed = new Poststed
                {
                    Navn = "Sandvika",
                    Nummer = 1337
                };

                var salt = generateSalt(384);

                var nyAutentisering = new Autentisering
                {
                    PassordSalt = salt,
                    PassordHash = Hash("123", salt),
                    engangsSeed = generateSeed(384),
                    engangsIterasjon = 100000
                };

                var nyKunde = new Kunde
                {
                    Personnummer = "12345678912",
                    Navn = "Frank Frankenstein",
                    Adresse = "Osloveien 2",
                    Tlf = "87654321",
                    Poststed = nyPoststed
                };

                nyAutentisering.Kunde = nyKunde;
                nyKunde.Autentisering = nyAutentisering;
                db.Poststeder.Add(nyPoststed);
                db.Kunder.Add(nyKunde);
                db.Autentiseringer.Add(nyAutentisering);

                Konto nyKonto1 = new Konto
                {
                    kontotype = "Brukskonto",
                    kontonr = "4325.54.45342",
                    saldo = "32048,54",
                    Kunde = nyKunde
                };

                Konto nyKonto2 = new Konto
                {
                    kontotype = "Sparekonto",
                    kontonr = "4325.54.45237",
                    saldo = "490394,98",
                    Kunde = nyKunde
                };

                Konto nyKonto3 = new Konto
                {
                    kontotype = "Aksjekonto",
                    kontonr = "4325.54.45543",
                    saldo = "4,05",
                    Kunde = nyKunde
                };

                db.Kontoer.Add(nyKonto1);
                db.Kontoer.Add(nyKonto2);
                db.Kontoer.Add(nyKonto3);

                Random r = new Random();

                for (int i = 0;i< 213; i++)
                {
                    Konto konto = null;

                    switch (i % 3)
                    {
                        case 0:
                            konto = nyKonto1;
                            break;
                        case 1: konto = nyKonto2;
                            break;
                        case 2: konto = nyKonto3;
                            break;
                    }

                    if (konto == null)
                        throw new Exception("wtf");

                    DateTime start = new DateTime(2013, 1, 1);
                    int days = (DateTime.Today - start).Days;
                    var dato = start.AddDays(r.Next(days));

                    Betaling nyBetaling = new Betaling
                    {
                        frakonto = konto.kontonr,
                        tilkonto = r.Next(1429, 9894).ToString() + "." + r.Next(12, 98).ToString() + "." + r.Next(31928, 98596).ToString(),
                        dato = dato.ToString(),
                        beløp = r.Next(39, 94578).ToString(),
                        melding = "",
                        utført = true,
                        Konto = konto
                    };
                    db.Betalinger.Add(nyBetaling);
                }

                db.SaveChanges();

            }
        }

        private static readonly int NUM__PAST_ITERATIONS_TO_REMEMBER = 200;

        private static byte[] generateSeed(int bits)
        {
            int bytes = bits / 8;

            byte[] returnValue = new byte[bytes];

            rngCsp.GetBytes(returnValue);

            return returnValue;
        }

        public static int convertToHumanreadable(byte[] hash)
        {
            // If the system architecture is little-endian (that is, little end first),
            // reverse the byte array.
            if (BitConverter.IsLittleEndian)
                Array.Reverse(hash);

            long temp = BitConverter.ToInt64(hash, 0);

            return (int)(Math.Abs(temp % 1000000));
        }

        private static byte[] Hash(string password, byte[] salt)
        {
            byte[] bytes = new byte[password.Length * sizeof(char)];
            System.Buffer.BlockCopy(password.ToCharArray(), 0, bytes, 0, bytes.Length);

            SHA384 alg = new SHA384Managed();
            return alg.ComputeHash(Combine(bytes, salt));
        }

        private static byte[] Combine(byte[] first, byte[] second)
        {
            byte[] ret = new byte[first.Length + second.Length];
            Buffer.BlockCopy(first, 0, ret, 0, first.Length);
            Buffer.BlockCopy(second, 0, ret, first.Length, second.Length);
            return ret;
        }

        private static byte[] generateSalt(int bits)
        {
            int bytes = bits / 8;

            byte[] returnValue = new byte[bytes];

            rngCsp.GetBytes(returnValue);

            return returnValue;
        }

        [HttpGet]
        public JsonResult Personnummer(string personnummer)
        {
            var db = new DBContext();

            var kunde = db.Kunder.Any(x => x.Personnummer.Equals(personnummer));

            if (kunde)
            {
                Session["Personnummer"] = personnummer;
            }

            bool success = kunde;
            return Json(success, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Engangs(int engangspassord)
        {
            var success = false;

            var pnummer = Session["Personnummer"];

            if (pnummer != null)
            {
                using (var db = new DBContext())
                {
                    //Finn KundeAutentisering knyttet til kunden
                    var Autentisering = (from a in db.Autentiseringer where a.Kunde.Personnummer.Equals((string)pnummer) select a).First();

                    //Lag en liste some kan holde 'NUM__PAST_ITERATIONS_TO_REMEMBER' av de foregående iterasjonene, 
                    //for å sjekke om kunden har generert flere passord en den har brukt hos banken
                    List<byte[]> sisteIterasjoner = new List<byte[]>();

                    SHA384 alg = new SHA384Managed();

                    byte[] hash = Autentisering.engangsSeed;

                    //Kalkuler fram til aktuelle iterasjonen
                    for (int i = 0; i < Autentisering.engangsIterasjon; i++)
                    {
                        hash = alg.ComputeHash(hash);

                        //Husk noen iterasjoner tilbake,
                        //i tilfelle kunden har generert fler passord en brukt
                        if (i + NUM__PAST_ITERATIONS_TO_REMEMBER > Autentisering.engangsIterasjon)
                        {
                            sisteIterasjoner.Add(hash);
                        }
                    }

                    //Fordi vi lagret de iterasjonene i motsatt rekkefølge
                    //av hvilken vei vi faktisk bruker opp passordene
                    //så reversenes listen
                    for (int i = sisteIterasjoner.Count() - 1; i > 0; i--)
                    {
                        //Converter hashen til et mindre format, og sammenlign med bruker input 
                        if (convertToHumanreadable(sisteIterasjoner.ElementAt(i)) == engangspassord)
                        {
                            //Passordene stemte

                            //Finn ut hvilken iterasjon vi er på  
                            int validIteration = Autentisering.engangsIterasjon - ((sisteIterasjoner.Count() - 1) - i);

                            //Oppdater databasen og returner sant
                            Autentisering.engangsIterasjon = validIteration - 1;

                            db.SaveChanges();

                            Session["engangs"] = true;

                            success = true;
                        }
                    }
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
                var db = new DBContext();

                var Autentisering = db.Autentiseringer.FirstOrDefault(x => x.Kunde.Personnummer.Equals(pnummer));

                var inputHash = Hash(passord, Autentisering.PassordSalt);

                if (Autentisering.PassordHash.SequenceEqual(inputHash))
                {
                    Session["loggedInn"] = true;
                    Session["Loggetfullenavn"] = Autentisering.Kunde.Navn;
                    success = true;
                }
            }

            return Json(success, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult getNextOTP(string pnummer)
        {
            using (var db = new DBContext())
            {
                //Finn BankIDBrikke knyttet til kunden
                var Auth = db.Autentiseringer.FirstOrDefault(x => x.Kunde.Personnummer.Equals(pnummer));

                //Sjekk om det eksisterer
                if (Auth == null)
                {
                    return Json(0, JsonRequestBehavior.AllowGet);
                }

                SHA384 alg = new SHA384Managed();

                byte[] hash = Auth.engangsSeed;

                //Kalkuler fram til neste iterasjon
                for (int i = 0; i < Auth.engangsIterasjon - 1; i++)
                {
                    hash = alg.ComputeHash(hash);
                }

                //Lagre endringer
                Auth.engangsIterasjon = Auth.engangsIterasjon - 1;


                try
                {
                    db.SaveChanges();
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
                {
                    Exception raise = dbEx;
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            string message = string.Format("{0}:{1}",
                                validationErrors.Entry.Entity.ToString(),
                                validationError.ErrorMessage);
                            // raise a new exception nesting
                            // the current instance as InnerException
                            raise = new InvalidOperationException(message, raise);
                        }
                    }
                    throw raise;
                }

                return Json(convertToHumanreadable(hash), JsonRequestBehavior.AllowGet);
            }
        }
    }
}