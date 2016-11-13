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
        [Display(Name = "Personnummer")]
        [Required(ErrorMessage = "Personnummer må oppgis")]
        [RegularExpression(@"[0-9]{11}", ErrorMessage = "Postnr må være 11 siffer")]
        public string Personnummer { get; set; }
        [Display(Name = "Navn")]
        [Required(ErrorMessage = "Navn må oppgis")]
        public string Navn { get; set; }
        [Display(Name = "Adresse")]
        [Required(ErrorMessage = "Adresse må oppgis")]
        public string Adresse { get; set; }
        [Display(Name = "Mobil")]
        [Required(ErrorMessage = "Mobil må oppgis")]
        [RegularExpression(@"[0-9]{8}", ErrorMessage = "Mobil må være 8 siffer")]
        public string Tlf { get; set; }

        public bool slettet { get; set; }
        public virtual Poststed Poststed { get; set; }
        public virtual Autentisering Autentisering { get; set; }
        public virtual ICollection<Konto> Kontoer { get; set; }
    }
}