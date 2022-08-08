using Albumes_MemoriesByCoco.LogicaNegocios;
using Albumes_MemoriesByCoco.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Albumes_MemoriesByCoco.Controllers
{
    public class PerfilController : Controller
    {
        // GET: Perfil
        private static int _nUsuario =0;
        public ActionResult Index()
        {
            if (Session["Usuario"] != null)
            {
                List<UsuarioModel> listaUsuario = new List<UsuarioModel>();
                listaUsuario = (List<UsuarioModel>)Session["Usuario"];
                _nUsuario = listaUsuario[0].nID;
                return View("MiPerfil");


            }
            return RedirectToAction("InicioSesion", "InicioSesion");

        }
        public JsonResult CancelarPedido()
        {
            try
            {

                    Session["NumeroPedido"] = null;
                
           
            
                    Session["PedidoUsuario"] = null;
               
            }
            catch(Exception ex)
            {
            using(Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    db.PA_InsertarBitacora(ex.Message, DateTime.Now, _nUsuario);
                }
            }

            return Json("", JsonRequestBehavior.AllowGet);
        }
        public ActionResult MiPerfil()
        {
            if (Session["Usuario"] == null)
            {

                return RedirectToAction("InicioSesion", "InicioSesion");

            }
            else
            {
            
                List<UsuarioModel> listaUsuario = new List<UsuarioModel>();
                listaUsuario = (List<UsuarioModel>)Session["Usuario"];
                _nUsuario = listaUsuario[0].nID;
                ViewBag.NombreCompletoUsuario = listaUsuario[0].strNombre + " " + listaUsuario[0].strApellido1 + " " + listaUsuario[0].strApellido2;

                if(listaUsuario[0].nRol == Constantes.RolAdministrador)
                {
                    ViewBag.Admin = 1;
                }
              
            }
         
            return View();
        }



        #region "Mantenimiento Precios"
        public JsonResult ConsultarPrecios()
        {
            List<PrecioModel> precios = new List<PrecioModel>();
            try
            {
                using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    var lista = db.PA_CONS_ConsultarPrecios().ToList();

                    precios = (from e in lista
                               select new PrecioModel
                               {
                                   IdPrecios=e.paq_precio,
                                   DescripcionPrecios=e.paq_descripcion,
                                   Valor=(decimal)e.paq_valor
                               }).ToList();
                }
            }
            catch (Exception ex)
            {
                using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    db.PA_InsertarBitacora(ex.Message, DateTime.Now, _nUsuario);
                }
            }

            return Json(precios, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ActualizarPrecios(int id, string valor)
        {
            Utilitarios objUtil = new Utilitarios();
            try
            {
                using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    

                    db.PA_MantenimientoPrecios((int)Utilitarios.eTipoOperacion.Actualizar, id, "", Decimal.Parse(valor));

                }
            }
            catch (Exception ex)
            {
                using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    db.PA_InsertarBitacora(ex.Message, DateTime.Now, _nUsuario);
                }
            }
            return Json("");
        }

        public JsonResult EliminarPrecios(int id)
        {
            try
            {
                using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                   
                    db.PA_MantenimientoPrecios((int)Utilitarios.eTipoOperacion.Eliminar, id, "", 0);
                }
            }
            catch (Exception ex)
            {
                using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    db.PA_InsertarBitacora(ex.Message, DateTime.Now, _nUsuario);
                }
            }
            return Json("");
        }

        public JsonResult AgregarPrecios(PrecioModel objPrecio)
        {
            try
            {
                using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    db.PA_MantenimientoPrecios((int)Utilitarios.eTipoOperacion.Insertar, -1,objPrecio.DescripcionPrecios, objPrecio.Valor);
                }
            }
            catch (Exception ex)
            {
                using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    db.PA_InsertarBitacora(ex.Message, DateTime.Now, _nUsuario);
                }
            }
            return Json("");
        }
        #endregion


        #region "Mantenimiento Preguntas"
        public JsonResult ConsultarPreguntas()
        {
            List<PreguntasModel> estados = new List<PreguntasModel>();
            try
            {
                using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    var lista = db.PA_CONS_ConsultarPreguntas().ToList();

                    estados = (from e in lista
                               select new PreguntasModel
                               {
                                   DescripcionPregunta = e.paq_descripcion,
                                   Inactiva = (bool)e.paq_inactivo,
                                   IdPregunta = e.paq_pregunta
                               }).ToList();
                }
            }
            catch (Exception ex)
            {
                using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    db.PA_InsertarBitacora(ex.Message, DateTime.Now, _nUsuario);
                }
            }

            return Json(estados, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CambiarInactivoPreguntas(int id, bool preguntas)
        {
            Utilitarios objUtil = new Utilitarios();
            try
            {
                using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    db.PA_Mantenimiento_Preguntas((int)Utilitarios.eTipoOperacion.Actualizar, preguntas, id, "");
                }
            }
            catch (Exception ex)
            {
                using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    db.PA_InsertarBitacora(ex.Message, DateTime.Now, _nUsuario);
                }
            }
            return Json("");
        }

        public JsonResult EliminarPreguntas(int id)
        {
            try
            {
                using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    db.PA_Mantenimiento_Preguntas((int)Utilitarios.eTipoOperacion.Eliminar, true, id, "");
                }
            }
            catch (Exception ex)
            {
                using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    db.PA_InsertarBitacora(ex.Message, DateTime.Now, _nUsuario);
                }
            }
            return Json("");
        }

        public JsonResult AgregarPreguntas(PreguntasModel objPreguntas)
        {
            try
            {
                using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    db.PA_Mantenimiento_Preguntas((int)Utilitarios.eTipoOperacion.Insertar, objPreguntas.Inactiva, -1, objPreguntas.DescripcionPregunta);
                }
            }
            catch (Exception ex)
            {
                using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    db.PA_InsertarBitacora(ex.Message, DateTime.Now, _nUsuario);
                }
            }
            return Json("");
        }

        #endregion

        #region "Mantenimiento Frases"
        [HttpPost]
        public JsonResult EliminarRegistroFrase(int id)
        {
            byte[] array = new byte[0];
            try
            {
                using(Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    db.PA_MantenimientoFrases_((int)Utilitarios.eTipoOperacion.Eliminar,id,"",true,array);
                    db.SaveChanges();



                }
            }catch(Exception ex)
            {
                using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    db.PA_InsertarBitacora(ex.Message, DateTime.Now, _nUsuario);
                }
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ConsultarRegistroFrase()
        {
            int? NumPags = null;
            double Num = 0;
            bool bloquear=false;
            

            try
            {

                using(Data.MemoriesByCocoEntities db= new Data.MemoriesByCocoEntities())
                {
                  NumPags= db.PA_CONS_ConsultarRegistrosFrases().FirstOrDefault();

                    if (NumPags != null)
                    {
                         Num = (double)NumPags / Constantes.CantidadFrasesxPagina;


                    }

                    PaginacionModel model = new PaginacionModel();
                    model.nMin = 1;
                    model.nMax = (Math.Ceiling(Num));

                    return Json(model, JsonRequestBehavior.AllowGet);


                 


                }
            }catch(Exception ex)
            {
                using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    db.PA_InsertarBitacora(ex.Message, DateTime.Now, _nUsuario);
                }
            }

            return Json(Num, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [System.Web.Services.WebMethod]
        public JsonResult MostrarFrases(int pagina)
        {
          

            List<FrasesModel> listaFrases = new List<FrasesModel>();
            FrasesModel model = new FrasesModel();
            try
            {
            
                listaFrases = model.SeleccionarFrases(pagina);
            }
            catch(Exception ex)
            {
                using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    db.PA_InsertarBitacora(ex.Message, DateTime.Now, _nUsuario);
                }
            }
         
        
            var JsonResult = Json(listaFrases, JsonRequestBehavior.AllowGet);
            JsonResult.MaxJsonLength = int.MaxValue;
            return JsonResult ;



        }
            [HttpPost]
        public  JsonResult AgregarFrase(FrasesModel objFrase)
        {
            List<FrasesModel> listaFrases = new List<FrasesModel>();    
            FrasesModel model = new FrasesModel();
            Utilitarios objUtil = new Utilitarios();

            try
            {
                
                
               

                if (objFrase.DescripcionFrase == "" || objFrase.DescripcionFrase == null )
                {
                    throw new Exception();
                }

                using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    db.PA_MantenimientoFrases_((int)Utilitarios.eTipoOperacion.Insertar,-1,objFrase.DescripcionFrase, objFrase.Inactivo,objFrase.Sticker);
                    db.SaveChanges();
                }

                

            }
            catch (Exception ex)
            {
                using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    db.PA_InsertarBitacora(ex.Message, DateTime.Now, _nUsuario);
                }
            }
            return Json(listaFrases, JsonRequestBehavior.AllowGet) ;
        }

        
        public JsonResult ConsultarFraseLike(string strFrase)
        {
            List<FrasesModel> listaFrases = new List<FrasesModel>();    


            try
            {
                using(Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    var lista = db.PA_CONS_ConsultarFrasesLike_V2(strFrase).ToList();

                    listaFrases = (from e in lista
                                   select new FrasesModel
                                   {
                                       DescripcionFrase = e.paq_descripcion,
                                       IdFrase = e.paq_frase,
                               
                                       Inactivo = (bool)e.paq_inactivo
                                   }).ToList();
                }
            }catch(Exception ex)
            {
                using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    db.PA_InsertarBitacora(ex.Message, DateTime.Now, _nUsuario);
                }
            }


            return Json(listaFrases, JsonRequestBehavior.AllowGet);
        }


        public JsonResult CambiarInactivoFrase(int id, bool estado)
        {
            try
            {
                using(Data.MemoriesByCocoEntities db =new Data.MemoriesByCocoEntities())
                {
                    db.PA_ActivarFrase(id, estado);


                }
            }catch(Exception ex)
            {
                using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    db.PA_InsertarBitacora(ex.Message, DateTime.Now, _nUsuario);
                }
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ConsultarImgFrase(int id)
        {
            try
            {
                using(Data.MemoriesByCocoEntities db= new Data.MemoriesByCocoEntities())
                {
                    var sticker=db.PA_CONS_ConsultarStickerxID(id).FirstOrDefault();

                    string strSticker = Convert.ToBase64String(sticker);
                    var JsonResult = Json(strSticker, JsonRequestBehavior.AllowGet);
                    JsonResult.MaxJsonLength = int.MaxValue;

                    return JsonResult;
                 
                }
            }catch(Exception ex)
            {
                using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    db.PA_InsertarBitacora(ex.Message, DateTime.Now, _nUsuario);
                }
            }

            return Json("");
        }



        #endregion

        #region "Mantenimiento Vinil"

        [HttpPost]
        public JsonResult EliminarRegistroVinil(int id)
        {
            byte[] array = new byte[0];
            try
            {
                using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    db.PA_MantenimientoVinil((int)Utilitarios.eTipoOperacion.Eliminar, id,"", "", true, array);
                    db.SaveChanges();



                }
            }
            catch (Exception ex)
            {
                using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    db.PA_InsertarBitacora(ex.Message, DateTime.Now, _nUsuario);
                }
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ConsultarRegistroVinil()
        {
            int? NumPags = null;
            double Num = 0;
            bool bloquear = false;


            try
            {
                PaginacionModel model = new PaginacionModel();  
                using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    NumPags = db.PA_CONS_ConsultarRegistrosVinil().FirstOrDefault();

                    if (NumPags != null)
                    {
                        Num = (double)NumPags / Constantes.CantidadFrasesxPagina;


                    }
                    model.nMin = 1;
                    model.nMax = (Math.Ceiling(Num));

                    return Json(model, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    db.PA_InsertarBitacora(ex.Message, DateTime.Now, _nUsuario);
                }
            }

            return Json(Num, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [System.Web.Services.WebMethod]
        public JsonResult MostrarVinil(int pagina)
        {


            List<VinilModel> listaVinil = new List<VinilModel>();
            VinilModel model = new VinilModel();
            try
            {

                using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    var response = db.PA_CONS_ConsultarVinil_Paginacion(pagina, Constantes.CantidadFrasesxPagina);


                    listaVinil = (from e in response
                                   select new VinilModel
                                   {
                                       idVinil = e.paq_vinil,
                                       nombre=e.paq_nombre,
                                       descripcion = e.paq_descripcion,
                                       Inactivo = (bool)e.paq_inactivo,
                                      
                                   }).ToList();

                }
            }
            catch (Exception ex)
            {
                using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    db.PA_InsertarBitacora(ex.Message, DateTime.Now, _nUsuario);
                }
            }


            var JsonResult = Json(listaVinil, JsonRequestBehavior.AllowGet);
            JsonResult.MaxJsonLength = int.MaxValue;
            return JsonResult;



        }
        [HttpPost]
        public JsonResult AgregarVinil(VinilModel objVinil)
        {
            List<VinilModel> listaVinil = new List<VinilModel>();
            FrasesModel model = new FrasesModel();
            Utilitarios objUtil = new Utilitarios();

            try
            {


                HttpPostedFileBase Filebase = Request.Files[0];

                WebImage image = new WebImage(Filebase.InputStream);

                objVinil.Imagen = image.GetBytes();
                objVinil.Imagen = objUtil.CreateThumbnail(objVinil.Imagen, Constantes.CalidadImagen);

                if (objVinil.descripcion == "" || objVinil.descripcion == null || objVinil.Imagen == null)
                {
                    throw new Exception();
                }

                using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    db.PA_MantenimientoVinil((int)Utilitarios.eTipoOperacion.Insertar, -1,objVinil.nombre, objVinil.descripcion, objVinil.Inactivo, objVinil.Imagen);
                    db.SaveChanges();
                }



            }
            catch (Exception ex)
            {
                using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    db.PA_InsertarBitacora(ex.Message, DateTime.Now, _nUsuario);
                }
            }
            return Json(listaVinil, JsonRequestBehavior.AllowGet);
        }


        public JsonResult ConsultarVinilLike(string strVinil)
        {
            List<VinilModel> listaFrases = new List<VinilModel>();


            try
            {
                using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    var lista = db.PA_CONS_ConsultarVinilLike(strVinil).ToList();

                    listaFrases = (from e in lista
                                   select new VinilModel
                                   {
                                       idVinil = e.paq_vinil,
                                       nombre = e.paq_nombre,
                                       descripcion = e.paq_descripcion,
                                       Inactivo = (bool)e.paq_inactivo
                                   }).ToList();
                }
            }
            catch (Exception ex)
            {
                using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    db.PA_InsertarBitacora(ex.Message, DateTime.Now, _nUsuario);
                }
            }


            return Json(listaFrases, JsonRequestBehavior.AllowGet);
        }


        public JsonResult CambiarInactivoVinil(int id, bool estado)
        {
            try
            {
                using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    db.PA_ActivarVinil(id, estado);


                }
            }
            catch (Exception ex)
            {
                using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    db.PA_InsertarBitacora(ex.Message, DateTime.Now, _nUsuario);
                }
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ConsultarImgVinil(int id)
        {
            try
            {
                using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    var sticker = db.PA_CONS_ConsultarImgxIDVinil(id).FirstOrDefault();

                    string strSticker = Convert.ToBase64String(sticker);
                    var JsonResult = Json(strSticker, JsonRequestBehavior.AllowGet);
                    JsonResult.MaxJsonLength = int.MaxValue;

                    return JsonResult;

                }
            }
            catch (Exception ex)
            {
                using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    db.PA_InsertarBitacora(ex.Message, DateTime.Now, _nUsuario);
                }
            }

            return Json("");
        }

        #endregion

        #region "Mantenimiento Estados"


        public JsonResult ConsultarEstados()
        {
            List<EstadoModel> estados = new List<EstadoModel>();    
            try
            {
                using(Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                   var lista= db.PA_CONS_ConsultarEstados().ToList();

                    estados = (from e in lista select new EstadoModel
                    {
                        nombre=e.paq_descripcion,
                        notifica=(bool)e.paq_notifica,
                        idEstado=e.paq_estado_pedido
                    }).ToList();
                }
            }catch(Exception ex)
            {
                using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    db.PA_InsertarBitacora(ex.Message, DateTime.Now, _nUsuario);
                }
            }

            return Json(estados, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CambiarNotificaEstado(int id, bool estado)
        {
            Utilitarios objUtil = new Utilitarios();
            try
            {
                using(Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    db.PA_Mantenimiento_PAQ_ESTADO_PEDIDO((int)Utilitarios.eTipoOperacion.Actualizar, estado, id, "");
                    db.SaveChanges();
                  




                }
            }catch(Exception ex)
            {
                using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    db.PA_InsertarBitacora(ex.Message, DateTime.Now, _nUsuario);
                }
            }
            return Json("");
        }

        public JsonResult EliminarEstado(int id)
        {
            try
            {
                using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    db.PA_Mantenimiento_PAQ_ESTADO_PEDIDO((int)Utilitarios.eTipoOperacion.Eliminar, true, id, "");
                }
            }
            catch (Exception ex)
            {
                using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    db.PA_InsertarBitacora(ex.Message, DateTime.Now, _nUsuario);
                }
            }
            return Json("");
        }

        public JsonResult AgregarEstado(EstadoModel objEstado)
        {
            try
            {
                using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    db.PA_Mantenimiento_PAQ_ESTADO_PEDIDO((int)Utilitarios.eTipoOperacion.Insertar, objEstado.notifica, -1, objEstado.nombre);
                }
            }
            catch (Exception ex)
            {
                using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    db.PA_InsertarBitacora(ex.Message, DateTime.Now, _nUsuario);
                }
            }
            return Json("");
        }


        #endregion

        #region "Crear Album"
       
        public JsonResult ObtenerNumeroPedido()
        {
            try
            {
                using(Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    int? NumeroPedido = db.PA_Obtener_NumeroPedido().FirstOrDefault() ;

                    if (Session["NumeroPedido"] != null)
                    {
                        Session["NumeroPedido"] = null;
                    }
                    if (Session["PedidoUsuario"] != null)
                    {
                        Session["PedidoUsuario"] = null;
                    }


                    Session["NumeroPedido"] = NumeroPedido;
                }
            }catch (Exception ex)
            {
                using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    db.PA_InsertarBitacora(ex.Message, DateTime.Now, _nUsuario);
                }
            }
            return Json("");
        }



        public JsonResult AlbumPredefinido(int nPredefinido)
        {
            PedidoModel objPedido = new PedidoModel();
            objPedido.Personalizado = nPredefinido;
            Session["PedidoUsuario"] = objPedido;

            return Json("");
        }

        public JsonResult TipoAlbum(string strTipoAlbum)
        {
            PedidoModel objPedido = new PedidoModel();
            try
            {
                

              
                    objPedido=(PedidoModel)Session["PedidoUsuario"];
                    objPedido.TipoAlbum = strTipoAlbum;
                    Session["PedidoUsuario"] = objPedido;

                
                

            }catch(Exception ex)
            {
                using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    db.PA_InsertarBitacora(ex.Message, DateTime.Now, _nUsuario);
                }
            }
            return Json("");
        }


        public JsonResult ConsultarVinilUPaginadosSelect(int pag)
        {
            List<VinilModel> listaFrases = new List<VinilModel>();
            try
            {
                using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    var response = db.PA_CONS_ConsultarVinil_Paginacion_Img(pag, Constantes.CantidadFrasexPaginaVinil);


                    listaFrases = (from e in response
                                   select new VinilModel
                                   {
                                       idVinil = e.paq_vinil,
                                       nombre = e.paq_nombre,
                                       descripcion = e.paq_descripcion,
                                       Inactivo = (bool)e.paq_inactivo,
                                       strImagen= Convert.ToBase64String(e.paq_img)
                                   }).ToList();

                }
            }
            catch (Exception ex)
            {
                using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    db.PA_InsertarBitacora(ex.Message, DateTime.Now, _nUsuario);
                }
            }

            return Json(listaFrases, JsonRequestBehavior.AllowGet);
        }


        public JsonResult ConsultarVinilUStickers()
        {
            double NumeroPag = 0;
            try
            {
                using(Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                  int? NumeroPaginas=  db.PA_CONS_ConsultarRegistroVinilActivo().FirstOrDefault();

                    if (NumeroPaginas != null)
                    {
                        NumeroPag= (double)NumeroPaginas / Constantes.CantidadFrasexPaginaSticker;


                    }
                    return Json(NumeroPag, JsonRequestBehavior.AllowGet);
                }
            }catch(Exception ex)
            {
                using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    db.PA_InsertarBitacora(ex.Message, DateTime.Now, _nUsuario);
                }
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ConsultarVinilULike_(string strFrase)
        {
            List<VinilModel> listaFrases = new List<VinilModel>();
            try
            {
                using(Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    var response=db.PA_CONS_ConsultarVinilLike_Img(strFrase).ToList();



                    listaFrases = (from e in response
                                   select new VinilModel
                                   {
                                       idVinil = e.paq_vinil,
                                       nombre = e.paq_nombre,
                                       descripcion = e.paq_descripcion,
                                       Inactivo = (bool)e.paq_inactivo,
                                       strImagen = Convert.ToBase64String(e.paq_img)
                                   }).ToList();

                }
            }
            catch (Exception ex)
            {
                using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    db.PA_InsertarBitacora(ex.Message, DateTime.Now, _nUsuario);
                }
            }
            return Json(listaFrases, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GuardarVinilElegido(int idVinil)
        {
            PedidoModel objPedido = new PedidoModel();
            try
            {
                if (Session["PedidoUsuario"] != null)
                {
                    objPedido = (PedidoModel)Session["PedidoUsuario"];

                    objPedido.TipoVinil = idVinil;

                    Session["PedidoUsuario"] = objPedido;
                }
                else
                {
                    Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    return Json(new { IsCreated = false, ErrorMessage = "Termino el tiempo para realizar el pedido..." });
                }
               

                
            }
            catch (Exception ex)
            {

                using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    db.PA_InsertarBitacora(ex.Message, DateTime.Now, _nUsuario);
                    db.SaveChanges();
                }
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return Json(new { IsCreated = false, ErrorMessage = "Error en el servidor" });


            }

            return Json("", JsonRequestBehavior.AllowGet);
        }


        public JsonResult ConsultarFraseUsuario(int pagina)
        {
            List<FrasesModel> listaFrases = new List<FrasesModel>();
            List<FrasesModel> listaFrasesSesion = new List<FrasesModel>();

            FrasesModel frases = new FrasesModel();
            PedidoModel objPedido = new PedidoModel();
            try
            {
                if (Session["PedidoUsuario"] != null)
                {
                    objPedido = (PedidoModel)Session["PedidoUsuario"];

                    for (int i = 0; i < objPedido.listaFrases.Count; i++)
                    {
                        FrasesModel objFrase = new FrasesModel();
                        objFrase.IdFrase = objPedido.listaFrases[i];
                        listaFrasesSesion.Add(objFrase);
                    }


                }
                else
                {

                }

                using (Data.MemoriesByCocoEntities db= new Data.MemoriesByCocoEntities())
                {
                  var response=  db.PA_CONS_ConsultarFrases_Paginacion_Usuario(pagina, Constantes.CantidadFrasexPaginaUsuario).ToList();


                    listaFrases = (from e in response
                                   select new FrasesModel
                                   {
                                       IdFrase = e.paq_frase,
                                       DescripcionFrase = e.paq_descripcion,
                                       Inactivo = (bool)e.paq_inactivo,
                                       check = frases.FraseCheckeada(e.paq_frase, listaFrasesSesion)
                                   }).ToList();

                    return Json(listaFrases, JsonRequestBehavior.AllowGet);
                }
            }catch (Exception ex)
            {
                using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    db.PA_InsertarBitacora(ex.Message, DateTime.Now, _nUsuario);
                    db.SaveChanges();
                }
            }

            return Json(null, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult ConsultarRegistroFrase_Usuario()
        {
            int? NumPags = null;
            double Num = 0;
            bool bloquear = false;


            try
            {
                PaginacionModel model = new PaginacionModel();
                using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    NumPags = db.PA_CONS_ConsultarRegistrosFrases_Usuario().FirstOrDefault();

                    if (NumPags != null)
                    {
                        Num = (double)NumPags / Constantes.CantidadFrasexPaginaUsuario;


                    }

                    model.nMin = 1;
                    model.nMax = (Math.Ceiling(Num));

                    return Json(model, JsonRequestBehavior.AllowGet);

                }
            }
            catch (Exception ex)
            {
                using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    db.PA_InsertarBitacora(ex.Message, DateTime.Now, _nUsuario);
                    db.SaveChanges();
                }
            }

            return Json(Num, JsonRequestBehavior.AllowGet);
        }


        public JsonResult ConsultarFraseLike_Usuario(string strFrase)
        {
            List<FrasesModel> listaFrases = new List<FrasesModel>();
            List<FrasesModel> listaFrasesSesion = new List<FrasesModel>();
           
            FrasesModel frases = new FrasesModel();
            PedidoModel objPedido= new PedidoModel();

            try
            {

                if (Session["PedidoUsuario"] != null)
                {
                    objPedido = (PedidoModel)Session["PedidoUsuario"];

                    for(int i = 0; i < objPedido.listaFrases.Count; i++) {
                        FrasesModel objFrase= new FrasesModel();
                        objFrase.IdFrase= objPedido.listaFrases[i];
                        listaFrasesSesion.Add(objFrase);
                  }

                   
                }
                else
                {

                }
                using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    var lista = db.PA_CONS_ConsultarFrasesLike_Usuario(strFrase).ToList();

                    listaFrases = (from e in lista
                                   select new FrasesModel
                                   {
                                       DescripcionFrase = e.paq_descripcion,
                                       IdFrase = e.paq_frase,
                                       Inactivo = (bool)e.paq_inactivo,
                                       check=frases.FraseCheckeada(e.paq_frase, listaFrasesSesion)
                                   }).ToList();
                }
            }
            catch (Exception ex)
            {
                using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    db.PA_InsertarBitacora(ex.Message, DateTime.Now, _nUsuario);
                    db.SaveChanges();
                }
            }


            return Json(listaFrases, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public JsonResult AgregarFrase_Usuario(string strFrase)
        {
            List<FrasesModel> listaFrases = new List<FrasesModel>();
            FrasesModel model = new FrasesModel();
            Utilitarios objUtil = new Utilitarios();
            PedidoModel objPedido = new PedidoModel();

            try
            {


               

                if (strFrase == "" || strFrase == null )
                {
                    throw new Exception();
                }

                using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                   int? codigo= db.PA_MantenimientoFrases_((int)Utilitarios.eTipoOperacion.Insertar, -1, strFrase, true , null).FirstOrDefault();
                    db.SaveChanges();


                    if (Session["PedidoUsuario"] != null)
                    {
                        objPedido = (PedidoModel)Session["PedidoUsuario"];
                    
                        objPedido.listaFrases.Add((int)codigo);

                        Session["PedidoUsuario"] = objPedido;
                    }
                    else
                    {
                        Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        return Json(new { IsCreated = false, ErrorMessage = "Termino el tiempo para realizar el pedido..." });
                    }


                }



            }
            catch (Exception ex)
            {
                using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    db.PA_InsertarBitacora(ex.Message, DateTime.Now, _nUsuario);
                    db.SaveChanges();
                }
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return Json(new { IsCreated = false, ErrorMessage = "Error en el servidor" });
            }
            return Json(listaFrases, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SeleccionFraseUsuario(int idFrase)
        {
            PedidoModel objPedido = new PedidoModel();
            try
            {
                if (Session["PedidoUsuario"] != null)
                {
                    objPedido = (PedidoModel)Session["PedidoUsuario"];
                
                    objPedido.listaFrases.Add(idFrase);



                    Session["PedidoUsuario"] = objPedido;
                }
                else
                {
                    Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    return Json(new { IsCreated = false, ErrorMessage = "Termino el tiempo para realizar el pedido..." });
                }

            } catch (Exception ex)
            {
                using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    db.PA_InsertarBitacora(ex.Message, DateTime.Now, _nUsuario);
                    db.SaveChanges();
                }
            }

            return Json("", JsonRequestBehavior.AllowGet);
        }


        public JsonResult DeseleccionFraseUsuario(int idFrase)
        {
            PedidoModel objPedido = new PedidoModel();
            try
            {
                if (Session["PedidoUsuario"] != null)
                {
                    objPedido = (PedidoModel)Session["PedidoUsuario"];


                    objPedido.listaFrases.Remove(idFrase);
                    

                    Session["PedidoUsuario"] = objPedido;
                }
                else
                {
                    Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    return Json(new { IsCreated = false, ErrorMessage = "Termino el tiempo para realizar el pedido..." });
                }

            }
            catch (Exception ex)
            {
                using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    db.PA_InsertarBitacora(ex.Message, DateTime.Now, _nUsuario);
                    db.SaveChanges();
                }
            }

            return Json("", JsonRequestBehavior.AllowGet);
        }

        public JsonResult ConsultarFrasesSeleccionadasUsuario()
        {
            PedidoModel objPedido= new PedidoModel();
            
            List<FrasesModel> listaFrases = new List<FrasesModel>();
            try
            {
                if (Session["PedidoUsuario"] != null)
                {
                    objPedido = (PedidoModel)Session["PedidoUsuario"];


                

                    using(Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                    {
                        for(int i=0;i < objPedido.listaFrases.Count; i++)
                        {
                           var response= db.PA_CONS_ConsultarFrasexID(objPedido.listaFrases[i]).FirstOrDefault();


                            FrasesModel objFrase = new FrasesModel();
                            objFrase.IdFrase= response.paq_frase;
                            objFrase.DescripcionFrase=response.paq_descripcion;
                            listaFrases.Add(objFrase);


                        }
                        
                    }
      
                   
                   
                }
                else
                {
                    Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    return Json(new { IsCreated = false, ErrorMessage = "Termino el tiempo para realizar el pedido..." });
                }
            }
            catch(Exception ex)
            {
                using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    db.PA_InsertarBitacora(ex.Message, DateTime.Now, _nUsuario);
                    db.SaveChanges();
                }
            }
            return Json(listaFrases, JsonRequestBehavior.AllowGet);
        }

        #region "Preguntas"
        public JsonResult ConsultarPreguntasUsuario()
        {
            List<PreguntasModel> listaPreguntas = new List<PreguntasModel>();
            try
            {
                using(Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                  var response=  db.PA_CONS_ConsultarPreguntaxID(-1).ToList();


                    listaPreguntas= (from e in response select new PreguntasModel
                    {
                        IdPregunta=e.paq_pregunta,
                        DescripcionPregunta=e.paq_descripcion,
                        Inactiva=(bool)e.paq_inactivo
                    }).ToList();

                }
            }catch(Exception ex)
            {
                using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    db.PA_InsertarBitacora(ex.Message, DateTime.Now, _nUsuario);
                    db.SaveChanges();
                }
            }
            return Json(listaPreguntas, JsonRequestBehavior.AllowGet);
        }

        public JsonResult InsertarImagenPedido(string Imagen)
        {

            if (Session["CantidadFotos"] != null)
            {
                int CantidadFotos = (int)Session["CantidadFotos"] + 1;
                Session["CantidadFotos"] = CantidadFotos;
            }
            else
            {
                int CantidadFotos = 1;
                Session["CantidadFotos"] = CantidadFotos;
            }


            byte[] ImagenPosted = new Byte[64];
            int? idImagen = 0;
            try
            {

                HttpPostedFileBase Filebase = Request.Files[0];

                WebImage image = new WebImage(Filebase.InputStream);

                ImagenPosted = image.GetBytes();
                //  objVinil.Imagen = objUtil.CreateThumbnail(objVinil.Imagen, Constantes.CalidadImagen);


                using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    idImagen= db.PA_INS_InsertarImagenPedidoUsuario((int)Session["NumeroPedido"], ImagenPosted).FirstOrDefault();
                   // idImagen = db.PA_INS_InsertarImagenPedidoUsuario(20, ImagenPosted).FirstOrDefault();


               
                }
            }
            catch (Exception ex)
            {
                using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    db.PA_InsertarBitacora(ex.Message, DateTime.Now, _nUsuario);
                    db.SaveChanges();
                }
            }
            return Json(idImagen, JsonRequestBehavior.AllowGet);
        }

        public JsonResult EliminarImagenPedido(int IdImagen)
        {
            try
            {


                if (Session["CantidadFotos"] != null)
                {
                    int CantidadFotos = (int)Session["CantidadFotos"]-1;
                    Session["CantidadFotos"] = CantidadFotos;
                }

                using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    db.PA_Eliminar_ImagenPedido(IdImagen);
                }   
            }catch(Exception ex)
            {
                using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    db.PA_InsertarBitacora(ex.Message, DateTime.Now, _nUsuario);
                    db.SaveChanges();
                }
            }
            return Json("");
        }

        public JsonResult InsertarRespuestasUsuario(string response, int idPregunta)
        {
            try
            {
                using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    // db.PA_INS_InsertarRespuestasUsuario(idPregunta, response, (int)Session["NumeroPedido"]);

                    db.PA_INS_InsertarRespuestasUsuario(idPregunta, response, (int)Session["NumeroPedido"]);

                }
            }
            catch (Exception ex)
            {
                using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    db.PA_InsertarBitacora(ex.Message, DateTime.Now, _nUsuario);
                    db.SaveChanges();
                }
            }
            return Json("");
        }
        #endregion


        #region "Precios Usuario"
        public JsonResult ConsultarPreciosUsuarios()
        {
            List<PrecioModel> precios = new List<PrecioModel>();
            PrecioModel objPrecio = new PrecioModel();
            int cantidadFotos = 0;
            try
            {
                if (Session["CantidadFotos"] != null)
                {
                    cantidadFotos = (int)Session["CantidadFotos"];
                }
               
                using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    var lista = db.PA_CONS_ConsultarPrecios().ToList();

                    precios = (from e in lista
                               select new PrecioModel
                               {
                                   
                                   DescripcionPrecios = e.paq_descripcion,
                                   Valor = e.paq_descripcion.Contains("foto adicional") ? (decimal)(objPrecio.ObtenerValorxFoto(e.paq_valor, cantidadFotos)) : (decimal)e.paq_valor
                               }).ToList();

                    decimal total = 0;
                    for(int i = 0; i < precios.Count; i++)
                    {
                        if (!precios[i].DescripcionPrecios.Contains("%"))
                        {
                            total = total + precios[i].Valor;
                        }
                       

                    }  

                    for(int i = 0; i < precios.Count; i++)
                    {
                        if (precios[i].DescripcionPrecios.Contains("%"))
                        {
                            objPrecio=objPrecio.CalcularDescuento(precios[i].DescripcionPrecios, precios[i].Valor, total);
                            precios.Remove(precios[i]);
                            precios.Add(objPrecio);
                            

                        }

                        if(precios[i].DescripcionPrecios.Contains("foto adicional") && precios[i].Valor==0)
                        {
                            precios.Remove(precios[i]);
                            i--;
                        }

                    }




                }
            }
            catch (Exception ex)
            {
                using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    db.PA_InsertarBitacora(ex.Message, DateTime.Now, _nUsuario);
                    db.SaveChanges();
                }
            }

            return Json(precios, JsonRequestBehavior.AllowGet);
        }


         public JsonResult InformacionUsuarioFinal()
        {
            List<UsuarioModel> listaUsuarios = new List<UsuarioModel>();
            UsuarioModel response = new UsuarioModel();
            try
            {
                if (Session["Usuario"] != null)
                {
                    listaUsuarios = (List<UsuarioModel>)Session["Usuario"];


                    response.strDireccion = listaUsuarios[0].strDireccion;
                    response.strTel = listaUsuarios[0].strTel;
                   

                }
               
            }
            catch(Exception ex){
                using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    db.PA_InsertarBitacora(ex.Message, DateTime.Now, _nUsuario);
                    db.SaveChanges();
                }
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ActualizarInformacionFinal(string direccion, string telefono)
        {
            List<UsuarioModel> listaUsuarios = new List<UsuarioModel>();
            UsuarioModel response = new UsuarioModel();
            try
            {
                if (Session["Usuario"] != null)
                {
                    listaUsuarios = (List<UsuarioModel>)Session["Usuario"];

                    using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                    {
                        db.PA_ActualizarInfoUsuario((int)Utilitarios.eTipoActualizaInfo.Envio, listaUsuarios[0].nID, "",
                                                "", "", "", telefono, direccion);
                    }
                }
                  
                
            }
            catch(Exception ex)
            {
                using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    db.PA_InsertarBitacora(ex.Message, DateTime.Now, _nUsuario);
                    db.SaveChanges();
                }
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }


        #endregion


        #endregion



        #region "Configuración Usuario"

        public JsonResult CargarInformacionPersonalConfig()
        {
            List<UsuarioModel> listaUsuarios = new List<UsuarioModel>();
            UsuarioModel response = new UsuarioModel();
            try
            {
                if (Session["Usuario"] != null)
                {
                    listaUsuarios = (List<UsuarioModel>)Session["Usuario"];

                    using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                    {
                        var respuesta = db.PA_CONS_ConsultarUsuario(listaUsuarios[0].nID).FirstOrDefault();

                        response.strNombre = respuesta.seg_nombre;
                        response.strApellido1 = respuesta.seg_apellido1;
                        response.strApellido2 = respuesta.seg_apellido2;
                        response.strCorreo = respuesta.seg_correo;
                        response.strDireccion = respuesta.seg_direccion;
                        response.strTel = respuesta.seg_tel;
                    }

                }

            }
            catch (Exception ex)
            {
                using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    db.PA_InsertarBitacora(ex.Message, DateTime.Now, _nUsuario);
                    db.SaveChanges();
                }
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ActualizarInformacionConfig(UsuarioModel objUsuario)
        {
            bool response = true;
            List<UsuarioModel> listaUsuarios = new List<UsuarioModel>();    
            Utilitarios objUtil = new Utilitarios();
            try
            {

                if (Session["Usuario"] != null)
                {
                    listaUsuarios = (List<UsuarioModel>)Session["Usuario"];   

                }



                    using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                   var correo= db.PA_CONS_ConsultarEmailRegistrado(objUsuario.strCorreo);


                    if (correo != null && objUsuario.strCorreo!=listaUsuarios[0].strCorreo)
                    {
                        response = false;
                        return Json(response, JsonRequestBehavior.AllowGet);
                    }
                  

                }

                    using(Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    db.PA_ActualizarInfoUsuario((int)Utilitarios.eTipoActualizaInfo.Configuracion, listaUsuarios[0].nID, objUsuario.strNombre,
                        objUsuario.strApellido1, objUsuario.strApellido2, objUsuario.strCorreo, objUsuario.strTel, objUsuario.strDireccion);
                }

               
                listaUsuarios[0].strNombre = objUsuario.strNombre;  
                listaUsuarios[0].strApellido1 = objUsuario.strApellido1;
                listaUsuarios[0].strApellido2 = objUsuario.strApellido2;
                listaUsuarios[0].strTel= objUsuario.strTel; 
                listaUsuarios[0].strCorreo = objUsuario.strCorreo;
                listaUsuarios[0].strDireccion = objUsuario.strDireccion;
                Session["Usuario"] = listaUsuarios;
            
                               







            }
            catch(Exception ex)
            {
                using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    db.PA_InsertarBitacora(ex.Message, DateTime.Now, _nUsuario);
                    db.SaveChanges();
                }
            }

            return Json(response, JsonRequestBehavior.AllowGet);

        }
        public JsonResult AsignarValoresPedidoFinal(string monto,string transferencia)
        {
            PedidoModel objPedido = new PedidoModel();
            try
            {

              

                if (Session["PedidoUsuario"] != null)
                {
                    objPedido = (PedidoModel)Session["PedidoUsuario"];
                    objPedido.strTransferencia = transferencia;
                    objPedido.nMonto = Decimal.Parse(monto);
                    objPedido.nEstado = Constantes.EstadoRegistrado;

                    Session["PedidoUsuario"] = objPedido;
                }
            }catch(Exception ex)
            {
                using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    db.PA_InsertarBitacora(ex.Message, DateTime.Now, _nUsuario);
                    db.SaveChanges();
                }
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }

        public JsonResult ConsultarCuentaBancaria()
        {

            try
            {
                using(Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    var r= db.PA_CONS_ConsultarCuentaBancaria(Constantes.CuentaBD).FirstOrDefault();


                    return Json(r, JsonRequestBehavior.AllowGet);
                }
            }catch(Exception ex)
            {
                using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    db.PA_InsertarBitacora(ex.Message, DateTime.Now, _nUsuario);
                    db.SaveChanges();
                }
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }
        public JsonResult RegistrarPedido()
        {
            int nPedido = 0;
            List<UsuarioModel> listaUsuarios = new List<UsuarioModel>();
            PedidoModel objPedido = new PedidoModel();
            try
            {

                

                if(Session["Usuario"]!=null && Session["PedidoUsuario"]!=null && Session["NumeroPedido"]!=null )
                {

                    nPedido = (int)Session["NumeroPedido"];
                listaUsuarios = (List<UsuarioModel>)Session["Usuario"];
                objPedido = (PedidoModel)Session["PedidoUsuario"];

                using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {

                   
                    objPedido.Personalizado=  objPedido.Personalizado==-1?null:objPedido.Personalizado;
                        objPedido.TipoVinil = objPedido.TipoVinil <= 0 ? null : objPedido.TipoVinil;

                    db.PA_RegistrarPedido_V2((int)Session["NumeroPedido"],listaUsuarios[0].nID, objPedido.Personalizado, objPedido.TipoAlbum, objPedido.TipoVinil,objPedido.strTransferencia,
                        objPedido.nMonto, Constantes.EstadoRegistrado, DateTime.Now);


                        if (!objPedido.strTransferencia.Contains("Pagado con mis puntos"))
                        {
                            db.PA_AsignarPuntos(listaUsuarios[0].nID, Constantes.PuntosPorAlbum);
                        }
                       
                        

                


                    if(objPedido.listaFrases.Count>0)
                        {
                            for(int i = 0; i < objPedido.listaFrases.Count;i++)
                            {
                                db.PA_InsertarFrasesxPedido((int)Session["NumeroPedido"], objPedido.listaFrases[i]);
                                db.SaveChanges();
                            }
                        } 


                }


                using(Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                        if (Session["CantidadFotos"] != null)
                        {
                            int nCantidadFotos = (int)Session["CantidadFotos"];
                            if (nCantidadFotos > Constantes.CantidadImagenesSinCobro)
                            {
                                db.PA_AsignarPuntos(listaUsuarios[0].nID, (nCantidadFotos - Constantes.CantidadImagenesSinCobro) * Constantes.PuntosPorImagen);
                            }
                        }
                    
                }




                }
                else
                {
                  
                }
            }
            catch(Exception ex)
            {
                using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    db.PA_InsertarBitacora(ex.Message, DateTime.Now, _nUsuario);
                    db.SaveChanges();
                }
            }

            return Json(nPedido, JsonRequestBehavior.AllowGet);
        }






        #endregion


        #region "Puntos"
        public JsonResult QuitarPuntos()
        {
            List<UsuarioModel> objUsuario = new List<UsuarioModel>();
           

                if (Session["Usuario"] != null)
            {
                objUsuario = (List<UsuarioModel>)Session["Usuario"];


                using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    db.PA_EliminarPuntos(objUsuario[0].nID);
                    db.SaveChanges();
                }
            }
            else
            {
              


                using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    db.PA_EliminarPuntos(_nUsuario);
                    db.SaveChanges();
                }
            }


            

            return Json("", JsonRequestBehavior.AllowGet);
        }
        public JsonResult ConsultarPuntos()
        {
            int contador = 0;
            List<UsuarioModel> objUsuario = new List<UsuarioModel>();
            try
            {
                if (Session["Usuario"] != null)
                {
                    objUsuario = (List<UsuarioModel>)Session["Usuario"];

                    using(Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                    {
                        var response=   db.PA_CONS_ConsultarPuntosxUsuario(objUsuario[0].nID).ToList();

                        

                        for(int i = 0; i < response.Count; i++)
                        {
                            contador = (int)response[i] + contador;
                        }


                    }

                }
                }catch(Exception ex)
            {
                using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    db.PA_InsertarBitacora(ex.Message, DateTime.Now, _nUsuario);
                    db.SaveChanges();
                }
            }


            return Json(contador, JsonRequestBehavior.AllowGet);

        }




        #endregion



        #region "Consulta Pedido"

        public JsonResult ConsultarPedido(int pagina, int? rol)
        {
            List<PedidoModel> listaPedido = new List<PedidoModel>();
            int filter = 0;
            List<UsuarioModel> listaUsuario = new List<UsuarioModel>();
            try
            {

                if (Session["Usuario"] != null)
                {

                    listaUsuario = (List<UsuarioModel>)Session["Usuario"];

                    if(rol==1 && listaUsuario[0].nRol == Constantes.RolAdministrador)
                    {
                        filter = -1;
                    }
                    else
                    {
                        
                        filter = listaUsuario[0].nID;
                    }
                

                    if (filter != 0)
                    {

                        using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                        {
                            var response = db.PA_CONS_ConsultarPedidos_Paginados_v4(filter, pagina, Constantes.CantidadFrasesxPagina).ToList();


                            listaPedido = (from e in response
                                           select new PedidoModel
                                           {
                                               IdPedido = e.paq_pedido,
                                               nombreCompleto = e.seg_nombre_completo,
                                               strEstado = e.paq_descripcion_estado,
                                               fechaPedido = ((DateTime)e.paq_fecha).ToString("dd/MM/yyyy"),
                                               nEstado=(int)e.paq_estado
                                           }).ToList();

                            return Json(listaPedido, JsonRequestBehavior.AllowGet);

                        }
                    }


                }

            }
            catch (Exception ex)
            {
                using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    db.PA_InsertarBitacora(ex.Message, DateTime.Now, _nUsuario);
                    db.SaveChanges();
                }
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }


        public JsonResult ConsultarPedidoXId(int codigo)
        {
            try
            {
                using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    var response = db.PA_CONS_ConsultarPedidoxId(codigo).FirstOrDefault();



                    return Json(response, JsonRequestBehavior.AllowGet);

                }
            }
            catch (Exception ex)
            {
                using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    db.PA_InsertarBitacora(ex.Message, DateTime.Now, _nUsuario);
                    db.SaveChanges();
                }
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }


        public JsonResult ConsultarFrasesxPedido(int codigo)
        {
            try
            {
                using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    var response = db.PA_CONS_ConsultarFrasesxPedido_V2(codigo).ToList();



                    return Json(response, JsonRequestBehavior.AllowGet);

                }
            }
            catch (Exception ex)
            {
                using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    db.PA_InsertarBitacora(ex.Message, DateTime.Now, _nUsuario);
                    db.SaveChanges();
                }
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }


        public JsonResult ConsultarFotosxPedido(int codigo)
        {
            List<PedidoModel> pedidos = new List<PedidoModel>();    
            try
            {
                using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    var response = db.PA_CONS_ConsultarFotosxPedido_(codigo).ToList();

                    pedidos= (from e in response select new PedidoModel
                    {
                        strFoto=Convert.ToBase64String(e.paq_imagen)
                    }).ToList();

                    var JsonResult = Json(pedidos, JsonRequestBehavior.AllowGet);
                    JsonResult.MaxJsonLength = int.MaxValue;
                    return JsonResult;
                

                }
            }
            catch (Exception ex)
            {
                using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    db.PA_InsertarBitacora(ex.Message, DateTime.Now, _nUsuario);
                    db.SaveChanges();
                }
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }


        public JsonResult ConsultarRespuestasxPedido(int codigo)
        {
            try
            {
                using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    var response = db.PA_CONS_ConsultarRespuestasxPedido(codigo).ToList();



                    return Json(response, JsonRequestBehavior.AllowGet);

                }
            }
            catch (Exception ex)
            {
                using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    db.PA_InsertarBitacora(ex.Message, DateTime.Now, _nUsuario);
                    db.SaveChanges();
                }
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }


        public JsonResult NuevoDetallePedido(int codigo, int nuevoEstado)
        {
            try
            {
                CorreoModel correo = new CorreoModel();
                using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    db.PA_ActualizarEstadoPedido(codigo, nuevoEstado);
                    db.SaveChanges();

                    var response = db.PA_CONS_ConsultarEstadoxId(nuevoEstado).FirstOrDefault(); 
                    
                    var pedido = db.PA_CONS_ConsultarPedidoxId(codigo).FirstOrDefault();

                    var usuario = db.PA_CONS_ConsultarUsuario(pedido.seg_usuario).FirstOrDefault();
                    if (response.paq_notifica == true)
                    {

                        string cuerpo = "";

                        cuerpo = "Estimado(a) " + usuario.seg_nombre + " " + usuario.seg_apellido1 + " " + usuario.seg_apellido2 + ".  <br>" +
                            "Se le notifica que el paquete con número <b>" + codigo + "</b> se encuentra en el estado " + response.paq_descripcion + ".";

                        correo.EnviarCorreo(usuario.seg_correo, "Notificación Memories By Coco", cuerpo);
                        


                    }

                    if (nuevoEstado == Constantes.EstadoRechazado)
                    {
                       var cantidad= db.PA_CONS_CantidadImgsxPedido(codigo).FirstOrDefault();


                        db.PA_EliminarPuntos_Cantidad(usuario.seg_id, (Constantes.PuntosPorAlbum+(cantidad*Constantes.PuntosPorImagen)));
                    }

                    if (nuevoEstado == Constantes.EstadoFinaliza)
                    {
                        db.PA_Eliminar_ImagenPedido(codigo);
                        db.SaveChanges();
                    }



                }
            }catch(Exception ex)
            {
                using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    db.PA_InsertarBitacora(ex.Message, DateTime.Now, _nUsuario);
                    db.SaveChanges();
                }
            }

            return Json("", JsonRequestBehavior.AllowGet);

        }

        public JsonResult ConsultarCantidadRegistrosPedido(int? rol)
        {
            double NumeroPag = 0;
            int filter = 0;
            List<UsuarioModel> listaUsuario = new List<UsuarioModel>();
            try
            {

                if (Session["Usuario"] != null)
                {

                    listaUsuario = (List<UsuarioModel>)Session["Usuario"];

                    if (rol == 1 && listaUsuario[0].nRol == Constantes.RolAdministrador)
                    {
                        filter = -1;
                    }
                    else
                    {
                        filter = listaUsuario[0].nID;
                    }
                }

                    using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    int? NumeroPaginas = db.PA_CONS_ConsultarRegistrosPedido(filter).FirstOrDefault();

                    if (NumeroPaginas != null)
                    {
                        NumeroPag = (double)NumeroPaginas / Constantes.CantidadPedidoxPagina;


                    }
                    PaginacionModel model = new PaginacionModel();
                    model.nMin = 1;
                    model.nMax = (Math.Ceiling(NumeroPag));

                    return Json(model, JsonRequestBehavior.AllowGet);



                    return Json(NumeroPag, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    db.PA_InsertarBitacora(ex.Message, DateTime.Now, _nUsuario);
                    db.SaveChanges();
                }
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region "Parametros"
        public JsonResult ActualizarParametros(ParametroModel objParametros)
        {
            try
            {
                using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    db.PA_Actualizar_Parametro(objParametros.id, objParametros.descripcion);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    db.PA_InsertarBitacora(ex.Message, DateTime.Now, _nUsuario);
                    db.SaveChanges();
                }
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return Json(new { IsCreated = false, ErrorMessage = "Error inesperado..." + ex });
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }


      

        public JsonResult ConsultarTelefono()
        {

            try
            {
                using(Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                   var response= db.PA_CONS_ConsultarTelMemories(Constantes.PosTelBD).FirstOrDefault();


                    return Json(response, JsonRequestBehavior.AllowGet);
                }
            }catch (Exception ex)
            {
                using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    db.PA_InsertarBitacora(ex.Message, DateTime.Now, _nUsuario);
                    db.SaveChanges();
                }
            }
            



                return Json("", JsonRequestBehavior.AllowGet);
        }


        public JsonResult ConsultarParametros()
        {
            List<ParametroModel> lista = new List<ParametroModel>();
            try
            {
                using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    var response = db.PA_ConsultarParametros(-1).ToList();


                    lista = (from e in response
                             select new ParametroModel
                             {
                                 id = e.paq_parametro,
                                 seccion = e.paq_titulo,
                                 descripcion = e.paq_descripcion
                             }).ToList();

                }
            }
            catch (Exception ex)
            {

                using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    db.PA_InsertarBitacora(ex.Message, DateTime.Now, _nUsuario);
                    db.SaveChanges();
                }

                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return Json(new { IsCreated = false, ErrorMessage = "Error inesperado..." + ex });
            }
            return Json(lista, JsonRequestBehavior.AllowGet);
        }

        #endregion

    }




    

}