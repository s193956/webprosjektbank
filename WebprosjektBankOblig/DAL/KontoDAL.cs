using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebprosjektBankOblig.Models;

namespace WebprosjektBankOblig.DAL
{
    public class KontoDAL
    {
        public List<Konto> hentKontoer(string pn)
        {
            var db = new BankDbContext();

            return db.Kontoer.Where(x => x.Kunde.Personnummer == pn).ToList();
        }

        public Konto hentKonto(string kontonr)
        {
            var db = new BankDbContext();

            return db.Kontoer.FirstOrDefault(x => x.kontonr == kontonr);
        }
    }
}