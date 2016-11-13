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
        private IAdminRepository _rep2;

        public AdminBLL()
        {
            _repository = new AuthRepository();
            _rep2 = new AdminRepository();
        }

        public AdminBLL(IAuthRepository stub, IAdminRepository stub2)
        {
            _repository = stub;
            _rep2 = stub2;
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

            bool success = admin.passordHash.SequenceEqual(hashed);

            if (!success)
            {
                return false;
            }else
            {
                return true;
            }
        }

        public List<Kunde> hentAlleKunder()
        {
            return _rep2.hentAlleKunder();
        }
    }
}