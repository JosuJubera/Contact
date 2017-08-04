using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Contact.Models;
using Microsoft.AspNet.Identity;

namespace Contact.Controllers
{
    public class ContactoesController : Controller
    {
        private ContactContext db = new ContactContext();

        // GET: Contactoes
        [Authorize]
        public ActionResult Index()
        {
            var UserId = GetCurrentUserId();
            return View(db.Contactoes.Where(x => x.UserId == UserId).ToList());
        }

        // GET: Contactoes/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contacto contacto = db.Contactoes.Find(id);
            if (contacto == null || !EnsureIsUserContact(contacto))
            {
                return HttpNotFound();
            }
            return View(contacto);
        }

        // GET: Contactoes/Create
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.UserId = GetCurrentUserId();
            return View();
        }

        // POST: Contactoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include = "Id,UserId,FirstName,LastName,Email,Phone,Birthday,StreetAddress,City,State,Zip")] Contacto contacto)
        {
            if (ModelState.IsValid)
            {
                db.Contactoes.Add(contacto);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserId = GetCurrentUserId();
            return View(contacto);
        }

        // GET: Contactoes/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contacto contacto = db.Contactoes.Find(id);
            if (contacto == null || !EnsureIsUserContact(contacto))
            {
                return HttpNotFound();
            }

            ViewBag.UserId = GetCurrentUserId();
            return View(contacto);
        }

        // POST: Contactoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([Bind(Include = "Id,UserId,FirstName,LastName,Email,Phone,Birthday,StreetAddress,City,State,Zip")] Contacto contacto)
        {
            if (ModelState.IsValid)
            {
                db.Entry(contacto).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserId = GetCurrentUserId();
            return View(contacto);
        }

        // GET: Contactoes/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contacto contacto = db.Contactoes.Find(id);
            if (contacto == null || !EnsureIsUserContact(contacto))
            {
                return HttpNotFound();
            }
            return View(contacto);
        }

        // POST: Contactoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            Contacto contacto = db.Contactoes.Find(id);
            if (!EnsureIsUserContact(contacto))
            {
                return HttpNotFound();
            }
            db.Contactoes.Remove(contacto);
            db.SaveChanges();
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

        public Guid GetCurrentUserId()
        {
            return new Guid(User.Identity.GetUserId());
        }

        public bool EnsureIsUserContact(Contacto contact)
        {
            return contact.UserId == GetCurrentUserId();
        }
    }
}
