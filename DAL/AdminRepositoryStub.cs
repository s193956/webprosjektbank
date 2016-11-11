using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebprosjektBankOblig.Models;

namespace WebprosjektBankOblig.DAL
{
    public class AdminRepositoryStub : DAL.IAdminRepository
    {

        BankDbContext db = new BankDbContext();

        public List<Kunde> hentAlleKunder()
        {
            var kundeListe = new List<Kunde>();
            var kunde = new Kunde()
            {
                Id = 1,
                Navn = "Per",
                Personnummer = "12345678912",
                Adresse = "Osloveien 82",
                Tlf = "12345678",
            };
            kundeListe.Add(kunde);
            kundeListe.Add(kunde);
            kundeListe.Add(kunde);
            return kundeListe;
        }

    }
}