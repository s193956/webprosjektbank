using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestOppgave.Models
{
    public class KodeBrikke
    {
        public static int genererKode()
        {
            Random rnd = new Random();

            int kode = rnd.Next(100001, 999999);
            return kode;
        }

        public static bool kodeOK(LoggInn au, Kunde k)
        {

            return false;
        }
    }
}