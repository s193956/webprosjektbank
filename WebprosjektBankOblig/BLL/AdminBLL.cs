using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebprosjektBankOblig.DAL;
using WebprosjektBankOblig.Models;

namespace WebprosjektBankOblig.BLL
{
    public class AdminBLL
    {
        public AdminBruker lagAdminBruker()
        {
            var salt = AuthBLL.generateSalt(384);

            var bruker = new AdminBruker
            {
                login = "admin",
                passordHash = AuthBLL.Hash("123", salt),
                passordSalt = salt
            };

            var db = new BankDbContext();
            db.Administratorer.Add(bruker);
            db.SaveChanges();

            return bruker;
        }

        public bool validerPassord(string l, string p)
        {
            var mDAL = new AuthDAL();

            var admin = mDAL.hentAdmin(l);

            if (admin == null)
                return false;

            var hashed = AuthBLL.Hash(p, admin.passordSalt);

            return admin.passordHash.SequenceEqual(hashed);
        }
    }
}