using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QLChatThai.Models.QLCTModel;
using Novacode;
using Microsoft.Office;
using Excel = Microsoft.Office.Interop.Excel;
namespace QLChatThai.Controllers
{
    public class DM_CTController : Controller
    {
        private QLCTModel db = new QLCTModel();
        
        // GET: /DM_CT/
        public ActionResult Index(int page = 1, string sort = "Ma_CT", string sortdir = "asc", string search = "")
        {

            int pageSize = 20;
            int totalRecord = 0;
            if (page < 1) page = 1;
            int skip = (page * pageSize) - pageSize;
            var data = GetDMCT(search, sort, sortdir, skip, pageSize, out totalRecord);
            ViewBag.TotalRows = totalRecord;
            ViewBag.search = search;
            return View(data);
            //var dm_ct = db.DM_CT.Include(d => d.DMCap2);
            //return View(db.DM_CT.ToList());
        }

        public List<DM_CT> GetDMCT(string search, string sort, string sortdir, int skip, int pageSize, out int totalRecord)
        {
            
                var v = (from a in db.DM_CT
                         where
                                 a.Ma_CT.Contains(search) ||
                                 a.Ten_CT.Contains(search) 
                         select a
                                );
                totalRecord = v.Count();
                v = v.OrderBy(sort + " " + sortdir);
                if (pageSize > 0)
                {
                    v = v.Skip(skip).Take(pageSize);
                }
                return v.ToList();
            
        }
        // GET: /DM_CT/Details/5
        public ActionResult List()
        {
            //var dm_ct = db.DM_CT.Include(d => d.DMCap2);
            return View(db.DM_CT.ToList());
        }


        // GET: /DM_CT/Create
        public ActionResult Create()
        {
            //ViewBag.MaCap2 = new SelectList(db.DMCap2, "Macap2", "Ten");
            return View();
        }

        // POST: /DM_CT/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="Ma_CT,Ten_CT,Ma_EC,Ma_BaselA,Ma_BaselY,TC_Nguyhai,Trangthai_tontai,Nguong_nguyhai,status,tuxuly")] DM_CT dm_ct)
        {
            //if (ModelState.IsValid)
            //{
                db.DM_CT.Add(dm_ct);
                db.SaveChanges();
                return RedirectToAction("Index");
            //}

            //ViewBag.MaCap2 = new SelectList(db.DMCap2, "Macap2", "Ten", dm_ct.MaCap2);
            //return View(dm_ct);
        }

        // GET: /DM_CT/Edit/5
        public ActionResult Edit(int id)
        {
            
            DM_CT dm_ct = db.DM_CT.Find(id);
            if (dm_ct == null)
            {
                return HttpNotFound();
            }
            //ViewBag.MaCap2 = new SelectList(db.DMCap2, "Macap2", "Ten", dm_ct.MaCap2);
            return View(dm_ct);
        }

        // POST: /DM_CT/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="idct,Ma_CT,Ten_CT,Ma_EC,Ma_BaselA,Ma_BaselY,TC_Nguyhai,Trangthai_tontai,Nguong_nguyhai,status,tuxuly")] DM_CT dm_ct)
        {
            //if (ModelState.IsValid)
            //{
                db.Entry(dm_ct).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            //}
            ////ViewBag.MaCap2 = new SelectList(db.DMCap2, "Macap2", "Ten", dm_ct.MaCap2);
            //return View(dm_ct);
        }

        // GET: /DM_CT/Delete/5
        public ActionResult Delete(int id)
        {
            
            DM_CT dm_ct = db.DM_CT.Find(id);
            if (dm_ct == null)
            {
                return HttpNotFound();
            }
            return View(dm_ct);
        }

