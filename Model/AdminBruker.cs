using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebprosjektBankOblig.Models
{
    public class AdminBruker
    {
        [Key]
        public int Id { get; set; }
        public string login { get; set; }
        public byte[] passordHash { get; set; }
        public byte[] passordSalt { get; set; }
    }
}