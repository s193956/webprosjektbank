using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebprosjektBankOblig.Models
{
    public class Poststed
    {
        [Key]
        public int Id { get; set; }
        public string Navn { get; set; }
        public int Nummer { get; set; }
 
        public virtual ICollection<Kunde> Kunder { get; set; }
    }
}