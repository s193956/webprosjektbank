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
        public string Personnummer { get; set; }
        public string Navn { get; set; }
        public string Adresse { get; set; }
        public string Tlf { get; set; }

        public virtual Poststed Poststed { get; set; }
        public virtual Autentisering Autentisering { get; set; }
        public virtual ICollection<Konto> Kontoer { get; set; }
    }
}