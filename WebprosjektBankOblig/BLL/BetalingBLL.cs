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

        BetalingDAL betDAL = new BetalingDAL();

        public List<Betaling> hentBetalinger(string pn, int? id, bool? utført)
        {
            if (id.HasValue)
            {
                return betDAL.hentBetalinger(pn, id.Value, utført);
            }
            else
            {
                return betDAL.hentBetalinger(pn, utført);
            }
        }

        public void registrerBetaling(Betaling betaling)
        {
            betDAL.lagreBetaling(betaling);
        }
    
    }
}