using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebprosjektBankOblig.Models
{
    public class Autentisering
    {
        public int Id { get; set; }
        public virtual Kunde Kunde { get; set; }
        public string PassordHash { get; set; }
        public string PassordSalt { get; set; }
        public byte[] engangsSeed { get; set; }
        public int engangsIterasjon { get; set; }
    }
}