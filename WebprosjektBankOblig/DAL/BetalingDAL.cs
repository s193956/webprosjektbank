using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebprosjektBankOblig.Models;

namespace WebprosjektBankOblig.DAL
{
    public class BetalingDAL
    {
        private BankDbContext db = new BankDbContext();

        public List<Betaling> hentBetalinger(string pn, int id, bool? utført)
        {
            return (from b in db.Betalinger
                    where b.Konto.Kunde.Personnummer == pn &&
                    b.Konto.Id == id &&
                    utført.Value ? b.utført == utført : true
                    orderby b.dato
                    select b).ToList();
        }

        public List<Betaling> hentBetalinger(string pn, bool? utført)
        {
            return (from b in db.Betalinger
                    where b.Konto.Kunde.Personnummer == pn &&
                    utført.Value ? b.utført == utført : true
                    orderby b.dato
                    select b).ToList();
        }

        public Betaling hentBetaling(int id)
        {
            return db.Betalinger.Find(id);
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