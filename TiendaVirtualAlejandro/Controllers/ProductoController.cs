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

        // GET: Producto de una categoría
        public ActionResult Index(Category? category)
        {
            if(category == null)
                return View(db.Producto.ToList());
            else
                return View(db.Producto.Where(p => p.Category.ToString().Equals(category.ToString())).ToList());
        }

        // GET: Producto/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Producto producto = db.Producto.Find(id);
            if (producto == null)
            {
                return HttpNotFound();
            }
            return View(producto);
        }

        // GET: Producto/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Producto/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nombre,Precio,Foto,Descripción,Category,Cantidad")] Producto producto)
        {            
            if (ModelState.IsValid)
            {
                db.Producto.Add(producto);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(producto);
        }

        // GET: Producto/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Producto producto = db.Producto.Find(id);
            if (producto == null)
            {
                return HttpNotFound();
            }
            return View(producto);
        }

        // POST: Producto/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nombre,Precio,Foto,Descripción,Category,Cantidad")] Producto producto)
        {
            if (ModelState.IsValid)
            {
                db.Entry(producto).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(producto);
        }

        // GET: Producto/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Producto producto = db.Producto.Find(id);
            if (producto == null)
            {
                return HttpNotFound();
            }
            return View(producto);
        }

        // POST: Producto/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Producto producto = db.Producto.Find(id);

            db.Producto.Remove(producto);
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
                int cantidadInventario = db.Producto.Find(id).Cantidad;

                if (cantidadInventario > 0)
                {
                    if (cc.Cliente == null && this.User.Identity.IsAuthenticated)
                        cc.Cliente = db.Cliente.SingleOrDefault(c => c.Email.Equals(this.User.Identity.Name));

                    if (cc.Keys.Any(k => k.Id.Equals(id)))
                    {
                        int cantidadCarrito = cc[cc.Keys.Single(k => k.Id.Equals(id))];

                        if (cantidadInventario > cantidadCarrito)
                            cc[cc.Keys.Single(p => p.Id.Equals(id))]++;
                    }
                    else
                        cc.Add(db.Producto.Find(id), 1);

                    db.SaveChanges();
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
                int cantidadInventario = cc.Keys.Single(k => k.Id.Equals(id)).Cantidad;
                int cantidadCarrito = cc[cc.Keys.Single(k => k.Id.Equals(id))];

                if (cantidadCarrito < cantidadInventario)
                {
                    cc[cc.Keys.Single(k => k.Id.Equals(id))]++;
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
                Producto productoCarrito = cc.Keys.Single(k => k.Id.Equals(id));
                int cantidadCarrito = cc[cc.Keys.Single(k => k.Id.Equals(id))];

                if (cantidadCarrito == 1)
                    cc.Remove(productoCarrito);
                else
                    cc[cc.Keys.Single(k => k.Id.Equals(id))]--;
            }
            return RedirectToAction("MiCarrito", "Pedido");
        }


        public ActionResult RemoveFromCart(CarritoCompra cc, int? id, int? cantidad)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                Producto productoCarrito = cc.Keys.Single(k => k.Id.Equals(id));

                if (cc.ContainsKey(productoCarrito))
                {
                    cc.Remove(productoCarrito);
                }
            }
            return RedirectToAction("MiCarrito", "Pedido");
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
