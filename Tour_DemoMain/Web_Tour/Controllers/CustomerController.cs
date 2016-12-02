using EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web_Tour.Controllers
{
    public class CustomerController : Controller
    {
        TourDuLichEntities db = new TourDuLichEntities();
        // GET: Customer
        public ActionResult Index()
        {
            var model = db.KhachHangs.OrderByDescending(x=>x.MaKhachHang).ToList();
            return View(model);
        }

        public ActionResult Insert()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Insert(KhachHang khachhang)
        {

            db.KhachHangs.Add(khachhang);
            db.SaveChanges();
            return View("Index");
        }
    }
}