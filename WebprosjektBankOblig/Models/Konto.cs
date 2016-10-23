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
        public string saldo { get; set; }

        public virtual Kunde Kunde { get; set; }
        public virtual ICollection<Betaling> Betalinger { get; set; }
        
    }
}