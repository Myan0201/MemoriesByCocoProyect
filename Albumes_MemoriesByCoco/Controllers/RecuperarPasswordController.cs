using Albumes_MemoriesByCoco.LogicaNegocios;
using Albumes_MemoriesByCoco.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Albumes_MemoriesByCoco.Controllers
{
  
    public class RecuperarPasswordController : Controller
    {
        private static readonly Random getrandom = new Random();
        // GET: RecuperarPassword
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult RecuperarContraseña()
        {
            
            try
            {
                ViewBag.InicioSesion = true;
            } catch (Exception ex)
            {

            }
            return View();
        }
   
        public ActionResult ConfirmarCambioContraseña(CambiarPasswordModel objCambiar)
        {
            if (!ModelState.IsValid)
            {
                return View("CambiarContraseña", objCambiar);
            }
            InicioSesionModel objInicio = new InicioSesionModel();
            Utilitarios objUtil = new Utilitarios();
            RecuperacionPasswordModel objRecuperar = new RecuperacionPasswordModel();
            try
            {
                Encriptar objEncriptar = new Encriptar();
                if (Session["CorreoCambio"] != null && Session["NumeroCambio"]!=null)
                {
                    if (objUtil.PasswordValida(objCambiar.Password, objCambiar.ConfirmarPassword))
                    {
                        using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                        {
                            string correo = Session["CorreoCambio"].ToString();
                           objInicio.strCorreo=correo;
                           int? codigoUsuario= db.PA_CONS_Consultar_Solicita_Password(Session["CorreoCambio"].ToString()).FirstOrDefault();

                            if(codigoUsuario == (int?)Session["NumeroCambio"])
                            {
                                db.PA_CambiosPassword(Session["Correo"].ToString(), objEncriptar.EncriptarData(objCambiar.Password));
                                db.SaveChanges();
                            }

                           
                        }
                    }
                    else
                    {
                        ViewBag.FailPass = 1;
                        return View("CambiarContraseña", objCambiar);
                    }
                }
                
               

            }
            catch (Exception ex)
            {

            }
            return RedirectToAction("InicioSesion", "InicioSesion",objInicio);
        }
        [HttpGet]
        public ActionResult CambiarContraseña(string id, string c)
        {
            Encriptar objEncriptar = new Encriptar();
            try
            {
                ViewBag.InicioSesion = true;
                string p = objEncriptar.DesEncriptarData(id);
                string j = objEncriptar.DesEncriptarData(c);
                Session["NumeroCambio"] = objEncriptar.DesEncriptarData(id);
                Session["CorreoCambio"] = objEncriptar.DesEncriptarData(c);

            }catch(Exception ex)
            {

            }
            return View();
        }
        public ActionResult EnviarRecuperacionPassword(RecuperacionPasswordModel objRecuperar)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View("RecuperarContraseña", objRecuperar);
                }
                CorreoModel objCorreo = new CorreoModel();
                Encriptar objEncriptar = new Encriptar();
                using(Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                   var correoRes= db.PA_CONS_ConsultarEmailRegistrado(objRecuperar.Correo);
                    db.SaveChanges();
                    db.Dispose();
                    if (correoRes != null)
                    {
                        int random = getrandom.Next(10000, 99999);

                        string CorreoEncriptado = objEncriptar.EncriptarData(objRecuperar.Correo);
                        string NumeroEncriptado = objEncriptar.EncriptarData(random.ToString());
                        string cuerpo = System.Configuration.ConfigurationManager.AppSettings["Path"].ToString() + "/RecuperarPassword/CambiarContraseña?id="+NumeroEncriptado+"&c="+CorreoEncriptado;
                        objCorreo.EnviarCorreo(objRecuperar.Correo, "Recuperación de contraseña","Para recuperar la contraseña ingresa al siguiente link: <a>"+cuerpo+"</a>");
                        ViewBag.CorreoExito = 1;
                        using(Data.MemoriesByCocoEntities db2= new Data.MemoriesByCocoEntities())
                        {
                            db2.PA_INS_SolicitaCambio(objRecuperar.Correo, random);
                            db2.SaveChanges();
                        }
                            
                        
                        
                        
                    }
                    else
                    {
                        ViewBag.CorreoNoExiste = 1;
                        return View("RecuperarContraseña", objRecuperar);
                    }
                }
            }catch(Exception ex)
            {

            }
            return View("RecuperarContraseña", objRecuperar);
        }

    }
}