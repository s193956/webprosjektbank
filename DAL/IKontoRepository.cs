using System.Collections.Generic;
using WebprosjektBankOblig.Models;

namespace WebprosjektBankOblig.DAL
{
    public interface IKontoRepository
    {
        Konto hentKonto(string kontonr);
        List<Konto> hentKontoer();
        List<Konto> hentKontoer(string pn);
    }
}