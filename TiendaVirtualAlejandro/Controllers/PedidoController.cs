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

        // GET: Pedido
        public ActionResult Index()
        {
            return View(db.Pedido);
        }

        // GET: Pedido/Details/5
        public ActionResult Details(int? id)
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

        // GET: Pedido/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Pedido/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
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

        // GET: Pedido/Edit/5
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

        // POST: Pedido/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
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

        // GET: Pedido/Delete/5
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

        // POST: Pedido/Delete/5
        [HttpPost, ActionName("Delete")]
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
                Cliente = cc.Cliente,
                ProductoVendido = new List<ProductoVendido>(),
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
                };
                db.ProductosVendidos.Add(productoVendido);
                pedido.ProductoVendido.Add(productoVendido);
                db.ProductosAlmacen.Find(productoAlmacen.Id).CantidadAlmacen -= productoAlmacen.CantidadCarrito;
            });
            db.SaveChanges();
            cc.Clear();
            return RedirectToAction("Index", pedido);
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
