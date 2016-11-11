using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebprosjektBankOblig.Models;

namespace WebprosjektBankOblig.DAL
{
    public class KontoRepository : DAL.IKontoRepository
    {
        BankDbContext db = new BankDbContext();


        public List<Konto> hentKontoer(string pn)
        {
            return db.Kontoer.Where(x => x.Kunde.Personnummer == pn).ToList();
        }

        public List<Konto> hentKontoer()
        {
            return db.Kontoer.ToList();
        }

        public Konto hentKonto(string kontonr)
        {
            return db.Kontoer.FirstOrDefault(x => x.kontonr == kontonr);
        }
    }
}