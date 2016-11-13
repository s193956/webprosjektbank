using System;
using System.Collections.Generic;
using System.Linq;
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

        public List<Betaling> hentBetalinger(string pn, int? id, bool? utført)
        {
            if (id.HasValue)
            {
                return _repository.hentBetalinger(pn, id.Value, utført);
            }
            else
            {
                return _repository.hentBetalinger(pn, utført);
            }
        }

        public void registrerBetaling(Betaling betaling)
        {
            _repository.lagreBetaling(betaling);

            processPayment(betaling);
        }

        public Betaling hentBetaling(int id)
        {
            return _repository.hentBetaling(id);
            //return db.Betalinger.Find(id);
        }

        public void endreBetaling(Betaling betaling)
        {
            _repository.endreBetaling(betaling);

            processPayment(betaling);
        }

        public void slettBetaling(int id)
        {
            _repository.slettBetaling(id);
        }

        public List<Betaling> hentBetalinger()
        {
            return _repository.hentBetalinger();
        }

        private void processPayment(Betaling betaling)
        {
            var db = new BankDbContext();
            
            // Skal ikke håndteres idag.
            if (betaling.dato != DateTime.Today)
                return;

            bool internalTransaction = db.Kontoer.Any(x => x.kontonr == betaling.tilkonto);

            // Den daglige transaksjonen av betalinger vil ta hånd om denne betalingen.
            if (!internalTransaction)
                return;

            var frakonto = db.Kontoer.FirstOrDefault(x => x.kontonr == betaling.frakonto);

            // det var for lite penger på konto, marker betaling som failed
            if(frakonto.saldo < betaling.beløp)
            {
                betaling.utført = false;
                db.SaveChanges();
                return;
            }



        }
    }
}