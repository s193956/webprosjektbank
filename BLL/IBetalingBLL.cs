using System.Collections.Generic;
using WebprosjektBankOblig.Models;

namespace WebprosjektBankOblig.BLL
{
    public interface IBetalingBLL
    {
        List<Betaling> hentBetalinger(string pn, int? id, bool? utført);
        void registrerBetaling(Betaling betaling);
    }
}