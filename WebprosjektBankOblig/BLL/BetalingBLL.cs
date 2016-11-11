using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebprosjektBankOblig.DAL;
using WebprosjektBankOblig.Models;

namespace WebprosjektBankOblig.BLL
{
    public class BetalingBLL
    {

        public List<Betaling> hentBetalinger(string pn, int? id, bool? utført)
        {
            var mDAL = new BetalingDAL();

            if (id.HasValue)
            {
                return mDAL.hentBetalinger(pn, id.Value, utført);
            }
            else
            {
                return mDAL.hentBetalinger(pn, utført);
            }
        }

        public void registrerBetaling(Betaling betaling)
        {
            var bDAL = new BetalingDAL();

            bDAL.lagreBetaling(betaling);
        }
    
    }
}