using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Albumes_MemoriesByCoco.LogicaNegocios
{
    public class GetIP
    {

        public void GetUser_IP()
        {
            string VisitorsIPAddr = string.Empty;
            if (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
            {
                VisitorsIPAddr = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
            }
            else if (HttpContext.Current.Request.UserHostAddress.Length != 0)
            {
                VisitorsIPAddr = HttpContext.Current.Request.UserHostAddress;
            }

            using(Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
            {
                db.PA_InsertarBitacora(VisitorsIPAddr, DateTime.Now, 23);
            }
           
        }
    }
}