using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Albumes_MemoriesByCoco.Models
{
    public class CambiarPasswordModel
    {
        [Required]
        [DisplayName("Contraseña")]
        public string Password { get;set; }

        [Required]
        [DisplayName("Confirmar Contraseña")]

        public string ConfirmarPassword { get;set; }    

    }
}