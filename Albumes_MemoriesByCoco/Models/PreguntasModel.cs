using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Albumes_MemoriesByCoco.Models
{
    public class PreguntasModel
    {
        public int IdPregunta { get; set; }  
        public string DescripcionPregunta { get; set; } 

        public bool Inactiva { get; set; }

    }
}