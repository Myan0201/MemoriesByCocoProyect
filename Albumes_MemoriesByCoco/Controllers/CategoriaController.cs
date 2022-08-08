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
    public class CategoriaController : Controller
    {
        // GET: Categoria
        public ActionResult Index()
        {
            return View();
        }





        [HttpPost]
        public JsonResult EliminarRegistroCategoria(int id)
        {
            byte[] array = new byte[0];
            try
            {
                using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    db.PA_MantenimientoCategoria((int)Utilitarios.eTipoOperacion.Eliminar, id, "", "", true, array);
                    db.SaveChanges();



                }
            }
            catch (Exception ex)
            {

            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ConsultarRegistroCategoria()
        {
            int? NumPags = null;
            double Num = 0;
            bool bloquear = false;

            PaginacionModel model = new PaginacionModel();  
            try
            {

                using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    NumPags = db.PA_CONS_ConsultarRegistrosCategoria().FirstOrDefault();

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

            }

            return Json("", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [System.Web.Services.WebMethod]
        public JsonResult MostrarCategoria(int pagina)
        {


            List<CategoriaModel> listaCategoria = new List<CategoriaModel>();
            CategoriaModel model = new CategoriaModel();
            try
            {

                using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    var response = db.PA_CONS_ConsultarCategoria_Paginacion(pagina, Constantes.CantidadFrasesxPagina);


                    listaCategoria = (from e in response
                                  select new CategoriaModel
                                  {
                                      idCategoria = e.paq_categoria,
                                      nombre = e.paq_nombre,
                                      descripcion = e.paq_descripcion,
                                      Inactivo = (bool)e.paq_inactivo,

                                  }).ToList();

                }
            }
            catch (Exception ex)
            {

            }


            var JsonResult = Json(listaCategoria, JsonRequestBehavior.AllowGet);
            JsonResult.MaxJsonLength = int.MaxValue;
            return JsonResult;



        }
        [HttpPost]
        public JsonResult AgregarCategoria(CategoriaModel objCategoria)
        {
            List<CategoriaModel> listaCategoria = new List<CategoriaModel>();
            FrasesModel model = new FrasesModel();
            Utilitarios objUtil = new Utilitarios();

            try
            {


                HttpPostedFileBase Filebase = Request.Files[0];

                WebImage image = new WebImage(Filebase.InputStream);

                objCategoria.Imagen = image.GetBytes();
                objCategoria.Imagen = objUtil.CreateThumbnail(objCategoria.Imagen, Constantes.CalidadImagen);

                if (objCategoria.descripcion == "" || objCategoria.descripcion == null || objCategoria.Imagen == null)
                {
                    throw new Exception();
                }

                using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    db.PA_MantenimientoCategoria((int)Utilitarios.eTipoOperacion.Insertar, -1, objCategoria.nombre, objCategoria.descripcion, objCategoria.Inactivo, objCategoria.Imagen);
                    db.SaveChanges();
                }



            }
            catch (Exception ex)
            {

            }
            return Json(listaCategoria, JsonRequestBehavior.AllowGet);
        }


        public JsonResult ConsultarCategoriaLike(string strCategoria)
        {
            List<CategoriaModel> listaFrases = new List<CategoriaModel>();


            try
            {
                using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    var lista = db.PA_CONS_ConsultarCategoriaLike(strCategoria).ToList();

                    listaFrases = (from e in lista
                                   select new CategoriaModel
                                   {
                                       idCategoria = e.paq_categoria,
                                       nombre = e.paq_nombre,
                                       descripcion = e.paq_descripcion,
                                       Inactivo = (bool)e.paq_inactivo
                                   }).ToList();
                }
            }
            catch (Exception ex)
            {

            }


            return Json(listaFrases, JsonRequestBehavior.AllowGet);
        }


        public JsonResult CambiarInactivoCategoria(int id, bool estado)
        {
            try
            {
                using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    db.PA_ActivarCategoria(id, estado);


                }
            }
            catch (Exception ex)
            {

            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ConsultarImgCategoria(int id)
        {
            try
            {
                using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    var sticker = db.PA_CONS_ConsultarImgxIDCategoria(id).FirstOrDefault();

                    string strSticker = Convert.ToBase64String(sticker);
                    var JsonResult = Json(strSticker, JsonRequestBehavior.AllowGet);
                    JsonResult.MaxJsonLength = int.MaxValue;

                    return JsonResult;

                }
            }
            catch (Exception ex)
            {

            }

            return Json("");
        }


        public JsonResult ConsultarCategoria()
        {
            List<CategoriaModel> listaCategoria= new List<CategoriaModel>();


            try
            {
                using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    var lista = db.PA_CONS_ConsultarCategoria(-1).ToList();

                    listaCategoria = (from e in lista
                                   select new CategoriaModel
                                   {
                                       idCategoria = e.paq_categoria,
                                       nombre = e.paq_nombre,
                                       descripcion = e.paq_descripcion
                                      
                                   }).ToList();
                }

            }
            catch(Exception ex)
            {

            }

            return Json(listaCategoria, JsonRequestBehavior.AllowGet);
        }




        public JsonResult CategoriaSeleccionada(int idCategoria)
        {
            Session["IdCategoria"]=idCategoria;

            return Json("", JsonRequestBehavior.AllowGet);
        }
       [HttpPost]
        public JsonResult ConsultarCategoriaImgs()
        {


            List<CategoriaModel> listaCategoria = new List<CategoriaModel>();
            CategoriaModel model = new CategoriaModel();
            try
            {

                using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    var response = db.PA_CONS_ConsultarCategoriaImgs(-1).ToList();


                    listaCategoria = (from e in response
                                      select new CategoriaModel
                                      {
                                          idCategoria = e.paq_categoria,
                                          nombre = e.paq_nombre,
                                          descripcion = e.paq_descripcion,
                                          Inactivo = (bool)e.paq_inactivo,
                                          strImagen= Convert.ToBase64String(e.paq_img)

                                      }).ToList();

                }
            }
            catch (Exception ex)
            {

            }


            var JsonResult = Json(listaCategoria, JsonRequestBehavior.AllowGet);
            JsonResult.MaxJsonLength = int.MaxValue;
            return JsonResult;



        }




    }
}