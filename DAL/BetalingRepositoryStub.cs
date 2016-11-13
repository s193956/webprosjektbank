using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebprosjektBankOblig.Models;

namespace WebprosjektBankOblig.DAL
{
    public class BetalingRepositoryStub : DAL.IBetalingRepository
    {
        private BankDbContext db = new BankDbContext();

        public List<Betaling> hentBetalinger(string pn, int id, bool? utført)
        {
            var betListe = new List<Betaling>();
            var bet = new Betaling()
            {
                Id = 1,
                frakonto = "",
                tilkonto = "",
                dato = DateTime.Now,
                beløp = 0,
                melding = "",
                utført = true

            };
            betListe.Add(bet);
            betListe.Add(bet);
            betListe.Add(bet);



            return betListe;
            /*
            return (from b in db.Betalinger
                    where b.Konto.Kunde.Personnummer == pn &&
                    b.Konto.Id == id &&
                    utført.Value ? b.utført == utført : true
                    orderby b.dato
                    select b).ToList();*/
        }

        public List<Betaling> hentBetalinger(string pn, bool? utført)
        {
            var betListe = new List<Betaling>();
            var bet = new Betaling()
            {
                Id = 1,
                frakonto = "",
                tilkonto = "",
                dato = DateTime.Now,
                beløp = 0,
                melding = "",
                utført = true

            };
            betListe.Add(bet);
            betListe.Add(bet);
            betListe.Add(bet);



            return betListe;

            /*return (from b in db.Betalinger
                    where b.Konto.Kunde.Personnummer == pn &&
                    utført.Value ? b.utført == utført : true
                    orderby b.dato
                    select b).ToList();*/
        }

        public List<Betaling> hentBetalinger()
        {
            var betListe = new List<Betaling>();
            var bet = new Betaling()
            {
                Id = 1,
                frakonto = "",
                tilkonto = "",
                dato = DateTime.Now,
                beløp = 0,
                melding = "",
                utført = true

            };
            betListe.Add(bet);
            betListe.Add(bet);
            betListe.Add(bet);



            return betListe;
            //return db.Betalinger.ToList();
        }

        public Betaling hentBetaling(int id)
        {
            var bet = new Betaling()
            {
                Id = id
            };
            return db.Betalinger.Find(bet);
            //return db.Betalinger.Find(id);
        }

        public void lagreBetaling(Betaling betaling)
        {
            db.Betalinger.Add(betaling);

            var fraKonto = db.Kontoer.FirstOrDefault(x => x.kontonr == betaling.frakonto);

            betaling.Konto = fraKonto;

            db.SaveChanges();
        }

        public void endreBetaling(Betaling betaling)
        {
            db.Entry(betaling).State = EntityState.Modified;
            /*
            var betaling = db.Betalinger.Find(b.Id);
            
            betaling.beløp = b.beløp;
            betaling.dato = b.dato;
            betaling.frakonto = b.frakonto;
            betaling.melding = b.melding;
            betaling.tilkonto = b.tilkonto;
            betaling.utført = b.utført;
            */
            db.SaveChanges();
        }

        public void slettBetaling(int id)
        {
            var betaling = hentBetaling(id);

            db.Betalinger.Remove(betaling);

            db.SaveChanges();
        }
    }
}