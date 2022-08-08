using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Albumes_MemoriesByCoco.Controllers
{
    public class VelasController : Controller
    {
        // GET: Velas
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ErrorInesperado()
        {
            return View();
        }

        public ActionResult PedidoProcesado()
        {
            if (Session["NumeroPedido"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Perfil");
            }
         
        }

        public JsonResult ObtenerNumeroPedido()
        {
            string response = "";
            try
            {

                response = Session["NumeroPedido"].ToString();
                
                    
               
                
            }
            catch(Exception ex)
            {

            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        

       

    }
}