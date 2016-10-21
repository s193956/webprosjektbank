using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebprosjektBankOblig.Models
{
    public class Autentisering
    {
        [Key]
        public int Id { get; set; }
        public byte[] PassordHash { get; set; }
        public byte[] PassordSalt { get; set; }
        public byte[] engangsSeed { get; set; }
        public int engangsIterasjon { get; set; }

        [Required]
        public virtual Kunde Kunde { get; set; }
    }
}