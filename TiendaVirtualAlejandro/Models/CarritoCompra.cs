using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TiendaVirtualAlejandro.Models
{
    //[ModelBinder(typeof(CarritoCompraModelBinder))]
    public class CarritoCompra : List<Stock>
    {
        public Cliente Cliente { get; set; }

        public decimal Total { get; set; }
    }

    public class CarritoCompraModelBinder : IModelBinder
    {
        readonly static string Ksc = "carritokey";

        public object BindModel(ControllerContext controllerContext, ModelBindingContext modelBindingContext)
        {
            CarritoCompra cc;

            if (controllerContext.HttpContext.Session[Ksc] == null)
            {
                cc = new CarritoCompra();
                cc.Total = 0;
                controllerContext.HttpContext.Session[Ksc] = cc;
            }
            else
            {
                cc = (CarritoCompra)controllerContext.HttpContext.Session[Ksc];
                cc.Total = 0;
                
                for (int i = 0; i < cc.Count; i++)
                {
                    cc.Total += cc[i].Producto.Precio * cc[i].Cantidad;
                }
            }

            return cc;
        }
    }
}