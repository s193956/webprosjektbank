using System.Collections.Generic;
using WebprosjektBankOblig.Models;

namespace WebprosjektBankOblig.DAL
{
    public interface IBetalingRepository
    {
        void endreBetaling(Betaling betaling);
        Betaling hentBetaling(int id);
        List<Betaling> hentBetalinger();
        List<Betaling> hentBetalinger(string pn, bool? utført);
        List<Betaling> hentBetalinger(string pn, int id, bool? utført);
        void lagreBetaling(Betaling betaling);
        void slettBetaling(int id);
    }
}