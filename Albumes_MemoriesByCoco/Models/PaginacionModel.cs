using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Albumes_MemoriesByCoco.Models
{
    public class PaginacionModel
    {
        public int nPagina { get; set; }
        public int nCantidad { get; set; }

        public double  nMax { get; set; }

        public int nMin { get; set; }   

    }
}