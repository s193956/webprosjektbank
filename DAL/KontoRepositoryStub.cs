using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebprosjektBankOblig.Models;

namespace WebprosjektBankOblig.DAL
{
    public class KontoRepositoryStub : DAL.IKontoRepository
    {
        BankDbContext db = new BankDbContext();


        public List<Konto> hentKontoer(string pn)
        {
            var kunde = new Kunde()
            {
                Personnummer = pn
            };
            var konto = new Konto()
            {
                Kunde = kunde
            };
            var kontoListe = new List<Konto>();
            kontoListe.Add(konto);
            return kontoListe;
        }

        public List<Konto> hentKontoer()
        {
            var kontoListe = new List<Konto>();
            var konto = new Konto()
            {
                Id = 1,
                kontotype = "",
                kontonr = "",
                saldo = 0,

            };
            kontoListe.Add(konto);
            kontoListe.Add(konto);
            kontoListe.Add(konto);
            return kontoListe;
        }

        public Konto hentKonto(string kontonr)
        {
            var konto = new Konto()
            {
                kontonr = kontonr

            };
            return konto;
        }
    }
}