using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QLChatThai.Models.QLCTModel;

namespace QLChatThai.Controllers
{
    public class XulyController : Controller
    {
        private QLCTModel db = new QLCTModel();

        // GET: /Xuly/
        public ActionResult Index()
        {
            return View(db.Xulies.ToList());
        }

        // GET: /Xuly/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Xuly xuly = db.Xulies.Find(id);
            if (xuly == null)
            {
                return HttpNotFound();
            }
            return View(xuly);
        }

        // GET: /Xuly/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Xuly/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="idxl,phuongphap,mota")] Xuly xuly)
        {
            //if (ModelState.IsValid)
            //{
                db.Xulies.Add(xuly);
                db.SaveChanges();
                return RedirectToAction("Index");
            //}

            //return View(xuly);
        }

        // GET: /Xuly/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Xuly xuly = db.Xulies.Find(id);
            if (xuly == null)
            {
                return HttpNotFound();
            }
            return View(xuly);
        }

        // POST: /Xuly/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="idxl,phuongphap,mota")] Xuly xuly)
        {
            if (ModelState.IsValid)
            {
                db.Entry(xuly).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(xuly);
        }

        // GET: /Xuly/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Xuly xuly = db.Xulies.Find(id);
            if (xuly == null)
            {
                return HttpNotFound();
            }
            return View(xuly);
        }

        // POST: /Xuly/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Xuly xuly = db.Xulies.Find(id);
            db.Xulies.Remove(xuly);
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
    }
}
