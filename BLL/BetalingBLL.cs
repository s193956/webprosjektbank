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
        }

        public Betaling hentBetaling(int id)
        {
            return _repository.hentBetaling(id);
            //return db.Betalinger.Find(id);
        }

        public void endreBetaling(Betaling betaling)
        {
            _repository.endreBetaling(betaling);
        }

        public void slettBetaling(int id)
        {
            _repository.slettBetaling(id);
        }

        public List<Betaling> hentBetalinger()
        {
            return _repository.hentBetalinger();
        }
    }
}