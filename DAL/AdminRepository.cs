using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebprosjektBankOblig.Models;

namespace WebprosjektBankOblig.DAL
{
    public class AdminRepository : DAL.IAdminRepository
    {

        BankDbContext db = new BankDbContext();

        public List<Kunde> hentAlleKunder()
        {
            return db.Kunder.ToList();
        }

    }
}