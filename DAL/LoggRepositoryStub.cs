using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebprosjektBankOblig.Models;

namespace WebprosjektBankOblig.DAL
{
    public class LoggRepositoryStub : DAL.ILoggRepository
    {
        BankDbContext db = new BankDbContext();

        public void SkrivLogg(string bruker, bool success, string beskrivelse, int? affectedId)
        {
            var entry = new LoggEntry
            {
                bruker = "",
                success = true,
                beskrivelse = "",
                affectedId = affectedId
            };

            db.Logg.Add(entry);

            db.SaveChanges();
        }
    }
}