using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Albumes_MemoriesByCoco.Models
{
    public class UsuarioModel
    {
        [Required]
        [DisplayName("Nombre")]
        public string strNombre { get; set; }

        [Required]
        [DisplayName("Primer apellido")]
        public string strApellido1 { get; set; }

        [DisplayName("Segundo apellido")]
        public string strApellido2 { get; set; }

        [Required]
        [DisplayName("Correo")]
        [EmailAddress]
        public string strCorreo { get; set; }

        [Required]
        [DisplayName("Celular")]
        
        public string strTel { get; set; }

        [Required]
        [DisplayName("Dirección")]
        public string strDireccion { get; set; }

        [Required]
        [DisplayName("Contraseña")]
        public string pass { get; set; }

        [Required]
        [DisplayName("Contraseña")]
        public string passConfirma { get; set; }

        public int nRol { get; set; }

        public int nID { get; set; }



    }
}