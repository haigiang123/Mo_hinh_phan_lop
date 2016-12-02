using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EF;
using PagedList;
using Web_Tour.Models;
using System.Globalization;

namespace Web_Tour.Controllers
{
    public class TouristGroupController : Controller
    {
        //TourDuLichModel db = new TourDuLichModel();
        TourDuLichEntities db = new TourDuLichEntities();
        // GET: TouristGroup

        public void Load(string SearchString = "", int page = 1, int pagesize = 10)
        {
            
                IQueryable<DoanDuLich> list = db.DoanDuLiches;
                if (!string.IsNullOrEmpty(SearchString))
                {
                    list = list.Where(x => x.TenDoan.Contains(SearchString));
                }
                ViewBag.Tour = list.OrderBy(x => x.MaDoan).Select(s => new ReportInfo
                {
                    MaTour = s.MaTour,
                    TenTour = s.Tour.TenTour,
                    MaDoan = s.MaDoan,
                    TenDoan = s.TenDoan,
                    NgayKhoiHanh = s.NgayKhoiHanh,
                    NgayKetThuc = s.NgayKetThuc,
                    TinhTrang = s.TinhTrang

                }).ToPagedList(page, pagesize);
            
            
        }

        public ActionResult Index(string SearchString = "", int page = 1, int pagesize = 10)
        {
            if (ModelState.IsValid)
            {
                Load(SearchString, page, pagesize);
                ViewBag.SearchString = SearchString;
                return View();
                //ViewBag.Group = db.DoanDuLiches.ToList();
                //ViewBag.Group = db.DoanDuLiches.Select(s => new ReportInfo
                //{
                //    MaTour = s.MaTour,
                //    TenTour = s.Tour.TenTour,
                //    MaDoan = s.MaDoan,
                //    TenDoan = s.TenDoan,
                //    NgayKhoiHanh = s.NgayKhoiHanh,
                //    NgayKetThuc = s.NgayKetThuc,
                //    MoTa = s.MoTa,
                //    TinhTrang = s.TinhTrang
                //}).ToList();
            }
            return View();

        }

        [HttpGet]
        public ActionResult LoadTour(int id)
        {
            var model = db.Tours.Find(id);
            if (ModelState.IsValid)
            {                
                ViewBag.Mota = model.MoTa;
                return View(model);
            }
            return View(model);
        }

        public void SetViewBag(long? selectedId = null)
        {
            var Name = db.Tours;
            ViewBag.MaTour = new SelectList(Name, "MaTour", "TenTour", selectedId);
        }

        [HttpGet]
        public ActionResult Insert()
        {
            SetViewBag();
            return View();
        }

