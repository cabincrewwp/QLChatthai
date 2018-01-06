using System;
using System.IO;
using System.IO.Compression;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Novacode;
using QLChatThai.Models.QLCTModel;

namespace QLChatThai.Controllers
{
    public class NhapController : Controller 
    {
        private QLCTModel db = new QLCTModel();

        // GET: /Nhap/
        public ActionResult Index(int page = 1, string sort = "Code_KH", string sortdir = "asc", string search = "")
        {
            int pageSize = 20;
            int totalRecord = 0;
            if (page < 1) page = 1;
            int skip = (page * pageSize) - pageSize;
            var data = GetPN(search, sort, sortdir, skip, pageSize, out totalRecord);
            ViewBag.TotalRows = totalRecord;
            ViewBag.search = search;
            return View(data);

        }
        public List<dsnhap> GetPN(string search, string sort, string sortdir, int skip, int pageSize, out int totalRecord)
        {
            var listnhaps = (from n in db.Nhaps
                             join kh in db.Khachhangs on n.Code_KH equals kh.Code_KH //Ma_KH
                             join kho in db.DMKHOes on n.Makho equals kho.makho
                             join nv in db.Nhanviens on n.manv equals nv.manv
                             where  kh.Code_KH.Contains(search)||
                                    kh.Ten_KH.Contains(search)||
                                    nv.fullname.Contains(search)
                             select new dsnhap()
                             {
                                 Code_KH = kh.Code_KH,
                                 sophieu = n.sophieu,
                                 Sochungtu = n.Sochungtu,
                                 khachhang = kh.Ten_KH,
                                 kho = kho.Tenkho,
                                 ngaynhan = n.ngaynhan,
                                 ngaynhap = n.ngaynhap,
                                 NV_Gioithieu1 = n.NV_Gioithieu1,
                                 NV_Gioithieu2 = n.NV_Gioithieu2,
                                 NV_Gioithieu3 = n.NV_Gioithieu3,
                                 NV_KDMua1 = n.NV_KDMua1,
                                 NV_KDMua2 = n.NV_KDMua2,
                                 NV_KDMua3 = n.NV_KDMua3,
                                 Ghichu = n.Ghichu,
                                 status = n.status,
                                 nguoinhan = nv.fullname,
                                 Soxe = n.Soxe
                             }).OrderBy(sort + " " + sortdir);

            totalRecord = listnhaps.Count();
            
            if (pageSize > 0)
            {
                listnhaps = listnhaps.Skip(skip).Take(pageSize);
            }
            
            return listnhaps.ToList();
        }
        public ActionResult Add()
        {
            ViewBag.Soxe = new SelectList(db.DMXes, "soxe", "soxe");
            ViewBag.makho = new SelectList(db.DMKHOes, "makho", "Tenkho");//Ma_KH
            ViewBag.Ma_KH = new SelectList(db.Khachhangs, "Code_KH", "Ten_KH");
            ViewBag.manv = new SelectList(db.Nhanviens, "manv", "fullname");
            ViewBag.NV_KDMua1 = new SelectList(db.Nhanviens, "fullname", "fullname");
            ViewBag.NV_KDMua2 = new SelectList(db.Nhanviens, "fullname", "fullname");
            ViewBag.NV_KDMua3 = new SelectList(db.Nhanviens, "fullname", "fullname");
            return View();
        }
        public ActionResult List()
        {
            var listnhaps = (from n in db.Nhaps 
                                 join kh    in db.Khachhangs    on n.Code_KH  equals kh.Code_KH //Ma_KH
                                 join kho   in db.DMKHOes       on n.Makho  equals kho.makho
                                 join nv    in db.Nhanviens     on n.manv   equals nv.manv
                             select new dsnhap()
                             {
                                sophieu=n.sophieu,
                                Sochungtu=n.Sochungtu,
                                khachhang=kh.Ten_KH,
                                kho=kho.Tenkho,
                                ngaynhan=n.ngaynhan,
                                ngaynhap=n.ngaynhap,
                                NV_Gioithieu1=n.NV_Gioithieu1,
                                NV_Gioithieu2 = n.NV_Gioithieu2,
                                NV_Gioithieu3 = n.NV_Gioithieu3,
                                NV_KDMua1=n.NV_KDMua1,
                                NV_KDMua2 = n.NV_KDMua2,
                                NV_KDMua3 = n.NV_KDMua3,
                                Ghichu=n.Ghichu,
                                status=n.status,
                                nguoinhan=nv.fullname,
                                Soxe=n.Soxe
                             });
            return View(listnhaps.ToList());
        }
        public ActionResult Chungtu()
        {
            return View();
        }
        
        //public ActionResult Lapchungtu()
        //{
        //    return View();
        //}
        
