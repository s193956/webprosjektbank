using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebprosjektBankOblig.Models
{
    public class Kunde
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Personnummer:")]
        [Required(ErrorMessage = "Personnummer må oppgis")]
        public string Personnummer { get; set; }

        [Display(Name = "Navn:")]
        [Required(ErrorMessage = "Navn må oppgis")]
        public string Navn { get; set; }

        [Display(Name = "Adressse:")]
        [Required(ErrorMessage = "Adressse må oppgis")]
        public string Adresse { get; set; }

        [Display(Name = "Telefonnummer:")]
        [Required(ErrorMessage = "Telefonnummer må oppgis")]
        public string Tlf { get; set; }

        public bool slettet { get; set; }
        
        public virtual Poststed Poststed { get; set; }
        public virtual Autentisering Autentisering { get; set; }
        public virtual ICollection<Konto> Kontoer { get; set; }
        
        [NotMapped]
        [Display(Name = "Passord:")]
        public string Passord { get; set; }

        [NotMapped]
        [Display(Name = "Gjenta passord:")]
        public string GjentaPassord { get; set; }
    }
}