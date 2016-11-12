using System.Collections.Generic;
using WebprosjektBankOblig.Models;

namespace WebprosjektBankOblig.BLL
{
    public interface IKontoBLL
    {
        Konto hentKonto(string kontonr);
        List<Konto> hentKontoer();
        List<Konto> hentKontoer(string pn);
    }
}