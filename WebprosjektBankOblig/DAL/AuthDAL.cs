using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebprosjektBankOblig.Models;

namespace WebprosjektBankOblig.DAL
{ 
    public class AuthDAL
    {
        public Kunde hentKunde(string pn)
        {
            var db = new BankDbContext();

            return db.Kunder.FirstOrDefault(x => x.Personnummer == pn);
        }

        public Kunde hentKunde(int id)
        {
            var db = new BankDbContext();

            return db.Kunder.FirstOrDefault(x => x.Id == id);
        }

        public Autentisering hentAuth(string pn)
        {
            var db = new BankDbContext();

            return db.Autentiseringer.FirstOrDefault(x => x.Kunde.Personnummer == pn);
        }

        public void endreAutentisering(Autentisering auth)
        {
            var db = new BankDbContext();

            var aut = db.Autentiseringer.Find(auth.Id);

            aut.engangsIterasjon = auth.engangsIterasjon;
            aut.engangsSeed = auth.engangsSeed;
            aut.PassordHash = auth.PassordHash;
            aut.PassordSalt = auth.PassordSalt;
            
            db.SaveChanges();            
        }

    }
}