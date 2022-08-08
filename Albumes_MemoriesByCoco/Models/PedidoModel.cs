using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Albumes_MemoriesByCoco.Models
{
    [Serializable]
    public class PedidoModel
    {
        public int IdPedido { get;set; }
        public int? Personalizado { get; set; }

        public string TipoAlbum { get; set; }

        public List<int> listaFrases = new List<int>();

       public string nombreCompleto { get; set; }

        public string strEstado { get; set; }       

        public int? TipoVinil { get; set; }

        public string Respuesta { get; set; }

        public int idPregunta { get; set; }

        public string strTransferencia { get; set; }
        public int nEstado { get; set; }
        public decimal nMonto { get; set; }

        public int nAlbumPredefinido { get; set; }


        public string fechaPedido { get;set;}

        public string strFoto { get; set; }


    }
}