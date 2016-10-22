namespace TestOppgave.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public enum KontoType
    {
        Brukskonto, Sparekonto
    }
    public class Konto
    {
        [Key]
        public int id { get; set; }

        [Key]
        [Display(Name = "Personnummer:")]
        [Required(ErrorMessage = "Personnummer må oppgis")]
        public string Personnummer { get; set; }

        [Display(Name = "Kontonr")]
        [Required(ErrorMessage = "Vennligst velg kontonr")]
        public string Kontonr { get; set; }


        [Display(Name = "Kontotype")]
        [Required(ErrorMessage = "Vennligst velg kontotype")]
        public KontoType KontoType { get; set; }

        public long Saldo { get; set; }

        public virtual List<Kunde> Kunder { get; set; }

        public virtual List<Betaling> Betalinger { get; set; }
    }
}