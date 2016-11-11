using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebprosjektBankOblig.Models
{
    public class LoggEntry
    {
        [Key]
        public int Id { get; set; }
        public string bruker { get; set; }
        public bool success { get; set; }
        public string beskrivelse { get; set; }
        public int?  affectedId { get; set; }
    }
}