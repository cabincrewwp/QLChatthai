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
    
    public class CTNhapController : Controller
    {
        private QLCTModel db = new QLCTModel();
        
        // GET: /CTNhap/
        public ActionResult Index()
        {
            var ctnhaps = db.CTNHaps.Include(c => c.Xuly);
            return View(ctnhaps.ToList());
        }

        // GET: /CTNhap/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CTNHap ctnhap = db.CTNHaps.Find(id);
            if (ctnhap == null)
            {
                return HttpNotFound();
            }
            return View(ctnhap);
        }

        // GET: /CTNhap/Create
        public ActionResult Create(int id)
        {
            var list=new String[] {"Kg","Tấn","Phuy"};
            ViewBag.idct = new SelectList(db.DM_CT, "idct", "Ma_CT");
            ViewBag.idxl = new SelectList(db.Xulies, "idxl", "phuongphap");
            ViewBag.sophieu = id;
            ViewBag.dvt = new SelectList(list, "Kg");
            //ViewBag.loai = loai;
            
            return View();
        }

        // POST: /CTNhap/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="idctnhap,idct,Tenct,Sophieu,dvt,slthuc,slbaocao,Dongianhap,trangthai,idxl,loai")] CTNHap ctnhap)
        {
            
            //if (ModelState.IsValid)
            //{
                ctnhap.loai = false;
                db.CTNHaps.Add(ctnhap);
                db.SaveChanges();
                return RedirectToAction("Details/" + ctnhap.Sophieu.ToString(), "Nhap");
            //}

            //ViewBag.idct = new SelectList(db.DM_CT, "idct", "Ten_CT", ctnhap.idct);
            //ViewBag.idxl = new SelectList(db.Xulies, "idxl", "phuongphap");
            //ViewBag.sophieu = ctnhap.idctnhap;
            //return RedirectToAction("Details/" + ctnhap.Sophieu.ToString(), "Nhap");
            //return Content(kq);
        }

        public ActionResult Creatett(int id)
        {
            var list = new String[] { "Kg", "Tấn", "Phuy" };
            ViewBag.idct = new SelectList(db.DM_CT, "idct", "Ma_CT");
            ViewBag.idxl = new SelectList(db.Xulies, "idxl", "phuongphap");
            ViewBag.sophieu = id;
            ViewBag.dvt = new SelectList(list, "Kg");
            //ViewBag.loai = loai;

            return View();
        }

        // POST: /CTNhap/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Creatett([Bind(Include = "idctnhap,idct,Tenct,Sophieu,dvt,slthuc,slbaocao,Dongianhap,loai")] CTNHap ctnhap)
        {

            //if (ModelState.IsValid)
            //{
            ctnhap.loai = true;
            db.CTNHaps.Add(ctnhap);
            db.SaveChanges();
            return RedirectToAction("Details/" + ctnhap.Sophieu.ToString(), "Nhap");
            //}

            //ViewBag.idct = new SelectList(db.DM_CT, "idct", "Ten_CT", ctnhap.idct);
            //ViewBag.idxl = new SelectList(db.Xulies, "idxl", "phuongphap");
            //ViewBag.sophieu = ctnhap.idctnhap;
            //return RedirectToAction("Details/" + ctnhap.Sophieu.ToString(), "Nhap");
            //return Content(kq);
        }

        // GET: /CTNhap/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CTNHap ctnhap = db.CTNHaps.Find(id);
            if (ctnhap == null)
            {
                return HttpNotFound();
            }
            var list = new String[] { "Kg", "Tấn", "Phuy" };
            ViewBag.idct = new SelectList(db.DM_CT, "idct", "Ma_CT",ctnhap.idct);
            ViewBag.idxl = new SelectList(db.Xulies, "idxl", "phuongphap",ctnhap.idxl);
            ViewBag.dvt = new SelectList(list, "Kg");
            ViewBag.sophieu = ctnhap.Sophieu;
            return View(ctnhap);
        }

        // POST: /CTNhap/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="idctnhap,Sophieu,Tenct,idct,dvt,slthuc,slbaocao,Dongianhap,trangthai,idxl")] CTNHap ctnhap)
        {
            //if (ModelState.IsValid)
            //{
                db.Entry(ctnhap).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details/" + ctnhap.Sophieu.ToString(), "Nhap");
            //}
            //ViewBag.Ma_CT = new SelectList(db.DM_CT, "Ma_CT", "Ma_CT", ctnhap.idct);
            //ViewBag.idxl = new SelectList(db.Xulies, "idxl", "phuongphap",ctnhap.idctnhap);
            //return RedirectToAction("Details/" + ctnhap.Sophieu.ToString(), "Nhap");
        }

        // GET: /CTNhap/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CTNHap ctnhap = db.CTNHaps.Find(id);
            if (ctnhap == null)
            {
                return HttpNotFound();
            }
            ViewBag.sophieu = ctnhap.Sophieu;
            return View(ctnhap);
        }

        // POST: /CTNhap/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CTNHap ctnhap = db.CTNHaps.Find(id);
            db.CTNHaps.Remove(ctnhap);
            db.SaveChanges();
            return RedirectToAction("Details/" + ctnhap.Sophieu.ToString(), "Nhap");
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
