using WebprosjektBankOblig.Models;

namespace WebprosjektBankOblig.BLL
{
    public interface IAdminBLL
    {
        AdminBruker lagAdminBruker();
        bool validerPassord(string l, string p);
    }
}