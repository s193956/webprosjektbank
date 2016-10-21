using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebprosjektBankOblig.Models
{
    public class Poststeder
    {
        [Key]
        public int Id { get; set; }
        public int Postnummer { get; set; }
        public string Poststed { get; set; }

        public virtual Kunde[] kunder{ get; set; }
    }
}