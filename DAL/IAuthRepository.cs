using WebprosjektBankOblig.Models;

namespace WebprosjektBankOblig.DAL
{
    public interface IAuthRepository
    {
        void endreAutentisering(Autentisering auth);
        AdminBruker hentAdmin(string login);
        Autentisering hentAuth(string pn);
        Kunde hentKunde(string pn);
        Kunde hentKunde(int id);
    }
}