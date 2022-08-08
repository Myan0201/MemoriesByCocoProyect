using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Albumes_MemoriesByCoco.Models
{
    public class CategoriaModel
    {

        public int idCategoria{ get; set; }
        [DisplayName("Nombre")]
        public string nombre { get; set; }

        [DisplayName("Descripción")]
        public string descripcion { get; set; }
        public string strImagen { get; set; }



        [DataType(DataType.Upload)]
        public byte[] Imagen { get; set; }

        public bool Inactivo { get; set; }
    }
}