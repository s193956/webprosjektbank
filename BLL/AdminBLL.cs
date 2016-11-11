using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebprosjektBankOblig.DAL;
using WebprosjektBankOblig.Models;

namespace WebprosjektBankOblig.BLL
{
    public class AdminBLL : BLL.IAdminBLL
    {
        private IAuthRepository _repository;

        public AdminBLL()
        {
            _repository = new AuthRepository();
        }

        public AdminBLL(IAuthRepository stub)
        {
            _repository = stub;
        }
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
            var admin = _repository.hentAdmin(l);

            if (admin == null)
                return false;

            var hashed = AuthBLL.Hash(p, admin.passordSalt);

            return admin.passordHash.SequenceEqual(hashed);
        }
    }
}