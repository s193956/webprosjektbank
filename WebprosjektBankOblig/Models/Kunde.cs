using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebprosjektBankOblig.Models
{
    public class Kunde
    {
        public int Id { get; set; }
        public string Personnummer { get; set; }
        public string Navn { get; set; }
        public string Adresse { get; set; }
        public string Tlf { get; set; }
        
        public virtual Poststeder Poststed { get; set; }
        public virtual Autentisering auth { get; set; }
    }
}