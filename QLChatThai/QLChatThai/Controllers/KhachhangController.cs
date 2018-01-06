using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QLChatThai.Models.QLCTModel;
using Microsoft.Office;
using Excel = Microsoft.Office.Interop.Excel;
namespace QLChatThai.Controllers
{
    public class KhachhangController : Controller
    {
        private QLCTModel db = new QLCTModel();

        // GET: /Khachhang/
        public ActionResult Index()
        {
            //ViewBag.Ma_tinh = new SelectList(db.Tinhthanhs, "Ma_tinh", "Ten");
            
            var khdisp=from kh in db.Khachhangs
                        join tt in db.Tinhthanhs on kh.Ma_tinh equals tt.Ma_tinh
                        orderby kh.Code_KH
                       select new KhachhangDisp
                       {
                            Code_KH=kh.Code_KH,
                            Ten_KH=kh.Ten_KH,
                            Diachi_VP=kh.Diachi_VP,
                            Diachi_CS=kh.Diachi_CS,
                            Dienthoaivp=kh.Dienthoaivp,
                            Dienthoaics=kh.Dienthoaics,
                            MS_QLCTNH=kh.MS_QLCTNH,
                            Ten_tinh=tt.Ten
                            };
            //var khachhangs = db.Khachhangs.ToList();
            return View(khdisp.ToList());
        }

        // GET: /Khachhang/Details/5
  

        // GET: /Khachhang/Create
        public ActionResult Create()
        {
            ViewBag.Ma_tinh = new SelectList(db.Tinhthanhs, "Ma_tinh", "Ten");
            return View();
        }

        // POST: /Khachhang/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="Code_KH,Ten_KH,Diachi_VP,Diachi_CS,Dienthoaivp,Dienthoaics,MS_QLCTNH,Ma_tinh")] Khachhang khachhang)
        {
            //if (ModelState.IsValid)
            //{
                khachhang.Code_KH=khachhang.Code_KH.ToUpper();
                db.Khachhangs.Add(khachhang);
                db.SaveChanges();
                return RedirectToAction("Index");
            //}

            //ViewBag.Ma_tinh = new SelectList(db.Tinhthanhs, "Ma_tinh", "Ten", khachhang.Ma_tinh);
            //return RedirectToAction("Index");
        }

        // GET: /Khachhang/Edit/5
        public ActionResult Edit(string id)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            Khachhang khachhang = db.Khachhangs.Find(id);
            if (khachhang == null)
            {
                return HttpNotFound();
            }
            ViewBag.Ma_tinh = new SelectList(db.Tinhthanhs, "Ma_tinh", "Ten", khachhang.Ma_tinh);
            return View(khachhang);
        }

        // POST: /Khachhang/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Code_KH,Ten_KH,Diachi_VP,Diachi_CS,Dienthoaivp,Dienthoaics,MS_QLCTNH,Ma_tinh")] Khachhang khachhang)
        {
            //if (ModelState.IsValid)
            //{
                try { 
                        db.Entry(khachhang).State = EntityState.Modified;
                        db.SaveChanges();
                        ModelState.Clear(); // Xóa trạng thái form
                        ModelState.AddModelError("", "Cập nhật thành công!");                        
                    }
                catch
                {
                    ModelState.AddModelError("", "Cập nhật thất bại!");
                }
                return RedirectToAction("Index"); 
            //}
            //ViewBag.Ma_tinh = new SelectList(db.Tinhthanhs, "Ma_tinh", "Ten", khachhang.Ma_tinh);
            //return View(khachhang);
        }

        // GET: /Khachhang/Delete/5
        public ActionResult Delete(string id)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            Khachhang khachhang = db.Khachhangs.Find(id);
            if (khachhang == null)
            {
                return HttpNotFound();
            }
            return View(khachhang);
        }

        // POST: /Khachhang/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Khachhang khachhang = db.Khachhangs.Find(id);
            db.Khachhangs.Remove(khachhang);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public JsonResult GetCodeKH(string key)
        {
            var CodeKH = (from kh in db.Khachhangs
                          where kh.Code_KH.StartsWith(key)
                          select new { kh.Code_KH, kh.Ten_KH });
            return Json(CodeKH, JsonRequestBehavior.AllowGet);

        }
        public void UpdateKH()
        {
            Excel.Application oexcel = new Excel.Application();
            Excel.Workbook owb;
            Excel.Worksheet ost;
            owb = oexcel.Workbooks.Open(@"D:\Quan Ly Chat Thai\QLChatThai\Khachhang.xls");
            oexcel.Visible = true;
            ost = owb.Sheets[2];
            int _id = 0;
            for (int i = 2; i <= 133; i++)
            {
                _id = Convert.ToInt32(ost.Cells[i, 1].Value);
                if (_id > 0)
                {
                    var kh = db.Khachhangs.Find(_id);
                    if (kh != null)
                    {
                        kh.Code_KH = Convert.ToString(ost.Cells[i, 2].Value);
                        db.SaveChanges();
                    }
                    else
                    {
                        ost.Cells[i, 15] = "Có vấn đề, không tìm thấy";
                    }
                }
                else
                {
                    //Chưa có thì thêm mới vào
                    Khachhang kh = new Khachhang();
                    kh.Code_KH = Convert.ToString(ost.Cells[i, 2].Value);
                    kh.Ten_KH = Convert.ToString(ost.Cells[i, 3].Value);
                    kh.Diachi_VP = Convert.ToString(ost.Cells[i, 4].Value);
                    kh.Diachi_CS = Convert.ToString(ost.Cells[i, 5].Value);
                    kh.Dienthoaivp = Convert.ToString(ost.Cells[i, 6].Value);
                    kh.Dienthoaics = Convert.ToString(ost.Cells[i, 7].Value);
                    kh.MS_QLCTNH = Convert.ToString(ost.Cells[i, 8].Value);
                    kh.Ma_tinh = Convert.ToInt32(ost.Cells[i, 9].Value);
                    db.Khachhangs.Add(kh);
                    db.SaveChanges();
                }
            }

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