        //[HttpPost]
        //public ActionResult Lapchungtu(DateTime tungay, DateTime toingay)
        public ActionResult Lapchungtu(FormCollection fields)
        {
            //DateTime tungay =Convert.ToDateTime(Fields["tungay"]);
            //DateTime toingay = Convert.ToDateTime(Fields["toingay"]);
            DateTime tungay = DateTime.ParseExact(fields["txttungay"],"dd/MM/yyyy",null);
            DateTime toingay = DateTime.ParseExact(fields["txttoingay"], "dd/MM/yyyy", null);
            ViewBag.giaidoan = tungay.ToString().Substring(0,10) + " -> " + toingay.ToString().Substring(0,10);
            List<LapCT> DSCT = new List<LapCT>();
            //Danh sách Code_KH trong khoảng thời gian lập chứng từ
            var dskh=(from n in db.Nhaps 
                          where (n.ngaynhan >=tungay) && (n.ngaynhan<=toingay)
                          select new {
                            n.Code_KH
                          }).Distinct();
            //Láy thông tin khách hàng
            foreach (var kh in dskh)
            {
                Khachhang okh = db.Khachhangs.Find(kh.Code_KH.ToUpper());
                var oct = (from n in db.Nhaps
                       join ct in db.CTNHaps on n.sophieu equals ct.Sophieu
                       join xl in db.Xulies on ct.idxl equals xl.idxl
                       join dmct in db.DM_CT on ct.idct equals dmct.idct
                       where (n.ngaynhan >= tungay) && (n.ngaynhan <= toingay) && (n.Code_KH == kh.Code_KH) && (ct.slthuc > 0)
                       group new { n.Code_KH,dmct.Ten_CT,dmct.Ma_CT, ct.dvt, ct.trangthai, xl.phuongphap,dmct.tuxuly, ct.slthuc }
                       by new { n.Code_KH, dmct.Ten_CT, dmct.Ma_CT, ct.dvt, ct.trangthai, xl.phuongphap, dmct.tuxuly } into g
                       select new CTLapCT()
                       {
                           Code_KH = g.Key.Code_KH.ToUpper(),
                           TenCT = g.Key.Ten_CT,
                           MaQLCTNH=g.Key.Ma_CT,
                           dvt = g.Key.dvt,
                           trangthai = g.Key.trangthai,
                           phuongphapxl = g.Key.phuongphap,
                           tuxuly=g.Key.tuxuly,
                           soluong = (double)g.Sum(x => x.slthuc)
                       });

                DSCT.Add(new LapCT { khachhang = okh,tu_ngay=tungay,toi_ngay=toingay,CTlapct= oct.ToList()});
            }
            return View(DSCT.ToList());

           // var chungtu = (from n in db.Nhaps
           //                    where (n.ngaynhap >=tungay) && (n.ngaynhap<=toingay)
           //                    join ct in db.CTNHaps on n.sophieu equals ct.Sophieu
           //                    join xl in db.Xulies on ct.idxl equals xl.idxl
           //                    join kh in db.Khachhangs on n.Code_KH equals kh.Code_KH
           //                    join dmct in db.DM_CT on ct.idct equals dmct.idct
           //                    group new { n.Code_KH, kh.Ten_KH, dmct.Ten_CT, ct.dvt, ct.trangthai, xl.phuongphap, ct.slthuc }
           //                    by new { n.Code_KH, kh.Ten_KH, dmct.Ten_CT, ct.dvt, ct.trangthai, xl.phuongphap } into g
           //                    select new Chungtu()
           //                    {
           //                        Code_KH = g.Key.Code_KH,
           //                        Ten_KH = g.Key.Ten_KH,
           //                        Ten_CT = g.Key.Ten_CT,
           //                        dvt = g.Key.dvt,
           //                        trangthai = g.Key.trangthai,
           //                        phuongphap = g.Key.phuongphap,
           //                        soluong = g.Sum(x => x.slthuc)
           //                    }
           //               );

           //return View(chungtu.ToList());
            
        }

