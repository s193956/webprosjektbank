using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebprosjektBankOblig.Models;
using System.Data.SqlClient;

namespace WebprosjektBankOblig.DAL
{
    public class BetalingRepository : DAL.IBetalingRepository
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

        public List<Betaling> hentBetalinger()
        {
            return db.Betalinger.ToList();
        }

        public List<Betaling> hentBetalinger(int kontoId)
        {
            return db.Betalinger.Where(x => x.Konto.Id == kontoId).ToList();
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

            db.SaveChanges();
        }

        public void slettBetaling(int id)
        {
            var betaling = hentBetaling(id);

            db.Betalinger.Remove(betaling);

            db.SaveChanges();
        }

        public void endreBetaling(int id, string frakonto, string tilkonto, string dato, string beløp, string melding)
        {
            var bet = db.Betalinger.Find(id);

            if (bet == null)
                throw new Exception("Betalingen finnes ikke!");

            bet.frakonto = frakonto;
            bet.tilkonto = tilkonto;
            bet.dato = dato;
            bet.beløp = beløp;
            bet.melding = melding;

            if (!db.ChangeTracker.HasChanges())
            {
                throw new Exception("HVA FAEN I HELVETE ER DET SOM FOREGÅR");
            }

            db.SaveChanges();
        }
    }
}