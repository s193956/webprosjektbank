using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebprosjektBankOblig.Models
{
    //dette er både domenemodell og view
    public class Betaling
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Fra Konto:")]
        public string frakonto { get; set; }

        [Display(Name = "Til Konto:")]
        [Required(ErrorMessage = "Konto må oppgis")]
        public string tilkonto { get; set; }

        [Display(Name = "Dato:")]
        [Required(ErrorMessage = "Dato for transaksjon må oppgis")]
        [DataType(DataType.Date)]
        public DateTime dato { get; set; }

        [Display(Name = "Beløp:")]
        [Required(ErrorMessage = "Beløp må oppgis")]
        public decimal beløp { get; set; }

        [Display(Name = "Melding/ KID:")]
        public string melding { get; set; }

        public bool behandlet{ get; set; }

        public bool utført { get; set; }


        public virtual Konto Konto { get; set; }
    }
}