using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TestOppgave.Models
{
    public class LoggInn
    {
        [Key]
        public int id { get; set; }

        [Key]
        [Display(Name = "Personnummer:")]
        [Required(ErrorMessage = "Personnummer må oppgis")]
        public string Personnummer { get; set; }

        [Display(Name = "Passord:")]
        [Required(ErrorMessage = "Passord må oppgis")]
        [DataType(DataType.Password)]
        public string Passord { get; set; }

        public virtual List<Kunde> Kunder { get; set; }

        public virtual List<Konto> Kontoer { get; set; }

        public virtual List<Betaling> Betalinger { get; set; }
    }
}