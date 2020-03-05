﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TiendaVirtualAlejandro.Models
{
    //[ModelBinder(typeof(CarritoCompraModelBinder))]
    public class CarritoCompra : List<Producto>
    {

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
                controllerContext.HttpContext.Session[Ksc] = cc;
            }
            else
                cc = (CarritoCompra)controllerContext.HttpContext.Session[Ksc];

            return cc;
        }
    }
}