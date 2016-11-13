using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebprosjektBankOblig.Models
{
    //dette er både view og domenemodell
    public class Konto
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Kontotype:")]
        public string kontotype { get; set; }

        [Display(Name = "Kontonummer:")]
        public string kontonr { get; set; }

        [Display(Name = "Disponibelt beløp:")]
        public decimal saldo { get; set; }
        
        public bool slettet { get; set; }

        public virtual Kunde Kunde { get; set; }
        public virtual ICollection<Betaling> Betalinger { get; set; }
        
    }
}