using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Albumes_MemoriesByCoco.Models
{
    public class EstadoModel
    {

        public int idEstado { get; set; }
        [DisplayName("Nombre")]
        public string nombre { get;set; }

        [DisplayName("Notificar remitente")]
        public bool notifica { get; set; }


    }
}