        [HttpPost]
        public ActionResult Insert(DoanDuLich model, int MaTour)
        {
            if (ModelState.IsValid)
            {
                var tour = db.Tours.Find(MaTour);
                var dem = tour.SoDem;
                var ngay = tour.SoNgay;

                if (dem > ngay)
                {
                    model.NgayKetThuc = model.NgayKhoiHanh.AddDays(Convert.ToDouble(dem));
                }
                else if (dem < ngay)
                {
                    model.NgayKetThuc = model.NgayKhoiHanh.AddDays(Convert.ToDouble(ngay));
                }
                else if (dem == ngay)
                {
                    model.NgayKetThuc = model.NgayKhoiHanh.AddDays(Convert.ToDouble(ngay));
                }
                model.TinhTrang = 1;
                db.DoanDuLiches.Add(model);
                db.SaveChanges();
                Load();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Update(int id)
        {
            SetViewBag();
            var model = db.DoanDuLiches.Find(id);
            return View(model);
        }

        [HttpPost]
        public ActionResult Update(DoanDuLich model, int MaTour)
        {
            if (ModelState.IsValid)
            {
                var tour = db.Tours.Find(MaTour);
                var dem = tour.SoDem;
                var ngay = tour.SoNgay;

                if (dem > ngay)
                {
                    model.NgayKetThuc = model.NgayKhoiHanh.AddDays(Convert.ToDouble(dem));
                }
                else if (dem < ngay)
                {
                    model.NgayKetThuc = model.NgayKhoiHanh.AddDays(Convert.ToDouble(ngay));
                }
                else if (dem == ngay)
                {
                    model.NgayKetThuc = model.NgayKhoiHanh.AddDays(Convert.ToDouble(ngay));
                }
                model.TinhTrang = 1;
                db.Entry(model).State = System.Data.Entity.EntityState.Modified;

                db.SaveChanges();
                Load();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        public int ChangeStatus(int id)
        {
            var user = db.DoanDuLiches.Find(id);
            if (user.TinhTrang == 1)
            {
                user.TinhTrang = 2;
            }
            else if (user.TinhTrang == 2)
            {
                user.TinhTrang = 1;
            }
            db.SaveChanges();
            return user.TinhTrang;
        }

        public ActionResult UpdateStatus(int id)
        {
            var result = ChangeStatus(id);
            return Json(new
            {
                status = result
            });

        }

        public ActionResult SearchCustomer(int Id)
        {
            //var model = db.DoanDuLiches.OrderBy(x=>x.MaDoan).Where(x=>x.MaDoan==Id).Select(s => new CusInfo
            //{
            //    MaKhachhang = s.KhachTheoDoans.Select(g => new CusInfoA {
            //        MaKhachHang=g.KhachHang.MaKhachHang,
            //        TenKhachHang=g.KhachHang.TenKhachHang,
            //        SDT=g.KhachHang.SDT,
            //        GioiTinh=g.KhachHang.GioiTinh,
            //        DiaChi=g.KhachHang.DiaChi,
            //        PassportNumber=g.KhachHang.PassportNumber,
            //        TinhTrang=g.KhachHang.TinhTrang
            //    })
            //}).ToList();
            SetViewBagCus();
            ViewBag.madoan = Id;
            var model = db.KhachTheoDoans.OrderBy(x => x.MaDoan).Where(x => x.MaDoan == Id).Select(g => new CusInfoA
            {
                MaKhachHang = g.KhachHang.MaKhachHang,
                TenKhachHang = g.KhachHang.TenKhachHang,
                SDT = g.KhachHang.SDT,
                GioiTinh = g.KhachHang.GioiTinh,
                DiaChi = g.KhachHang.DiaChi,
                PassportNumber = g.KhachHang.PassportNumber,
                TinhTrang = g.KhachHang.TinhTrang,
                Chitiet=g.Chitiet,
                checktinhtrang=g.DoanDuLich.TinhTrang,
                MaDoan=g.MaDoan
            }).ToList();

            var check = db.DoanDuLiches.Find(Id);
            ViewBag.checkstatus = check.TinhTrang;
            

            return View(model);
        }

        [HttpPost]
        public ActionResult SearchCustomer()
        {
            return View();
        }

        public void SetViewBagCus(long? selectedId = null)
        {
            var Name = db.KhachHangs;
            ViewBag.MaKhachHang = new SelectList(Name, "TenKhachHang", "TenKhachHang", selectedId);
        }

        [HttpPost]
        public ActionResult JSonSearch(String Keyword = "")
        {
            var model = db.KhachHangs
                .Where(p => p.TenKhachHang.Contains(Keyword))
                .Select(p => new {p.MaKhachHang,p.TenKhachHang,p.SDT,p.GioiTinh,p.DiaChi,p.PassportNumber});
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public bool ChangeStatusCustom(int id,int dd)
        {
            var user = db.KhachTheoDoans.SingleOrDefault(x=>x.MaKhachHang==id&&x.MaDoan==dd);
            user.Chitiet = !user.Chitiet;
            db.SaveChanges();
            return user.Chitiet;
        }

        public ActionResult UpdateStatusCustom(int id,int dd)
        {
            var result = ChangeStatusCustom(id,dd);
            return Json(new
            {
                status = result
            });

        }

        public ActionResult Category()
        {
            ViewBag.Cates = new[]{
                "Điện thoại di động",
                "Máy tính xách tay",
                "Máy tính để bàn",
                "Quạt máy",
                "Tivi",
                "Tủ lạnh",
                "Máy ảnh",
                "Nữ trang"
            };

            return PartialView("_Category");
        }

        public ActionResult AddCus(/*KhachTheoDoan model*/)
        {
            var madoan = Convert.ToInt32(Request["madoan"]);
            var makhachhang = Request.Form.GetValues("makhachhang");
            if (makhachhang != null)
            {
                for (int i = 0; i < makhachhang.Length; i++)
                {
                    KhachTheoDoan model = new KhachTheoDoan();
                    model.MaDoan = madoan;
                    model.MaKhachHang = Convert.ToInt32(makhachhang[i]);
                    model.Chitiet = true;
                    db.KhachTheoDoans.Add(model);
                    db.SaveChanges();
                    model = null;
                }
                Load();
                return RedirectToAction("Index");
            }
            return Redirect("http://localhost:53465/TouristGroup/SearchCustomer/"+madoan);   
        }
    }
}