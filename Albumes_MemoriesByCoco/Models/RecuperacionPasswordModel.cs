using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Albumes_MemoriesByCoco.Models
{
    public class RecuperacionPasswordModel
    {
        [Required]
        [EmailAddress]
        public string Correo { get; set; }
    }
}