namespace WebprosjektBankOblig.BLL
{
    public interface IAuthBLL
    {
        string hentKundeNavn(string pn);
        int hentNesteEngangspassord(string pn);
        void insertTestData();
        bool kundeEksisterer(string pn);
        bool validerEngangspassord(string pn, int ep);
        bool validerPassord(string pn, string passord);
    }
}