using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TiendaVirtualAlejandro.Models;

namespace TiendaVirtualAlejandro.Controllers
{
    public class PedidoController : Controller
    {
        private ModeloContainer db = new ModeloContainer();

        [Authorize]
        public ActionResult Index()
        {
            return View(db.Pedido.Where(p=>p.EmailCliente.EmailId.Equals(this.User.Identity.Name)).ToList());
        }

        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pedido pedido = db.Pedido.Find(id);
            if (pedido == null || !pedido.EmailCliente.EmailId.Equals(this.User.Identity.Name))
            {
                return HttpNotFound();
            }
            
            return View(pedido);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id")] Pedido pedido)
        {
            if (ModelState.IsValid)
            {
                db.Pedido.Add(pedido);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(pedido);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pedido pedido = db.Pedido.Find(id);
            if (pedido == null)
            {
                return HttpNotFound();
            }
            return View(pedido);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id")] Pedido pedido)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pedido).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(pedido);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pedido pedido = db.Pedido.Find(id);
            if (pedido == null)
            {
                return HttpNotFound();
            }
            return View(pedido);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Pedido pedido = db.Pedido.Find(id);
            db.Pedido.Remove(pedido);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult MiCarrito(CarritoCompra cc)
        {
            var listProducts = db.ProductosAlmacen;
            foreach(var producto in cc)
            {
                producto.CantidadAlmacen = listProducts.Find(producto.Id).CantidadAlmacen;
            }
            return View(cc);
        }

        public ActionResult Purchase(CarritoCompra cc)
        {
            Pedido pedido = new Pedido()
            {
                EmailCliente = db.Cliente.Find(cc.Cliente.EmailId),
                ProductosVendidos = new List<ProductoVendido>(),
            };
            ProductoVendido productoVendido;
            cc.ForEach(productoAlmacen =>
            {
                productoVendido = new ProductoVendido()
                {
                    Cantidad = productoAlmacen.CantidadCarrito,
                    Nombre = productoAlmacen.Nombre,
                    Categoria = productoAlmacen.Categoria,
                    Descripcion = productoAlmacen.Descripcion,
                    Foto = productoAlmacen.Foto,
                    Precio = productoAlmacen.Precio,
                    Pedido = pedido,
                };
                db.ProductosVendidos.Add(productoVendido);
                pedido.ProductosVendidos.Add(productoVendido);
                db.ProductosAlmacen.Find(productoAlmacen.Id).CantidadAlmacen -= productoAlmacen.CantidadCarrito;
            });
            db.SaveChanges();
            cc.Clear();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
