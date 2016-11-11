using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebprosjektBankOblig.Models;

namespace WebprosjektBankOblig.DAL
{
    public class AuthRepositoryStub : DAL.IAuthRepository
    {
        BankDbContext db = new BankDbContext();

        public Kunde hentKunde(string pn)
        {
            var kunde = new Kunde()
            {             
                Personnummer = pn
            };
            return kunde;
        }

        public Kunde hentKunde(int id)
        {
            var kunde = new Kunde()
            {
                Id = id
            };
            return kunde;
        }

        public Autentisering hentAuth(string pn)
        {
            var kunde = new Kunde()
            {
                Personnummer = pn
            };
            var aut = new Autentisering()
            {
                Kunde = kunde
            };
            return aut;
        }

        public void endreAutentisering(Autentisering auth)
        {
            var aut = db.Autentiseringer.Find(auth.Id);

            aut.engangsIterasjon = auth.engangsIterasjon;
            aut.engangsSeed = auth.engangsSeed;
            aut.PassordHash = auth.PassordHash;
            aut.PassordSalt = auth.PassordSalt;

            db.SaveChanges();
        }

        public AdminBruker hentAdmin(string login)
        {
            return db.Administratorer.FirstOrDefault(x => x.login == login);
        }

    }
}