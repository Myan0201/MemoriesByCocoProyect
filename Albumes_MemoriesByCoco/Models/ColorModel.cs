using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Albumes_MemoriesByCoco.Models
{
    public class ColorModel
    {

        public string titulo { get; set; }
        public string descripcion { get; set; }




        public string ConsultarColor()
        {
            string response = "";
            try
            {
                using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    var respuesta = db.PA_CONS_ConsultarColor().FirstOrDefault();


                    response = respuesta.paq_descripcion;
                }
            }
            catch (Exception ex)
            {

            }
            return response;
        }

    }

}