        // POST: /DM_CT/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DM_CT dm_ct = db.DM_CT.Find(id);
            db.DM_CT.Remove(dm_ct);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public void load_DMCT()
        {
               
            string filename = @"D:\Quan Ly Chat Thai\Danh muc chat thai.docx";
            string mact, tenct, maec, mabasela, mabasely,tinhchat,trangthai,nguong,cap2;
            var template = DocX.Load(filename);
            Table table = template.Tables[0];
            for (int i = 1; i < table.RowCount; i++)
            {
                if (table.Rows[i].Cells[0].Paragraphs.First().Text.Trim().Length > 10)
                {
                    mact = table.Rows[i].Cells[0].Paragraphs.First().Text.Trim().Substring(0, 10);
                }
                else
                {
                    mact = table.Rows[i].Cells[0].Paragraphs.First().Text.Trim();
                }

                
                if (mact.Length>=8){
                    if (table.Rows[i].Cells[1].Paragraphs.First().Text.Trim().Length > 150)
                    {
                        tenct = table.Rows[i].Cells[1].Paragraphs.First().Text.Trim().Substring(0, 150);
                    }
                    else
                    {
                        tenct = table.Rows[i].Cells[1].Paragraphs.First().Text.Trim();
                    }
                    if (table.Rows[i].Cells[2].Paragraphs.First().Text.Trim().Length > 10)
                    {

                        maec = table.Rows[i].Cells[2].Paragraphs.First().Text.Trim().Substring(0, 10);
                    }
                    else
                    {
                        maec = table.Rows[i].Cells[2].Paragraphs.First().Text.Trim();
                    }
                    mabasela = table.Rows[i].Cells[3].Paragraphs.First().Text.Trim();
                    if (mabasela.Length > 20)
                    {
                        mabasela = mabasela.Substring(0, 20);
                    }
                    if (table.Rows[i].Cells[4].Paragraphs.First().Text.Trim().Length > 20)
                    {
                        mabasely = table.Rows[i].Cells[4].Paragraphs.First().Text.Trim().Substring(0, 20);
                    }
                    else 
                    {
                        mabasely = table.Rows[i].Cells[4].Paragraphs.First().Text.Trim();
                    }
                    if (table.Rows[i].Cells[5].Paragraphs.First().Text.Trim().Length > 50)
                    {
                        tinhchat = table.Rows[i].Cells[5].Paragraphs.First().Text.Trim().Substring(0, 50);
                    }
                    else
                    {
                        tinhchat = table.Rows[i].Cells[5].Paragraphs.First().Text.Trim();
                    }
                    if (table.Rows[i].Cells[6].Paragraphs.First().Text.Trim().Length > 20)
                    {
                        trangthai = table.Rows[i].Cells[6].Paragraphs.First().Text.Trim().Substring(0, 20);
                    }
                    else {
                        trangthai = table.Rows[i].Paragraphs.First().Text.Trim().Trim();
                    }
                    if (table.Rows[i].Cells[7].Paragraphs.First().Text.Trim().Length > 10)
                    {
                        nguong = table.Rows[i].Cells[7].Paragraphs.First().Text.Trim().Substring(0, 10);
                    }
                    else{
                        nguong = table.Rows[i].Cells[7].Paragraphs.First().Text.Trim();
                    }
                    
                    cap2=mact.Substring(0,5);

                    DM_CT ct =new DM_CT();
                    ct.Ma_CT=mact;
                    ct.Ten_CT=tenct;
                    ct.Ma_EC=maec;
                    ct.Ma_BaselA=mabasela;
                    ct.Ma_BaselY=mabasely;
                    ct.TC_Nguyhai=tinhchat;
                    ct.Trangthai_tontai=trangthai;
                    ct.Nguong_nguyhai=nguong;
                    ct.MaCap2=cap2;
                    db.DM_CT.Add(ct);
                    db.SaveChanges();

                }
            }
            template.Dispose();
        }
        public void Update_DMCT()
        {
            string filename = @"D:\Quan Ly Chat Thai\20May2017\DS MÃ CTNH TRONG GPXL.xlsx";
            string mact, tenct, maec, mabasela, mabasely, tinhchat, trangthai, nguong, tuxu;
            Excel.Application oexcel = new Excel.Application();
            Excel.Workbook owb;
            Excel.Worksheet ost;
            owb = oexcel.Workbooks.Open(filename);
            oexcel.Visible = true;
            ost = owb.Sheets[2];
            for (int i = 3; i <= 434; i++)
            {
                mact = ost.Cells[i, 1].Value;
                tenct = ost.Cells[i, 2].Value;
                maec = ost.Cells[i, 3].Value;
                mabasela = ost.Cells[i, 4].Value;
                mabasely = ost.Cells[i, 5].Value;
                tinhchat = ost.Cells[i, 6].Value;
                trangthai = ost.Cells[i, 7].Value;
                nguong = ost.Cells[i, 8].Value;
                tuxu = ost.Cells[i, 9].Value;
                var ct1 =db.DM_CT.FirstOrDefault(ct => ct.Ma_CT == mact);
                ct1.Ten_CT = tenct;
                ct1.Ma_EC = maec;
                ct1.Ma_BaselA = mabasela;
                ct1.Ma_BaselY = mabasely;
                ct1.TC_Nguyhai = tinhchat;
                ct1.Nguong_nguyhai = nguong;
                ct1.Trangthai_tontai = trangthai;
                if (tuxu == "X")
                    ct1.tuxuly = true;
                else
                    ct1.tuxuly = false;
                db.SaveChanges();
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