        public FileResult TaoCT()
        {
            //Biến nhận
            String ma_kh = Request["makh"];
            DateTime tungay = DateTime.ParseExact(Request["tungay"], "dd/MM/yyyy", null);
            DateTime toingay = DateTime.ParseExact(Request["toingay"], "dd/MM/yyyy", null);
            
            Khachhang khachhang = db.Khachhangs.Find(ma_kh.ToUpper());
            //Khachhang khachhang = db.Khachhangs.FirstOrDefault(kh => kh.Code_KH == ma_kh);

            //Mau 6 lien đơn vị tự xử lý
            var chungtu = (from n in db.Nhaps
                           join ct in db.CTNHaps on n.sophieu equals ct.Sophieu
                           join xl in db.Xulies on ct.idxl equals xl.idxl
                           join kh in db.Khachhangs on n.Code_KH equals kh.Code_KH
                           join dmct in db.DM_CT on ct.idct equals dmct.idct
                           where (n.Code_KH == ma_kh) && (n.ngaynhan >= tungay) && (n.ngaynhan <= toingay)
                           group new { n.Code_KH, kh.Ten_KH, dmct.Ten_CT, ct.dvt, ct.trangthai, xl.phuongphap, ct.slthuc }
                           by new { n.Code_KH, kh.Ten_KH, dmct.Ten_CT, ct.dvt, ct.trangthai, xl.phuongphap } into g
                           select new Chungtu()
                           {
                               Code_KH = g.Key.Code_KH,
                               Ten_KH = g.Key.Ten_KH,
                               Ten_CT = g.Key.Ten_CT,
                               dvt = g.Key.dvt,
                               trangthai = g.Key.trangthai,
                               phuongphap = g.Key.phuongphap,
                               soluong = g.Sum(x => x.slthuc)
                           }
                          );
            
            var filename = Server.MapPath("~/Mau/Maunew.docx");
            var fileluu = Server.MapPath("~/Baocao/Chungtu_" + ma_kh + ".docx");
            
            var template = DocX.Load(filename);
            var dtvp = khachhang.Dienthoaivp;
            var dtcs = khachhang.Dienthoaics;
            if (String.IsNullOrEmpty(dtvp))
            {
                dtvp = "....................";
            }
            if (String.IsNullOrEmpty(dtcs))
            {
                dtcs = "....................";
            }

            template.Bookmarks["tinhthanh"].SetText(khachhang.Tinhthanh.Ten);
            template.Bookmarks["chungtu"].SetText("");//model.Sochungtu);
            template.Bookmarks["dienthoai"].SetText(dtvp);
            template.Bookmarks["dienthoaics"].SetText(dtcs);
            template.Bookmarks["khachhang"].SetText(khachhang.Ten_KH);
            if (String.IsNullOrEmpty(khachhang.MS_QLCTNH))
            {
                template.Bookmarks["maqlctnh"].SetText("");
            }
            else
            {
                template.Bookmarks["maqlctnh"].SetText(khachhang.MS_QLCTNH);
            }

            template.Bookmarks["dcvp"].SetText(khachhang.Diachi_VP);
            template.Bookmarks["dccs"].SetText(khachhang.Diachi_CS);
            Table table = template.Tables[0];

            var result = (from ct in db.CTNHaps
                          join pn in db.Nhaps on ct.Sophieu equals pn.sophieu
                          join dm in db.DM_CT on ct.idct equals dm.idct
                          join xl in db.Xulies on ct.idxl equals xl.idxl
                          where (pn.Code_KH == ma_kh) && (pn.ngaynhan >= tungay) && (pn.ngaynhan <= toingay) && (dm.tuxuly)
                          group new { dm.Ma_CT, dm.Ten_CT, ct.dvt, ct.trangthai, xl.phuongphap, ct.slthuc }
                          by new { dm.Ma_CT, dm.Ten_CT, ct.dvt, ct.trangthai, xl.phuongphap } into g
                          select new Chungtu()
                          {
                              Code_KH = khachhang.Code_KH,
                              Ten_KH = khachhang.Ten_KH,
                              Ten_CT = g.Key.Ten_CT,
                              Ma_QLCTNH = g.Key.Ma_CT,
                              dvt = g.Key.dvt,
                              phuongphap = g.Key.phuongphap,
                              trangthai = g.Key.trangthai,
                              soluong = g.Sum(x => x.slthuc)
                          });


            int i = 0;
            int k = 2;
            table.Design = TableDesign.TableGrid;
            //=table.ColumnWidths();

            foreach (var oct in result.ToList())
            {
                table.InsertRow(k + i);
                i++;
                table.Rows[k + i - 1].Cells[0].Paragraphs.First().Append(i.ToString());

                table.Rows[k + i - 1].Cells[1].Paragraphs.First().Append(oct.Ten_CT);
                table.Rows[k + i - 1].Cells[5].Paragraphs.First().Append(oct.Ma_QLCTNH);
                table.Rows[k + i - 1].Cells[6].Paragraphs.First().Append(oct.soluong.ToString());
                table.Rows[k + i - 1].Cells[7].Paragraphs.First().Append(oct.phuongphap);
                if (String.IsNullOrEmpty(oct.trangthai))
                {
                }
                else
                {
                    if (oct.trangthai.ToLower() == "rắn")
                    {
                        table.Rows[k + i - 1].Cells[2].Paragraphs.First().Append("x");
                    }
                    else if (oct.trangthai.ToLower() == "lỏng")
                    {
                        table.Rows[k + i - 1].Cells[3].Paragraphs.First().Append("x");
                    }
                    else if (oct.trangthai.ToLower() == "bùn")
                    {
                        table.Rows[k + i - 1].Cells[4].Paragraphs.First().Append("x");
                    }
                }

                table.Rows[k + i - 1].Cells[0].Width = 30;
                table.Rows[k + i - 1].Cells[1].Width = 450;
                table.Rows[k + i - 1].Cells[2].Width = 10;
                table.Rows[k + i - 1].Cells[3].Width = 10;
                table.Rows[k + i - 1].Cells[4].Width = 10;
                table.Rows[k + i - 1].Cells[5].Width = 50;
                table.Rows[k + i - 1].Cells[6].Width = 30;
                table.Rows[k + i - 1].Cells[7].Width = 100;
                table.Rows[k + i - 1].Cells[6].Paragraphs.First().Alignment = Alignment.center;
                table.Rows[k + i - 1].Cells[5].Paragraphs.First().Alignment = Alignment.center;
                table.Rows[k + i - 1].Cells[2].Paragraphs.First().Alignment = Alignment.center;
                table.Rows[k + i - 1].Cells[3].Paragraphs.First().Alignment = Alignment.center;
                table.Rows[k + i - 1].Cells[4].Paragraphs.First().Alignment = Alignment.center;
                table.Rows[k + i - 1].Cells[6].Paragraphs.First().Alignment = Alignment.right;
                
            }


            template.SaveAs(fileluu);
            
            
            //Phan khong xu ly


            var result1 = (from ct in db.CTNHaps
                           join pn in db.Nhaps on ct.Sophieu equals pn.sophieu
                           join dm in db.DM_CT on ct.idct equals dm.idct
                           join xl in db.Xulies on ct.idxl equals xl.idxl
                           where (pn.Code_KH == ma_kh) && (pn.ngaynhap >= tungay) && (pn.ngaynhap <= toingay) && (!dm.tuxuly)
                           group new { dm.Ma_CT, dm.Ten_CT, ct.dvt, ct.trangthai, xl.phuongphap, ct.slthuc }
                           by new { dm.Ma_CT, dm.Ten_CT, ct.dvt, ct.trangthai, xl.phuongphap } into g
                           select new Chungtu()
                           {
                               Code_KH = khachhang.Code_KH,
                               Ten_KH = khachhang.Ten_KH,
                               Ten_CT = g.Key.Ten_CT,
                               Ma_QLCTNH = g.Key.Ma_CT,
                               dvt = g.Key.dvt,
                               phuongphap = g.Key.phuongphap,
                               trangthai = g.Key.trangthai,
                               soluong = g.Sum(x => x.slthuc)
                           });
            // co chat thai khong xu ly
            if (result1.Count() > 0)
            {
                var filename1 = Server.MapPath("~/Mau/Mau9.docx");
                var fileluu1 = Server.MapPath("~/Baocao/Chungtu_KXL" + ma_kh + ".docx");
                var template1 = DocX.Load(filename1);

                template1.Bookmarks["tinhthanh"].SetText(khachhang.Tinhthanh.Ten);
                template1.Bookmarks["chungtu"].SetText("");//model.Sochungtu);
                template1.Bookmarks["dienthoai"].SetText(dtvp);
                template1.Bookmarks["dienthoaics"].SetText(dtcs);
                template1.Bookmarks["khachhang"].SetText(khachhang.Ten_KH);
                if (String.IsNullOrEmpty(khachhang.MS_QLCTNH))
                {
                    template1.Bookmarks["maqlctnh"].SetText("");
                }
                else
                {
                    template1.Bookmarks["maqlctnh"].SetText(khachhang.MS_QLCTNH);
                }

                template1.Bookmarks["dcvp"].SetText(khachhang.Diachi_VP);
                template1.Bookmarks["dccs"].SetText(khachhang.Diachi_CS);


                Table table1 = template1.Tables[0];
                i = 0;
                k = 2;
                table1.Design = TableDesign.TableGrid;

                foreach (var oct in result1.ToList())
                {
                    table1.InsertRow(k + i);
                    i++;
                    table1.Rows[k + i - 1].Cells[0].Paragraphs.First().Append(i.ToString());
                    table1.Rows[k + i - 1].Cells[1].Paragraphs.First().Append(oct.Ten_CT);
                    table1.Rows[k + i - 1].Cells[5].Paragraphs.First().Append(oct.Ma_QLCTNH);
                    table1.Rows[k + i - 1].Cells[6].Paragraphs.First().Append(oct.soluong.ToString());
                    table1.Rows[k + i - 1].Cells[7].Paragraphs.First().Append(oct.phuongphap);
                    if (String.IsNullOrEmpty(oct.trangthai))
                    {
                    }
                    else
                    {
                        if (oct.trangthai.ToLower() == "rắn")
                        {
                            table1.Rows[k + i - 1].Cells[2].Paragraphs.First().Append("x");
                        }
                        else if (oct.trangthai.ToLower() == "lỏng")
                        {
                            table1.Rows[k + i - 1].Cells[3].Paragraphs.First().Append("x");
                        }
                        else if (oct.trangthai.ToLower() == "bùn")
                        {
                            table1.Rows[k + i - 1].Cells[4].Paragraphs.First().Append("x");
                        }
                    }

                    table1.Rows[k + i - 1].Cells[0].Width = 30;
                    table1.Rows[k + i - 1].Cells[1].Width = 450;
                    table1.Rows[k + i - 1].Cells[2].Width = 10;
                    table1.Rows[k + i - 1].Cells[3].Width = 10;
                    table1.Rows[k + i - 1].Cells[4].Width = 10;
                    table1.Rows[k + i - 1].Cells[5].Width = 50;
                    table1.Rows[k + i - 1].Cells[6].Width = 30;
                    table1.Rows[k + i - 1].Cells[7].Width = 100;
                    table1.Rows[k + i - 1].Cells[6].Paragraphs.First().Alignment = Alignment.center;
                    table1.Rows[k + i - 1].Cells[5].Paragraphs.First().Alignment = Alignment.center;
                    table1.Rows[k + i - 1].Cells[2].Paragraphs.First().Alignment = Alignment.center;
                    table1.Rows[k + i - 1].Cells[3].Paragraphs.First().Alignment = Alignment.center;
                    table1.Rows[k + i - 1].Cells[4].Paragraphs.First().Alignment = Alignment.center;
                    table1.Rows[k + i - 1].Cells[6].Paragraphs.First().Alignment = Alignment.right;

                }
                template1.SaveAs(fileluu1);
                //Tạo file nén và trả về
                using (var memoryStream = new MemoryStream())
                {
                    using (var ziparchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                    {
                        //var memoryStream = new MemoryStream();
                        //var ziparchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true);
                        // ziparchive.CreateEntry("~/Baocao/chungtu.zip");
                        //ziparchive..CreateEntryFromFile(Server.MapPath("~/Files/file1.txt"), "file1.txt");
                        ziparchive.CreateEntryFromFile(fileluu, "Chungtu_" + ma_kh + ".docx");
                        ziparchive.CreateEntryFromFile(fileluu1, "Chungtu_KXL" + ma_kh + ".docx", CompressionLevel.Fastest);
                    }

                    return File(memoryStream.ToArray(), "application/zip", "Chungtu.zip");
                }
            }
            else
            {
                //Neu không có phần do công ty khách xử lý thì trả về file word báo cáo xử ly.
                //Nếu có phần do công ty ngoài xử lý thì trả về file zip 
                System.IO.FileStream fs = System.IO.File.OpenRead(fileluu);
                byte[] data = new byte[fs.Length];
                int br = fs.Read(data, 0, data.Length);
                if (br != fs.Length)
                    throw new System.IO.IOException();

                return File(data, "application/msword", "Chungtu.docx");
            }
            
            
            //return View();
        }
        public ActionResult LapCT()
        {
            return View();
        } 
        [HttpPost]
        public ActionResult LapCT(int[] lstSophieus)
        {
            if (lstSophieus != null)
            {
                var chungtu = (from n in db.Nhaps
                               where lstSophieus.Contains(n.sophieu)
                               join ct in db.CTNHaps on n.sophieu equals ct.Sophieu
                               join xl in db.Xulies on ct.idxl equals xl.idxl
                               join kh in db.Khachhangs on n.Code_KH equals kh.Code_KH
                               join dmct in db.DM_CT on ct.idct equals dmct.idct
                               group new { n.Code_KH, kh.Ten_KH, dmct.Ten_CT, ct.dvt, ct.trangthai, xl.phuongphap, ct.slthuc }
                               by new { n.Code_KH, kh.Ten_KH, dmct.Ten_CT, ct.dvt, ct.trangthai, xl.phuongphap } into g
                               select new Chungtu()
                               {
                                   Code_KH = g.Key.Code_KH,
                                   Ten_KH = g.Key.Ten_KH,
                                   Ten_CT = g.Key.Ten_CT,
                                   dvt = g.Key.dvt,
                                   trangthai = g.Key.trangthai,
                                   phuongphap = g.Key.phuongphap,
                                   soluong = g.Sum(x => x.slthuc)
                               }
                          );

                //var chungtu = (from n in db.Nhaps
                //               where lstSophieus.Contains(n.sophieu)
                //               join ct in db.CTNHaps on n.sophieu equals ct.Sophieu                               
                //               group new { n.Code_KH, ct.idct, ct.dvt, ct.trangthai, ct.idxl, ct.slthuc }
                //               by new { n.Code_KH, ct.idct, ct.dvt, ct.trangthai, ct.idxl } into g
                //               select new Chungtu()
                //               {
                //                   Code_KH = g.Key.Code_KH,
                //                   idct = g.Key.idct,
                //                   dvt = g.Key.dvt,
                //                   trangthai = g.Key.trangthai,
                //                   idxl = g.Key.idxl,
                //                   soluong = g.Sum(x => x.slthuc)
                //               }
                //         );
                return View(chungtu.ToList());
            }
            // Lập chứng từ toàn bộ các khách hàng
            else 
            {
                
                var chungtu = (from n in db.Nhaps
                               join ct in db.CTNHaps on n.sophieu equals ct.Sophieu
                               join xl in db.Xulies on ct.idxl equals xl.idxl 
                               join kh in db.Khachhangs on n.Code_KH equals kh.Code_KH 
                               join dmct in db.DM_CT on ct.idct equals dmct.idct 
                               group new { n.Code_KH,kh.Ten_KH, dmct.Ten_CT, ct.dvt, ct.trangthai,xl.phuongphap, ct.slthuc }
                               by new { n.Code_KH, kh.Ten_KH, dmct.Ten_CT, ct.dvt, ct.trangthai, xl.phuongphap } into g
                               select new Chungtu()
                               {
                                   Code_KH=g.Key.Code_KH,
                                   Ten_KH=g.Key.Ten_KH,
                                   Ten_CT=g.Key.Ten_CT,
                                   dvt=g.Key.dvt,
                                   trangthai=g.Key.trangthai,
                                   phuongphap=g.Key.phuongphap,
                                   soluong=g.Sum(x=>x.slthuc)
                               }
                          );
                return View(chungtu.ToList());    
            }
            
        }
        public JsonResult GetCodeKH(string key)
        {
            var CodeKH = (from kh in db.Khachhangs  
                          where kh.Code_KH.Contains(key)  
                          select new {kh.Code_KH,kh.Ten_KH});  
           return Json(CodeKH, JsonRequestBehavior.AllowGet);  

        }
        
        // GET: /Nhap/Details/5
        public ActionResult Details(int id)
        {

            var pn = (from n in db.Nhaps
                          where n.sophieu==id
                          join nv in db.Nhanviens on n.manv equals nv.manv
                          join kh in db.Khachhangs on n.Code_KH equals kh.Code_KH 
                          join kho in db.DMKHOes on n.Makho equals kho.makho
                      select new NhapMaster()
                      {
                          sophieu=id,
                          Sochungtu=n.Sochungtu,
                          Ten_KH=kh.Ten_KH,
                          Tenkho=kho.Tenkho,
                          ngaynhan=n.ngaynhan,
                          ngaynhap=n.ngaynhap,
                          Soxe=n.Soxe,
                          fullname=nv.fullname,
                          NV_KDMua1=n.NV_KDMua1,
                          NV_KDMua2 = n.NV_KDMua2,
                          NV_KDMua3 = n.NV_KDMua3,
                          NV_Gioithieu1=n.NV_Gioithieu1,
                          NV_Gioithieu2=n.NV_Gioithieu2,
                          NV_Gioithieu3=n.NV_Gioithieu2,
                          Ghichu=n.Ghichu
                      }).FirstOrDefault();

            ViewBag.Nhap = pn;
            
            var resultss = (from ct in db.CTNHaps
                            where (ct.Sophieu ==id) && (!ct.loai)
                            join xl in db.Xulies on ct.idxl equals xl.idxl                           
                           select new NhapDetail()
                           {
                               idctnhap=ct.idctnhap,
                               sophieu=id,                               
                               Tenct=ct.Tenct,
                               dvt=ct.dvt,                               
                               trangthai=ct.trangthai,
                               phuongphap=xl.phuongphap,
                               slthuc=ct.slthuc,
                               loai = ct.loai,
                               dongianhap=0
                           });
            ViewBag.resultss = resultss.ToList();
            var resulttt = (from ct in db.CTNHaps
                            where ct.Sophieu == id && ct.loai
                            join xl in db.Xulies on ct.idxl equals xl.idxl
                            select new NhapDetail()
                            {
                                idctnhap = ct.idctnhap,
                                sophieu = id,
                                Tenct = ct.Tenct,
                                dvt = ct.dvt,
                                trangthai = ct.trangthai,
                                phuongphap = xl.phuongphap,
                                slthuc = ct.slthuc,
                                dongianhap=ct.Dongianhap,
                                loai = ct.loai
                            });
            ViewBag.resulttt = resulttt.ToList();
            return View();
        }

        // GET: /Nhap/Create
        public ActionResult Create()
        {
            ViewBag.Soxe = new SelectList(db.DMXes, "soxe", "soxe");
            ViewBag.makho = new SelectList(db.DMKHOes, "makho", "Tenkho");
            ViewBag.Code_KH = new SelectList(db.Khachhangs, "Code_KH", "Ten_KH");
            ViewBag.manv = new SelectList(db.Nhanviens, "manv", "fullname");
            ViewBag.NV_KDMua1 = new SelectList(db.Nhanviens, "fullname", "fullname");
            ViewBag.NV_KDMua2 = new SelectList(db.Nhanviens, "fullname", "fullname");
            ViewBag.NV_KDMua3 = new SelectList(db.Nhanviens, "fullname", "fullname");
            return View();
        }

        // POST: /Nhap/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="Sophieu,Sochungtu,Code_KH,makho,ngaynhan,ngaynhap,NV_Gioithieu1,NV_Gioithieu2,NV_Gioithieu3,NV_KDMua1,NV_KDMua2,NV_KDMua3,Ghichu,manv,soxe")] Nhap nhap)
        {
            //if (ModelState.IsValid)
            //{
                db.Nhaps.Add(nhap);
                db.SaveChanges();
                return RedirectToAction("Index");
            //}
            //ViewBag.Soxe = new SelectList(db.DMXes, "soxe", "soxe", nhap.Soxe);
            //ViewBag.Makho = new SelectList(db.DMKHOes, "Makho", "Tenkho", nhap.Makho);
            //ViewBag.Ma_KH = new SelectList(db.Khachhangs, "Ma_KH", "Ten_KH", nhap.Ma_KH);
            //ViewBag.manv = new SelectList(db.Nhanviens, "manv", "fullname", nhap.manv);
            //ViewBag.NV_KDMua1 = new SelectList(db.Nhanviens, "fullname", "fullname");
            //ViewBag.NV_KDMua2 = new SelectList(db.Nhanviens, "fullname", "fullname");
            //ViewBag.NV_KDMua3 = new SelectList(db.Nhanviens, "fullname", "fullname");
            //return View(nhap);

        }

        // GET: /Nhap/Edit/5
        public ActionResult Edit(int id)
        {
            Nhap nhap = db.Nhaps.Find(id);
            if (nhap == null)
            {
                return HttpNotFound();
            }
            ViewBag.Soxe = new SelectList(db.DMXes, "soxe", "soxe", nhap.Soxe);
            ViewBag.makho = new SelectList(db.DMKHOes, "makho", "Tenkho", nhap.Makho);
            ViewBag.Code_KH = new SelectList(db.Khachhangs, "Code_KH", "Ten_KH", nhap.Code_KH);
            ViewBag.manv = new SelectList(db.Nhanviens, "manv", "fullname", nhap.manv);
            ViewBag.NV_KDMua1 = new SelectList(db.Nhanviens, "fullname", "fullname");
            ViewBag.NV_KDMua2 = new SelectList(db.Nhanviens, "fullname", "fullname");
            ViewBag.NV_KDMua3 = new SelectList(db.Nhanviens, "fullname", "fullname");
            return View(nhap);
        }

        // POST: /Nhap/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Sophieu,Sochungtu,Code_KH,makho,ngaynhan,ngaynhap,NV_Gioithieu1,NV_Gioithieu2,NV_Gioithieu3,NV_KDMua1,NV_KDMua2,NV_KDMua3,Ghichu,manv,soxe")] Nhap nhap)
        {
            //if (ModelState.IsValid)
            //{
                db.Entry(nhap).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            //}
            //ViewBag.Soxe = new SelectList(db.DMXes, "soxe", "soxe", nhap.Soxe);
            //ViewBag.makho = new SelectList(db.DMKHOes, "makho", "Tenkho", nhap.Makho);
            //ViewBag.Ma_KH = new SelectList(db.Khachhangs, "Ma_KH", "Ten_KH", nhap.Ma_KH);
            //ViewBag.manv = new SelectList(db.Nhanviens, "manv", "fullname", nhap.manv);
            //ViewBag.NV_KDMua1 = new SelectList(db.Nhanviens, "fullname", "fullname");
            //ViewBag.NV_KDMua2 = new SelectList(db.Nhanviens, "fullname", "fullname");
            //ViewBag.NV_KDMua3 = new SelectList(db.Nhanviens, "fullname", "fullname");
            //return View(nhap);
        }

        // GET: /Nhap/Delete/5
        public ActionResult Delete(int id)
        {
            
            Nhap nhap = db.Nhaps.Find(id);
            if (nhap == null)
            {
                return HttpNotFound();
            }
            return View(nhap);
        }

        // POST: /Nhap/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Nhap nhap = db.Nhaps.Find(id);
                db.CTNHaps.Where(ctn => ctn.Sophieu == nhap.sophieu).ToList().ForEach(p => db.CTNHaps.Remove(p));
                db.Nhaps.Remove(nhap);
                db.SaveChanges();
            }
            catch
            {

            }
            return RedirectToAction("Index");
        }

