using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebprosjektBankOblig.Models
{
    public class Betaling
    {
        [Key]
        public int Id { get; set; }
        public string transaksjonsnr { get; set; }
        public string frakonto { get; set; }

        [Display(Name = "Til Konto:")]
        [Required(ErrorMessage = "Konto må oppgis")]
        public string tilkono { get; set; }

        [Display(Name = "Dato:")]
        [Required(ErrorMessage = "Dato for transaksjon må oppgis")]
        [DataType(DataType.Date)]
        public string dato { get; set; }

        [Display(Name = "Beløp:")]
        [Required(ErrorMessage = "Beløp må oppgis")]
        public string beløp { get; set; }
        public string melding { get; set; }

        public virtual Konto Konto { get; set; }
    }
}