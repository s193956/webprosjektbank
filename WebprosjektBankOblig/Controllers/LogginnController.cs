using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WebprosjektBankOblig.Models;

namespace WebprosjektBankOblig.Controllers
{
    public class LoggInnController : Controller
    {

        // GET: LoggInn
        public ActionResult Index()
        {
            return View();
        }

        //Returnerer bare view for nå
        public ActionResult BliKunde()
        {
            return View();
        }

        public ActionResult OmOss()
        {
            return View();
        }

        // GET: LoggInn
        public ActionResult Logginn()
        {
            var nyPoststed = new Poststeder
            {
                Postnummer = 1337,
                Poststed = "Sandvika"
            };

            var salt = generateSalt(384);

            var nyAuth = new Autentisering
            {
                PassordSalt = salt,
                PassordHash = Hash("123", salt),
                engangsSeed = generateSeed(384),
                engangsIterasjon = 1000000
            };

            // Ensure there is a user in the database
            var nyKunde = new Kunde
            {
                Navn = "Frank Frankenstein",
                Adresse = "Osloveien 2",
                Tlf = "87654321",
                Poststed = nyPoststed,
                auth = nyAuth
            };

            var db = new DBContext();

            db.Kunder.Add(nyKunde);
            db.SaveChanges();

            return View();
        }

        private static readonly int NUM__PAST_ITERATIONS_TO_REMEMBER = 200;

        private static byte[] generateSeed(int bits)
        {
            int bytes = bits / 8;

            byte[] returnValue = new byte[bytes];

            Random random = new Random();

            random.NextBytes(returnValue);

            return returnValue;
        }

        //Utestet, vet ikke hvordan den faktisk ser ut
        public static int convertToHumanreadable(byte[] hash)
        {
            // If the system architecture is little-endian (that is, little end first),
            // reverse the byte array.
            if (BitConverter.IsLittleEndian)
                Array.Reverse(hash);

            long temp = BitConverter.ToInt64(hash, 0);

            return (int)(temp % 1000000);
        }

        private byte[] Hash(string password, byte[] salt)
        {
            byte[] bytes = new byte[password.Length * sizeof(char)];
            System.Buffer.BlockCopy(password.ToCharArray(), 0, bytes, 0, bytes.Length);

            SHA384 alg = new SHA384Managed();
            return alg.ComputeHash(Combine(bytes, salt));
        }

        public static byte[] Combine(byte[] first, byte[] second)
        {
            byte[] ret = new byte[first.Length + second.Length];
            Buffer.BlockCopy(first, 0, ret, 0, first.Length);
            Buffer.BlockCopy(second, 0, ret, first.Length, second.Length);
            return ret;
        }

        private byte[] generateSalt(int bits)
        {
            int bytes = bits / 8;

            byte[] returnValue = new byte[bytes];

            Random random = new Random();

            random.NextBytes(returnValue);

            return returnValue;
        }

        [HttpGet]
        public JsonResult Personnummer(string personnummer)
        {
            var db = new DBContext();

            var kunde = db.Kunder.Any(x => x.Personnummer.Equals(personnummer));

            if (kunde)
            {
                Session["personnummer"] = personnummer;
            }

            bool success = kunde;
            return Json(success, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Engangs(int engangspassord)
        {
            var success = false;

            var pnummer = Session["personnummer"];

            if (pnummer != null)
            {
                using (var db = new DBContext())
                {
                    //Finn KundeAutentisering knyttet til kunden
                    var Autentisering = (from a in db.Autentiseringer where a.Kunde.Personnummer.Equals((string)pnummer) select a).Single();

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

                            Session["OTP"] = true;

                            success = true;
                        }
                    }
                    success = false;
                }
            }

            return Json(success, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Passord(string passord)
        {
            var success = false;

            bool hasOTP = (bool)Session["OTP"];
            string pnummer = (string)Session["personnummer"];

            if (hasOTP)
            {
                var db = new DBContext();
                success = db.Autentiseringer.Any(x => x.Kunde.Personnummer.Equals(pnummer) && x.PassordHash.Equals(Hash(passord, x.PassordSalt)));

                if (success)
                {
                    Session["loggedInn"] = true;
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
                db.SaveChanges();

                return Json(convertToHumanreadable(hash), JsonRequestBehavior.AllowGet);
            }
        }
    }
}