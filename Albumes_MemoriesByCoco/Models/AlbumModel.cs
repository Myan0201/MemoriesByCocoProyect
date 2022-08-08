using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Albumes_MemoriesByCoco.Models
{
    public class AlbumModel
    {
        public int idAlbum { get; set; }
        [DisplayName("Nombre")]
        public string nombre { get; set; }


        public int Categoria { get; set; }
        public string strImagen { get; set; }

        public byte byImagen { get; set; }

        [DisplayName("Es Portada")]
        public bool esPortada { get;set; }
    
        public string nombreCategoria { get; set; }

        public string descripcionCategoria { get; set; }

    

      
    }
}