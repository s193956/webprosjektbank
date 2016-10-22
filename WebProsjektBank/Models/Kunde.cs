namespace TestOppgave.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public class Kunde
    {
        [Key]
        public int id { get; set; }

        [Key]
        [Display(Name = "Personnummer")]
        [RegularExpression(@"^(\d{11})$", ErrorMessage = "Personnummer må være 11 siffer.")]
        [Required(ErrorMessage = "Vennligst oppgi personnummer")]
        public string Personnummer { get; set; }

        [Display(Name = "Fornavn")]
        [Required(ErrorMessage = "Vennligst oppgi fornavn")]
        public string Fornavn { get; set; }

        [Display(Name = "Etternavn")]
        [Required(ErrorMessage = "Vennligst oppgi etternavn")]
        public string Etternavn { get; set; }

        [Display(Name = "Adresse")]
        [Required(ErrorMessage = "Vennligst oppgi adresse")]
        public string Adresse { get; set; }

        [Display(Name = "Postnr")]
        [Required(ErrorMessage = "Vennligst oppgi postnr")]
        public string Postnr { get; set; }

        [Display(Name = "Poststed")]
        [Required(ErrorMessage = "Vennligst oppgi poststed")]
        public string Poststed { get; set; }

        [Required(ErrorMessage = "Vennligst oppgi telefonnr")]
        [Display(Name = "Telefon")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{2})[-. ]?([0-9]{3})$", ErrorMessage = "Ugyldig telefon nummer.")]
        public string Telefonnr { get; set; }

        [Display(Name = "Epost")]
        [EmailAddress(ErrorMessage = "Ugyldig Epost")]
        public string Epost { get; set; }

        [Required]
        [StringLength(100, ErrorMessage =
        "{0} må være minst {2} tegn.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Passord")]
        public string Passord { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Gjenta Passord")]
        [Compare("Passord", ErrorMessage =
        "Passordene må stemme.")]
        public string PassordGjenta { get; set; }

        public virtual List<Konto> Kontoer { get; set; }

        public virtual List<Betaling> Betalinger { get; set; }

        public virtual LoggInn Logginng { get; set; }
    }
}