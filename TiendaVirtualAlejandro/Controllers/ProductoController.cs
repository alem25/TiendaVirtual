﻿using System;
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
                Producto prod = db.Producto.Find(id);

                if(prod.Cantidad > 0)
                {
                    if (cc.Keys.Any(k => k.Id.Equals(id)))
                    {
                        cc[cc.Keys.Where(p => p.Id.Equals(id)).Single()]++;
                    }
                    else
                        cc.Add(db.Producto.Find(id), 1);
                    prod.Cantidad--;

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
                Producto prod = db.Producto.Find(id);

                if (prod.Cantidad > 0)
                {
                    cc[cc.Keys.Single(k => k.Id.Equals(id))]++;
                    prod.Cantidad--;
                    db.SaveChanges();
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
                Producto prod = db.Producto.Find(id);

                if (cc[cc.Keys.Single(k => k.Id.Equals(id))] == 1)
                    cc.Remove(cc.Keys.Single(k => k.Id.Equals(id)));
                else
                    cc[cc.Keys.Single(k => k.Id.Equals(id))]--;
                prod.Cantidad++;
                db.SaveChanges();
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
                if (cc.Keys.Any(k => k.Id.Equals(id)))
                {
                    cc.Remove(cc.Keys.Where(p => p.Id.Equals(id)).Single());
                    db.Producto.Find(id).Cantidad += cantidad.Value;
                    db.SaveChanges();
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
