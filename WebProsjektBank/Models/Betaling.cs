namespace TestOppgave.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public class Betaling
    {
        [Key]
        public int id { get; set; }
        [Key]
        [Display(Name = "Personnummer:")]
        [Required(ErrorMessage = "Personnummer må oppgis")]
        public string Personnummer { get; set; }

        [Display(Name = "Betalingsnr:")]
        [Required(ErrorMessage = "Betalingsnr må oppgis")]
        public string Betalingsnr { get; set; }

        [Display(Name = "Beløp")]
        [Required(ErrorMessage = "Vennligst oppgi beløp")]
        public long Beløp { get; set; }

        [Display(Name = "Dato")]
        [Required(ErrorMessage = "Vennligst oppgi dato for transaksjon")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime Dato { get; set; }

        [Display(Name = "Senders konto:")]
        [Required(ErrorMessage = "Vennligst oppgi konto")]
        public string FraKonto { get; set; }

        [Display(Name = "Til konto:")]
        [Required(ErrorMessage = "Vennligst oppgi konto")]
        public string TilKonto { get; set; }

        public virtual List<Konto> Kontoer { get; set; }

        public virtual List<Kunde> Kunder { get; set; }
    }
}