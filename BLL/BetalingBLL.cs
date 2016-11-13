using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Web;
using WebprosjektBankOblig.DAL;
using WebprosjektBankOblig.Models;

namespace WebprosjektBankOblig.BLL
{
    public class BetalingBLL : BLL.IBetalingBLL
    {

        private IBetalingRepository _repository;

        public BetalingBLL()
        {
            _repository = new BetalingRepository();
        }

        public BetalingBLL(IBetalingRepository stub)
        {
            _repository = stub;
        }

        public List<Betaling> hentBetalinger(string pn, int? id, bool? behandlet)
        {
            if (id.HasValue)
            {
                return _repository.hentBetalinger(pn, id.Value, behandlet);
            }
            else
            {
                return _repository.hentBetalinger(pn, behandlet);
            }
        }

        public void registrerBetaling(Betaling betaling)
        {
            _repository.lagreBetaling(betaling);

            processPayment(betaling.Id);
        }

        public Betaling hentBetaling(int id)
        {
            return _repository.hentBetaling(id);
            //return db.Betalinger.Find(id);
        }

        public void endreBetaling(Betaling betaling)
        {
            _repository.endreBetaling(betaling);

            processPayment(betaling.Id);
        }

        public void slettBetaling(int id)
        {
            _repository.slettBetaling(id);
        }

        public List<Betaling> hentBetalinger()
        {
            return _repository.hentBetalinger();
        }

        private void processPayment(int Id)
        {
            var db = new BankDbContext();

            var betaling = db.Betalinger.Find(Id);

            // Skal ikke håndteres idag.
            if (betaling.dato != DateTime.Today)
                return;

            bool internalTransaction = db.Kontoer.Any(x => x.kontonr == betaling.tilkonto);

            // Den daglige transaksjonen av betalinger vil ta hånd om denne betalingen.
            if (!internalTransaction)
                return;

            // Betalingen skal gjennomføres nå
            var frakonto = db.Kontoer.FirstOrDefault(x => x.kontonr == betaling.frakonto);

            // Det var for lite penger på konto, marker betaling som feilet
            if (frakonto.saldo < betaling.beløp)
            {
                betaling.behandlet = true;
                betaling.utført = false;
                db.SaveChanges();
                return;
            }

            if (internalTransaction)
            {
                var tilkonto = db.Kontoer.FirstOrDefault(x => x.kontonr == betaling.tilkonto);

                tilkonto.saldo += betaling.beløp;
                frakonto.saldo -= betaling.beløp;
            }
            else
            {
                frakonto.saldo -= betaling.beløp;
            }

            betaling.utført = true;
            betaling.behandlet = true;

            db.SaveChanges();

        }

        public void processTodaysPayments()
        {
            Thread.Sleep(2350);

            var db = new BankDbContext();
           
            var betalinger = db.Betalinger.Where(x => !x.behandlet && 
                x.dato == DateTime.Today);

            foreach(var betaling in betalinger)
            {
                // Betalingen skal gjennomføres nå
                var frakonto = db.Kontoer.FirstOrDefault(x => x.kontonr == betaling.frakonto);

                // Det var for lite penger på konto, marker betaling som feilet
                if (frakonto.saldo < betaling.beløp)
                {
                    betaling.behandlet = true;
                    betaling.utført = false;
                    db.SaveChanges();
                    return;
                }

                if (db.Kontoer.Any(x => x.kontonr == betaling.tilkonto))
                {
                    var tilkonto = db.Kontoer.FirstOrDefault(x => x.kontonr == betaling.tilkonto);

                    tilkonto.saldo += betaling.beløp;
                    frakonto.saldo -= betaling.beløp;
                }
                else
                {
                    frakonto.saldo -= betaling.beløp;
                }

                betaling.utført = true;
                betaling.behandlet = true;

                db.SaveChanges();
            }
        }

    }
}