﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebprosjektBankOblig.Models;

namespace WebprosjektBankOblig.DAL
{ 
    public class AuthRepository : DAL.IAuthRepository
    {
        BankDbContext db = new BankDbContext();

        public Kunde hentKunde(string pn)
        {
            return db.Kunder.FirstOrDefault(x => x.Personnummer == pn);
        }

        public Kunde hentKunde(int id)
        {
            return db.Kunder.FirstOrDefault(x => x.Id == id);
        }

        public Autentisering hentAuth(string pn)
        {
            return db.Autentiseringer.FirstOrDefault(x => x.Kunde.Personnummer == pn);
        }

        public void endreAutentisering(Autentisering auth)
        {
            var aut = db.Autentiseringer.Find(auth.Id);

            aut.engangsIterasjon = auth.engangsIterasjon;
            aut.engangsSeed = auth.engangsSeed;
            aut.PassordHash = auth.PassordHash;
            aut.PassordSalt = auth.PassordSalt;
            
            try{
                db.SaveChanges();
                new LoggRepository().SkrivLogg(null, true, "Autentisering endret", auth.Id);
            }
            catch
            {
                LoggRepository.SkrivLoggFil("Feil ved endring av autentisering");
            }         
        }
        
        public AdminBruker hentAdmin(string login)
        {
            return db.Administratorer.FirstOrDefault(x => x.login == login);
        }

    }
}