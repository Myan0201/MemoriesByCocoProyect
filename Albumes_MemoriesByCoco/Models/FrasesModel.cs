using Albumes_MemoriesByCoco.LogicaNegocios;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Albumes_MemoriesByCoco.Models
{
    public class FrasesModel
    {
        public int IdFrase { get; set; }

        [Required]
        [MaxLength(100)]
        [DisplayName("Descripción")]
        public string DescripcionFrase { get; set; }


        [Required]
        [Display(Name ="Sticker")]

        public string strSticker { get; set; }

        [DataType(DataType.Upload)]
        public byte[] Sticker { get; set; }
        public bool Inactivo { get; set; }

        public bool check { get; set; }


        public bool FraseCheckeada(int nFrase, List<FrasesModel> objLista)
        {
            bool existe = false;
            try
            {
                if (objLista != null)
                {
                    for (int i = 0; i < objLista.Count; i++)
                    {
                        if (objLista[i].IdFrase == nFrase)
                        {
                            existe = true;
                            break;
                        }
                    }
                }
               
            }catch(Exception ex)
            {

            }

            return existe;
        }
        public List<FrasesModel> SeleccionarFrases(int pag)
        {
            List <FrasesModel> listaFrases = new List<FrasesModel>(); 
            try
            {
                using (Data.MemoriesByCocoEntities db = new Data.MemoriesByCocoEntities())
                {
                    var response = db.PA_CONS_ConsultarFrases_Paginacion(pag, Constantes.CantidadFrasesxPagina);


                    listaFrases = (from e in response
                                   select new FrasesModel
                                   {
                                       IdFrase = e.paq_frase,
                                       DescripcionFrase = e.paq_descripcion,
                                       Inactivo = (bool)e.paq_inactivo
                                   }).ToList();

                }
            }catch(Exception ex)
            {

            }
            return listaFrases;
        }


    }
}