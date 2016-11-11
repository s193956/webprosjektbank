using System.Collections.Generic;
using WebprosjektBankOblig.Models;

namespace WebprosjektBankOblig.DAL
{
    public interface IAdminRepository
    {
        List<Kunde> hentAlleKunder();
    }
}