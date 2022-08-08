using Albumes_MemoriesByCoco.LogicaNegocios;
using Albumes_MemoriesByCoco.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace Albumes_MemoriesByCoco.Controllers
{
    public class AlbumController : Controller
    {
        // GET: Album
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DiseñarAlbum()
        {
            return View();
        }


        public JsonResult ObtenerIdAlbum()
        {
            try
            {
                using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    if (Session["IdAlbumCrear"] == null)
                    {
                        int? response = db.PA_MantenimientoAlbumPredefinido((int)Utilitarios.eTipoOperacion.Insertar, -1, "", null).FirstOrDefault();


                        Session["IdAlbumCrear"] = response;
                    }
                  
                }

            }catch(Exception ex)
            {

            }
            return Json("", JsonRequestBehavior.AllowGet);
        }

        public JsonResult AgregarAlbum(string Nombre, int Categoria)
        {
            try
            {
                using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                   
                    db.PA_MantenimientoAlbumPredefinido((int)Utilitarios.eTipoOperacion.Actualizar, (int)Session["IdAlbumCrear"], Nombre, Categoria).FirstOrDefault();
                    db.SaveChanges();
                    Session["IdAlbumCrear"] = null;
                    

                }

            }
            catch (Exception ex)
            {

            }
            return Json("", JsonRequestBehavior.AllowGet);
        }


        public JsonResult InsertarImgAlbum(string Imagen, bool? esPortada)
        {
            int? idImg = null;
            try
            {
                byte[] ImagenPosted = new Byte[64];

                HttpPostedFileBase Filebase = Request.Files[0];

                WebImage image = new WebImage(Filebase.InputStream);

                ImagenPosted = image.GetBytes();
               
 
                using(Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    if (Session["IdAlbumCrear"] != null)
                    {
                        idImg = db.PA_MantenimientoImgAlbumPredefinido((int)Utilitarios.eTipoOperacion.Insertar, (int)Session["IdAlbumCrear"], -1, ImagenPosted, esPortada).FirstOrDefault();
                    }
                    
                }
            }catch(Exception ex)
            {

            }

            return Json(idImg, JsonRequestBehavior.AllowGet);
        }

        public JsonResult EliminarImagenAlbum(int id)
        {
        
            try
            {
              


                using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    if (Session["IdAlbumCrear"] != null)
                    {
                        db.PA_MantenimientoImgAlbumPredefinido((int)Utilitarios.eTipoOperacion.Eliminar, -1, id, null, null);
                            db.SaveChanges();
                    }

                }
            }
            catch (Exception ex)
            {

            }

            return Json("", JsonRequestBehavior.AllowGet);
        }

        public JsonResult MostrarAlbum(int pagina)
        {
            List<AlbumModel> listaAlbum = new List<AlbumModel> ();
            try
            {
                using(Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                 var response=   db.PA_CONS_ConsultarAlbum_Paginacion_(pagina, Constantes.CantidadFrasesxPagina).ToList();


                    listaAlbum= ( from e in response select new AlbumModel
                    {
                        idAlbum=e.paq_album,
                        nombre= e.paq_nombre,
                        Categoria= (int)e.paq_categoria,
                        nombreCategoria= e.paq_nombre_categoria

                       
                    }).ToList();


                }
            }catch (Exception ex)
            {

            }
            return Json(listaAlbum, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ConsultarAlbumLike(string strAlbum)
        {
            List<AlbumModel> listaAlbum = new List<AlbumModel>();
            try
            {
                using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    var response = db.PA_CONS_ConsultarAlbumLike(strAlbum).ToList();


                    listaAlbum = (from e in response
                                  select new AlbumModel
                                  {
                                      idAlbum = e.paq_album,
                                      nombre = e.paq_nombre,
                                      Categoria = (int)e.paq_categoria,
                                      nombreCategoria = e.paq_nombre_categoria


                                  }).ToList();


                }
            }
            catch (Exception ex)
            {

            }
            return Json(listaAlbum, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ConsultarRegistroAlbum()
        {
            double NumeroPag = 0;
            try
            {
                PaginacionModel model = new PaginacionModel(); 
                using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    int? NumeroPaginas = db.PA_CONS_ConsultarRegistrosAlbum().FirstOrDefault();

                    if (NumeroPaginas != null)
                    {
                        NumeroPag = (double)NumeroPaginas / Constantes.CantidadFrasexPaginaSticker;


                    }
                    model.nMin = 1;
                    model.nMax = (Math.Ceiling(NumeroPag));

                    return Json(model, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {

            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }

        public  JsonResult EliminarRegistroAlbum(int id)
        {
            try
            {
                using(Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    db.PA_MantenimientoImgAlbumPredefinido((int)Utilitarios.eTipoOperacion.EliminarReferencias, id, id, null, false);


                    db.PA_MantenimientoAlbumPredefinido((int)Utilitarios.eTipoOperacion.Eliminar,id,"",0);
                }       
            }catch(Exception ex)
            {

            }
            return Json("", JsonRequestBehavior.AllowGet);
        }

        public JsonResult ConsultarImagenesXAlbum(int id)
        {
            List<AlbumModel> listaAlbum = new List<AlbumModel>();
            try
            {
                using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    var response = db.PA_CONS_ConsultarImagenesAlbumPredefinido_V2(id).ToList();
                    
                  

                    listaAlbum = (from e in response
                                  select new AlbumModel
                                  {
                                      
                                     strImagen=Convert.ToBase64String(e.paq_img)
   

                                  }).ToList();

                    var JsonResult = Json(listaAlbum, JsonRequestBehavior.AllowGet);
                    JsonResult.MaxJsonLength = int.MaxValue;

                    return JsonResult;
                }
            }
            catch (Exception ex)
            {

            }
            return Json("", JsonRequestBehavior.AllowGet);
        }

        public JsonResult EliminarSesionAlbum()
        {
            try
            {
                if (Session["IdAlbumCrear"] != null)
                {
                    using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                    {
                        db.PA_MantenimientoImgAlbumPredefinido((int)Utilitarios.eTipoOperacion.EliminarReferencias, (int)Session["IdAlbumCrear"], (int)Session["IdAlbumCrear"], null, false);


                        db.PA_MantenimientoAlbumPredefinido((int)Utilitarios.eTipoOperacion.Eliminar, (int)Session["IdAlbumCrear"], "", 0);
                    }

                    Session["IdAlbumCrear"] = null;
                }
                    
            }
            catch(Exception ex)
            {

            }

            return Json("", JsonRequestBehavior.AllowGet);
        }

        public JsonResult ConsultarAlbumxCategoria(int idCategoria)
        {
            List<AlbumModel> listaAlbum = new List<AlbumModel>();
            try
            {

                using(Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                  var response=  db.PA_CONS_ConsultarAlbumxCategoria(idCategoria).ToList();


                    listaAlbum=(from e in response select new AlbumModel
                    {
                        idAlbum=e.paq_album,
                       nombre=e.paq_nombre,
                       strImagen=Convert.ToBase64String(e.paq_img)

                    }).ToList();



                }
                var JsonResult = Json(listaAlbum, JsonRequestBehavior.AllowGet);
                JsonResult.MaxJsonLength = int.MaxValue;

                return JsonResult;
            }
            catch(Exception ex)
            {

            }

            return Json("", JsonRequestBehavior.AllowGet);
        }


        public JsonResult SeleccionAlbumPredefinido(int idAlbum)
        {
         PedidoModel pedido = new PedidoModel();    
            try
            {
                pedido.Personalizado = idAlbum;
                pedido.nAlbumPredefinido = idAlbum;




                Session["PedidoUsuario"] = pedido;


            }
            catch (Exception ex)
            {

            }

            return Json("", JsonRequestBehavior.AllowGet);
        }


    }
}