        //public FileResult Chungtu(int id)
        //{
            //var model = db.Khachhangs.Find(id);
            //var filename = Server.MapPath("~/Mau/test.docx");
            //var fileluu = Server.MapPath("~/Chungtu/kq.docx");
            //var template = DocX.Load(filename);
            //template.AddCustomProperty(new CustomProperty("company_slogan", model.Ten_KH));
            //template.SaveAs(fileluu);
            //System.IO.FileStream fs = System.IO.File.OpenRead(fileluu);
            //byte[] data = new byte[fs.Length];
            //int br = fs.Read(data, 0, data.Length);
            //if (br != fs.Length)
            //    throw new System.IO.IOException();
            //return File(data, "application/msword", "Chungtu");
        //}
        
        public FileResult toword(int id)
        {
            var model = db.Nhaps.Find(id);
            var filename = Server.MapPath("~/Mau/Maunew.docx");
            var fileluu = Server.MapPath("~/Baocao/Chungtu_"+id.ToString()+".docx");
            var template = DocX.Load(filename);
            var dtvp = model.Khachhang.Dienthoaivp;
            var dtcs = model.Khachhang.Dienthoaics;
            if (String.IsNullOrEmpty(dtvp))
            {
                dtvp="....................";
            }
            if (String.IsNullOrEmpty(dtcs))
            {
                dtcs = "....................";
            }
            template.Bookmarks["tinhthanh"].SetText(model.Khachhang.Tinhthanh.Ten);
            template.Bookmarks["chungtu"].SetText(model.Sochungtu);
            template.Bookmarks["dienthoai"].SetText(dtvp);
            template.Bookmarks["dienthoaics"].SetText(dtcs);
            template.Bookmarks["khachhang"].SetText(model.Khachhang.Ten_KH);
            template.Bookmarks["maqlctnh"].SetText(model.Khachhang.MS_QLCTNH);
            template.Bookmarks["dcvp"].SetText(model.Khachhang.Diachi_VP);
            template.Bookmarks["dccs"].SetText(model.Khachhang.Diachi_CS);
            Table table = template.Tables[0];
            var result = db.CTNHaps
                        .Where(ctp => ctp.Sophieu == id);
            int i = 0;
            int k = 2;
            table.Design = TableDesign.TableGrid;
            //=table.ColumnWidths();

            foreach (var ct in result.ToList())
            {
                table.InsertRow(k+i);
                i++;
                table.Rows[k+i-1].Cells[0].Paragraphs.First().Append(i.ToString());
                
                table.Rows[k+i-1].Cells[1].Paragraphs.First().Append(ct.DM_CT.Ten_CT);
                table.Rows[k + i - 1].Cells[5].Paragraphs.First().Append(ct.DM_CT.Ma_CT);
                table.Rows[k+i-1].Cells[6].Paragraphs.First().Append(ct.slbaocao.ToString());
                table.Rows[k+i-1].Cells[7].Paragraphs.First().Append(ct.Xuly.phuongphap);
                if (ct.trangthai.ToLower()=="rắn")
                {
                    table.Rows[k + i - 1].Cells[2].Paragraphs.First().Append("x");
                }
                else if (ct.trangthai.ToLower() == "lỏng")
                {
                    table.Rows[k + i - 1].Cells[3].Paragraphs.First().Append("x");
                }
                else
                {
                    table.Rows[k + i - 1].Cells[4].Paragraphs.First().Append("x");
                } 
                //table.Rows[14].Cells[2].Paragraphs.First().Append(i.ToString());
                //table.Rows[14].Cells[3].Paragraphs.First().Append(i.ToString());
                //table.Rows[14].Cells[4].Paragraphs.First().Append(i.ToString());
                //table.Rows[k+i-1].Cells[5].Paragraphs.First().Append(ct.Xuly.Ma_CT);
                //table.Rows[k+i-1].Cells[6].Paragraphs.First().Append(ct.slbaocao.ToString());
                table.Rows[k + i - 1].Cells[0].Width = 30;
                table.Rows[k + i - 1].Cells[1].Width = 450;
                table.Rows[k + i - 1].Cells[2].Width = 10;
                table.Rows[k + i - 1].Cells[3].Width = 10;
                table.Rows[k + i - 1].Cells[4].Width = 10;
                table.Rows[k + i - 1].Cells[5].Width = 50;
                table.Rows[k + i - 1].Cells[6].Width = 30;
                table.Rows[k + i - 1].Cells[7].Width = 100;
                table.Rows[k + i - 1].Cells[6].Paragraphs.First().Alignment = Alignment.center;
                table.Rows[k + i - 1].Cells[5].Paragraphs.First().Alignment = Alignment.center;
                table.Rows[k + i - 1].Cells[2].Paragraphs.First().Alignment = Alignment.center;
                table.Rows[k + i - 1].Cells[3].Paragraphs.First().Alignment = Alignment.center;
                table.Rows[k + i - 1].Cells[4].Paragraphs.First().Alignment = Alignment.center;
                table.Rows[k + i - 1].Cells[6].Paragraphs.First().Alignment = Alignment.right;
                //table.Rows[k + i - 1].Cells[5].Paragraphs.First().h
            }
            

            template.SaveAs(fileluu);
            System.IO.FileStream fs = System.IO.File.OpenRead(fileluu);
            byte[] data = new byte[fs.Length];
            int br = fs.Read(data, 0, data.Length);
            if (br != fs.Length)
                throw new System.IO.IOException();
            return File(data, "application/msword", "Chungtu");
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
