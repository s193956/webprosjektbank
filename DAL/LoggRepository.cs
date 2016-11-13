using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using WebprosjektBankOblig.Models;

namespace WebprosjektBankOblig.DAL
{
    public class LoggRepository : DAL.ILoggRepository
    {
        BankDbContext db = new BankDbContext();

        public void SkrivLogg(string bruker, bool success, string beskrivelse, int? affectedId)
        {
            var entry = new LoggEntry
            {
                bruker = bruker,
                success = success,
                beskrivelse = beskrivelse,
                affectedId = affectedId
            };

            db.Logg.Add(entry);

            db.SaveChanges();
        }

        public static void SkrivLoggFil(string beskrivelse)
        {
            File.AppendAllText(AppDomain.CurrentDomain.GetData("DataDirectory").ToString(), beskrivelse + Environment.NewLine);
        }
    }
}