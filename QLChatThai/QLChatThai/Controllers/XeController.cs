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
    public class XeController : Controller
    {
        private QLCTModel db = new QLCTModel();

        // GET: /Xe/
        public ActionResult Index()
        {
            //if (Session["Xes"] != null)
            //{
            //    return View(Session["Xes"]);
            //}
            //Session["Xes"] = db.DMXes.ToList();

            return View(db.DMXes.ToList());
        }

        // GET: /Xe/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DMXe dmxe = db.DMXes.Find(id);
            if (dmxe == null)
            {
                return HttpNotFound();
            }
            return View(dmxe);
        }

        // GET: /Xe/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Xe/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="Soxe,mota,status")] DMXe dmxe)
        {
            //if (ModelState.IsValid)
            //{
                db.DMXes.Add(dmxe);
                db.SaveChanges();
                return RedirectToAction("Index");
            //}

            //return View(dmxe);
        }

        // GET: /Xe/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DMXe dmxe = db.DMXes.Find(id);
            if (dmxe == null)
            {
                return HttpNotFound();
            }
            return View(dmxe);
        }

        // POST: /Xe/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit([Bind(Include="Soxe,mota")] DMXe dmxe)
        {
            db.Entry(dmxe).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: /Xe/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DMXe dmxe = db.DMXes.Find(id);
            if (dmxe == null)
            {
                return HttpNotFound();
            }
            return View(dmxe);
        }

        // POST: /Xe/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(string id)
        {
            DMXe dmxe = db.DMXes.Find(id);
            db.DMXes.Remove(dmxe);
            db.SaveChanges();
            return View("Index",db.DMXes.ToList());
            
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
