using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using WebprosjektBankOblig.DAL;
using WebprosjektBankOblig.Models;

namespace WebprosjektBankOblig.BLL
{
    public class AuthBLL : BLL.IAuthBLL
    {
        public static RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider();

        private IAuthRepository _repository;

        public AuthBLL()
        {
            _repository = new AuthRepository();
        }

        public AuthBLL(IAuthRepository stub)
        {
            _repository = stub;
        }

        const int NUM__PAST_ITERATIONS_TO_REMEMBER = 200;

        public bool kundeEksisterer(string pn)
        {
            var kunde = _repository.hentKunde(pn);

            return kunde != null && !kunde.slettet;
        }

        public Kunde hentKunde(int id)
        {
            return _repository.hentKunde(id);
        }

        public bool validerEngangspassord(string pn, int ep)
        {
            //Finn KundeAutentisering knyttet til kunden
            var autentisering = _repository.hentAuth(pn);

            //Lag en liste some kan holde 'NUM__PAST_ITERATIONS_TO_REMEMBER' av de foregående iterasjonene, 
            //for å sjekke om kunden har generert flere passord en den har brukt hos banken
            List<byte[]> sisteIterasjoner = new List<byte[]>();

            SHA384 alg = new SHA384Managed();

            byte[] hash = autentisering.engangsSeed;

            //Kalkuler fram til aktuelle iterasjonen
            for (int i = 0; i < autentisering.engangsIterasjon; i++)
            {
                hash = alg.ComputeHash(hash);

                //Husk noen iterasjoner tilbake,
                //i tilfelle kunden har generert fler passord en brukt
                if (i + NUM__PAST_ITERATIONS_TO_REMEMBER > autentisering.engangsIterasjon)
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
                if (convertToHumanreadable(sisteIterasjoner.ElementAt(i)) == ep)
                {
                    //Passordene stemte

                    //Finn ut hvilken iterasjon vi er på  
                    int validIteration = autentisering.engangsIterasjon - ((sisteIterasjoner.Count() - 1) - i);

                    //Oppdater databasen og returner sant
                    autentisering.engangsIterasjon = validIteration - 1;

                    _repository.endreAutentisering(autentisering);

                    return true;
                }
            }
            return false;
        }

        public bool validerPassord(string pn, string passord)
        {
            var Autentisering = _repository.hentAuth(pn);

            var inputHash = Hash(passord, Autentisering.PassordSalt);

            return Autentisering.PassordHash.SequenceEqual(inputHash);
        }

        public string hentKundeNavn(string pn)
        {
            var kunde = _repository.hentKunde(pn);

            if (kunde != null)
            {
                return kunde.Navn;
            }

            return "";
        }

        public int hentNesteEngangspassord(string pn)
        {
            //Finn BankIDBrikke knyttet til kunden
            var auth = _repository.hentAuth(pn);

            //Sjekk om det eksisterer
            if (auth == null)
            {
                return 0;
            }

            SHA384 alg = new SHA384Managed();

            byte[] hash = auth.engangsSeed;

            //Kalkuler fram til neste iterasjon
            for (int i = 0; i < auth.engangsIterasjon - 1; i++)
            {
                hash = alg.ComputeHash(hash);
            }

            //Lagre endringer
            auth.engangsIterasjon = auth.engangsIterasjon - 1;

            _repository.endreAutentisering(auth);

            return convertToHumanreadable(hash);
        }

        public static byte[] generateSeed(int bits)
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

        public static byte[] Hash(string password, byte[] salt)
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

        public static byte[] generateSalt(int bits)
        {
            int bytes = bits / 8;

            byte[] returnValue = new byte[bytes];

            rngCsp.GetBytes(returnValue);

            return returnValue;
        }

        //TEST DATA
        public void insertTestData()
        {
            using (var db = new BankDbContext())
            {
                var hasData = db.Kunder.Any();

                if (hasData)
                {
                    return;
                }

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
                    Poststed = nyPoststed,
                    Passord = "123",
                    GjentaPassord = "123"
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

                for (int i = 0; i < 213; i++)
                {
                    Konto konto = null;

                    switch (i % 3)
                    {
                        case 0:
                            konto = nyKonto1;
                            break;
                        case 1:
                            konto = nyKonto2;
                            break;
                        case 2:
                            konto = nyKonto3;
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
                        dato = dato,
                        beløp = r.Next(39, 94578),
                        melding = "",
                        utført = true,
                        Konto = konto
                    };
                    db.Betalinger.Add(nyBetaling);
                }
                
                    db.SaveChanges();
                
            }
        }
    }
}