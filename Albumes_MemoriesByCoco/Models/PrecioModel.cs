using Albumes_MemoriesByCoco.LogicaNegocios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Albumes_MemoriesByCoco.Models
{
    public class PrecioModel
    {
        public int IdPrecios { get; set; }
        public string DescripcionPrecios { get; set; }

        public decimal Valor { get;set; }

        public decimal ValorFinal { get;set; }


        public decimal? ObtenerValorxFoto(decimal? valor, int cantidadFotos)
        {
            int CantidadActual = cantidadFotos;
            decimal? response = 0;
            try
            {
               

                if (cantidadFotos > 15 && valor>0)
                {
                    CantidadActual = cantidadFotos - Constantes.CantidadImagenesSinCobro;
                    response = CantidadActual * (decimal)valor;



                }
            }
            catch(Exception ex)
            {

            }
           

            return response;


        }

        public PrecioModel CalcularDescuento(string Descripcion, decimal valor, decimal total)
        {
            PrecioModel objPrecio = new PrecioModel();

            try
            {
                objPrecio.DescripcionPrecios = Descripcion;
                objPrecio.Valor = -(total * (valor / 100));

            }catch(Exception ex)
            {

            }

            return objPrecio;
        }
        




        }



    }
