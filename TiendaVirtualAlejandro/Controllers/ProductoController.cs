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
    public class ProductoController : Controller
    {
        private ModeloContainer db = new ModeloContainer();

        public ActionResult Index(Category? category)
        {
            if(category == null)
                return View(db.ProductosAlmacen.ToList());
            else
                return View(db.ProductosAlmacen.Where(p => p.Categoria.ToString().Equals(category.ToString())).ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductoAlmacen producto = db.ProductosAlmacen.Find(id);
            if (producto == null)
            {
                return HttpNotFound();
            }
            return View(producto);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nombre,Precio,Foto,Descripcion,Categoria,CantidadAlmacen,CantidadCarrito")] ProductoAlmacen producto)
        {            
            if (ModelState.IsValid)
            {
                db.ProductosAlmacen.Add(producto);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(producto);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductoAlmacen producto = db.ProductosAlmacen.Find(id);
            if (producto == null)
            {
                return HttpNotFound();
            }
            return View(producto);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nombre,Precio,Foto,Descripcion,Categoria,CantidadAlmacen,CantidadCarrito")] ProductoAlmacen producto)
        {
            if (ModelState.IsValid)
            {
                db.Entry(producto).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(producto);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductoAlmacen producto = db.ProductosAlmacen.Find(id);
            if (producto == null)
            {
                return HttpNotFound();
            }
            return View(producto);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProductoAlmacen producto = db.ProductosAlmacen.Find(id);

            db.ProductosAlmacen.Remove(producto);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult AddToCart(CarritoCompra cc, int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                ProductoAlmacen productoAlmacen = db.ProductosAlmacen.Find(id);

                if (productoAlmacen.CantidadAlmacen > 0)
                {
                    if (cc.Any(k => k.Id.Equals(id)))
                    {
                        if (productoAlmacen.CantidadAlmacen > cc.Single(k => k.Id.Equals(id)).CantidadCarrito)
                            cc.Single(p => p.Id.Equals(id)).CantidadCarrito++;
                    }
                    else
                    {
                        productoAlmacen.CantidadCarrito = 1;
                        cc.Add(productoAlmacen);
                        db.SaveChanges();
                    }
                }
                else
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return RedirectToAction("MiCarrito", "Pedido");
        }

        public ActionResult AddOneMore(CarritoCompra cc, int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                ProductoAlmacen producto = cc.Single(k => k.Id.Equals(id));

                if (producto.CantidadCarrito < producto.CantidadAlmacen)
                {
                    cc.Single(k => k.Id.Equals(id)).CantidadCarrito++;
                }
            }
            return RedirectToAction("MiCarrito", "Pedido");
        }

        public ActionResult RemoveOne(CarritoCompra cc, int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                ProductoAlmacen producto = cc.Single(k => k.Id.Equals(id));

                if (producto.CantidadCarrito == 1)
                    cc.Remove(producto);
                else
                    cc.Single(k => k.Id.Equals(id)).CantidadCarrito--;
            }
            return RedirectToAction("MiCarrito", "Pedido");
        }


        public ActionResult RemoveFromCart(CarritoCompra cc, int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                ProductoAlmacen producto = cc.Single(k => k.Id.Equals(id));

                if (cc.Contains(producto))
                    cc.Remove(producto);
            }
            return RedirectToAction("MiCarrito", "Pedido");
        }

        [HttpGet]
        [ValidateAntiForgeryToken]
        public ActionResult Search(string busqueda)
        {
            if (String.IsNullOrEmpty(busqueda))
                return View("Index", db.ProductosAlmacen);
            else
                return View("Index", db.ProductosAlmacen.Where(p => p.Nombre.ToUpper().Contains(busqueda.ToUpper().Trim()) || p.Categoria.ToString().ToUpper().Contains(busqueda.ToUpper().Trim())));
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
