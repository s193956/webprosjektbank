namespace TestOppgave.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public class KundeAutentisering
    {
        public int id { get; set; }

        public string PassordHash { get; set; }

        public byte[] OTPSeed { get; set; }

        public int OTPiterasjon { get; set; }

        public string OTPHashAlgoritme { get; set; }

        public string Kunde_Personnummer { get; set; }

        public virtual Kunde Kunde { get; set; }
    }
}
