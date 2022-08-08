using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Albumes_MemoriesByCoco.Models
{
    public class InicioSesionModel
    {
        [Required]
        [DisplayName("Correo")]
        public string strCorreo { get; set; }

        [Required]
        [DisplayName("Contraseña")]
        public string strPassword { get; set; }
    }
}