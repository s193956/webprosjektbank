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
   
    }
}