using Albumes_MemoriesByCoco.LogicaNegocios;
using Albumes_MemoriesByCoco.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;


namespace Albumes_MemoriesByCoco.Controllers
{
    public class RegistroController : Controller
    {
        private static readonly Random getrandom = new Random();
        // GET: Registro
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult RegistroUsuarios()
        {
            ViewBag.InicioSesion = true;
            return View();
        }

        
        public async Task<ActionResult> ReenviarCodigoVerificacion()
        {
            UsuarioModel registro = new UsuarioModel();
            CorreoModel objCorreo = new CorreoModel();
            try
            {
                int random = (int)Session["nRandom"];
                ViewBag.Reenviado = 1;
                ViewBag.nRandom = random;
                registro = (UsuarioModel)Session["objRegistro"];
                await objCorreo.EnviarCorreo(registro.strCorreo, "Verificación Memories By Coco", "Su codigo de verificación es: " + random);
            }
            catch(Exception ex)
            {

            }

            return View("RegistroUsuarios", registro);

        }
        public ActionResult FinalizarRegistroUsuario(int verificacion)
        {
            UsuarioModel registro = new UsuarioModel();
            InicioSesionModel objInicio = new InicioSesionModel();
            
            Encriptar ecp = new Encriptar();
            try
            {
                int random = (int) Session["nRandom"];
                registro = (UsuarioModel)Session["objRegistro"];

                if (verificacion == random)
                {
                    
                    using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                    {
                       
                        db.PA_INS_RegistrarUsuario(registro.strNombre, registro.strApellido1, registro.strApellido2, registro.strCorreo, registro.strTel,
                            registro.strDireccion, registro.strCorreo, ecp.EncriptarData(registro.pass));
                        db.SaveChanges();
                        objInicio.strCorreo = registro.strCorreo;
                       
                    }
                }
                else
                {
                    ViewBag.ErrorVerificacion = 1;
                    ViewBag.nRandom = random;
                    return View("RegistroUsuarios", registro);
                    
                }

               
                
               
            }
            catch(Exception ex)
            {

            }
            return RedirectToAction("InicioSesion", "InicioSesion", registro);
        }
        public ActionResult Registrar(UsuarioModel registro)
        {
            Utilitarios objUtil = new Utilitarios();
            try
            {
                if (!ModelState.IsValid)
                {
                    return View("RegistroUsuarios", registro);
                }
                using(Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                   var Correo= db.PA_CONS_ConsultarEmailRegistrado(registro.strCorreo).FirstOrDefault();

                    if (Correo != null)
                    {
                        ViewBag.CorreoExiste = 1;
                    }
                    else
                    {
                        
                        if (objUtil.PasswordValida(registro.pass, registro.passConfirma))
                        {

                       
                        Session["objRegistro"] = registro;

                        CorreoModel objCorreo = new CorreoModel();

                        int random = getrandom.Next(1000, 9999);
                        ViewBag.nRandom = random;
                        Session["nRandom"] = random;
                        objCorreo.EnviarCorreo(registro.strCorreo, "Verificación Memories By Coco", "Su código de verificación es: " + random);
                        }
                        else
                        {
                            ViewBag.FailPass = 1;
                        }
                    }

                    
                }
                
               

                
            }
            catch(Exception ex)
            {

            }


            return View("RegistroUsuarios", registro);
        }
    }
}