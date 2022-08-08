using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Albumes_MemoriesByCoco.LogicaNegocios;
using Albumes_MemoriesByCoco.Models;

namespace Albumes_MemoriesByCoco.Controllers
{
    public class InicioSesionController : Controller
    {
        // GET: InicioSesion
        public ActionResult Index()
        {
           
            return View("InicioSesion");
        }

        public ActionResult InicioSesion()
        {
            GetIP get = new GetIP();
            get.GetUser_IP();
            ViewBag.InicioSesion = true;
            return View();
        }
        public ActionResult Logearse(InicioSesionModel modelo)
        {
            if (!ModelState.IsValid)
            {
                return View("InicioSesion",modelo);
            }
            try
            {
               
                    Utilitarios objUtil = new Utilitarios();
                    Encriptar objEnc = new Encriptar();
                    using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                    {
                        var listaRespuesta = db.PA_CONS_ConsultarInicioSesion(modelo.strCorreo).ToList();

                        List<UsuarioModel> listaUsuario = new List<UsuarioModel>();

                        listaUsuario = (from e in listaRespuesta
                                        select new UsuarioModel
                                        {
                                            strCorreo = e.seg_correo,
                                            nID = e.seg_id,
                                            strNombre = e.seg_nombre,
                                            strApellido1 = e.seg_apellido1,
                                            strApellido2 = e.seg_apellido2,
                                            strDireccion = e.seg_direccion,
                                            strTel = e.seg_tel,
                                            pass = e.seg_pass,
                                            nRol = e.paq_tipo_usuario

                                        }).ToList();


                        Session["Usuario"] = listaUsuario;
                        

                        if (listaUsuario == null || listaUsuario.Count == 0)
                        {
                            modelo.strPassword = "";
                            ViewBag.DatosInvalidos = 1;
                            return View("InicioSesion", modelo);
                        }
                        if (modelo.strPassword == objEnc.DesEncriptarData(listaUsuario[0].pass) && modelo.strCorreo == listaUsuario[0].strCorreo)
                        {
                            return RedirectToAction("MiPerfil", "Perfil", modelo);
                        }
                        else
                        {
                            modelo.strPassword = "";
                            ViewBag.DatosInvalidos = 1;
                            return View("InicioSesion", modelo);
                        }

                    }



                
            }catch(Exception ex)
            {

            }



            return RedirectToAction("Index", "Perfil");


        }

        public ActionResult CerrarSesion()
        {
            Session.RemoveAll();

            return RedirectToAction("Index", "InicioSesion");

        }

       
     
    }
}