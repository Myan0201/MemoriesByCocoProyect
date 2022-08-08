using Albumes_MemoriesByCoco.LogicaNegocios;
using Albumes_MemoriesByCoco.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Albumes_MemoriesByCoco.Controllers
{
    public class InicioController : Controller
    {
        // GET: Inicio
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult InicioMemoriesByCoco()
        {

            return View();
        }


        public ActionResult IndexMemoriesByCoco()
        {

            return View();
        }


        public JsonResult ConsultarColor()
        {
            string response = "";
            ColorModel colorModel = new ColorModel();
            try
            {
                response = colorModel.ConsultarColor();
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return Json(new { IsCreated = false, ErrorMessage = "Error inesperado..." + ex });
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        public JsonResult EnvioCorreoContacto(ContactoModel objContacto)
        {
            CorreoModel envio = new CorreoModel();
            try
            {
                using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    var response = db.PA_CONS_ConsultarUsuarioAdmin().ToList();
                    var correo = db.PA_CONS_ConsultarCorreo(Constantes.CorreoBD).FirstOrDefault();
                    for (int i = 0; i < response.Count; i++)
                    {
                      

                            string cuerpo = "Estimado " + response[i].seg_nombre + " " + response[i].seg_apellido1 + " " + response[i].seg_apellido1 + "<br>" +
                                "Se le informa que tiene un contacto de un cliente con el nombre " + objContacto.nombre + " el cual ha brindado los siguientes datos: <br>" +
                                "Correo: " + objContacto.correo + "<br>" +
                                "Teléfono: " + objContacto.telefono + "<br>" +
                                "Mensaje: " + objContacto.mensaje + "<br>";

                            envio.EnviarCorreo(correo.paq_descripcion, "Contacto de cliente", cuerpo);
                        
                    }



                }
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return Json(new { IsCreated = false, ErrorMessage = "Error inesperado..." + ex });
            }


            return Json("", JsonRequestBehavior.AllowGet);
        }


    }
}