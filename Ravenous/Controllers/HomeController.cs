using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ravenous.Models;

namespace Ravenous.Controllers
{
    public class HomeController : Controller
    {
        private ravenousDBEntities db = new ravenousDBEntities();
        public ActionResult Index()
        {
            ViewBag.Name = "Mai";
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }




        // GET: Create Restaurant
        public ActionResult CreateRestaurant()
        {
            ViewBag.city = new SelectList(db.cities, "Id", "cityName");

            SelectListItem[] items = creatList();
            SelectListItem[] items2 = creatList();

            ViewBag.startTime = items;
            ViewBag.endTime = items2;
            return View();
        }


        // POST: Create Restaurant
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateRestaurant([Bind(Include = "Id,email,restaurantName,restaurantPhone,city,location,isAvailableForOccasion,isAvailableForKids,startTime,endTime")] ownerRestaurant ownerRestaurant)
        {
            if (ModelState.IsValid)
            {
                ownerRestaurant.isApproved = false;
                db.ownerRestaurants.Add(ownerRestaurant);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.city = new SelectList(db.cities, "Id", "cityName", ownerRestaurant.city);


            SelectListItem[] items = creatList();
            SelectListItem[] items2 = creatList();
            items[ownerRestaurant.startTime].Selected = true;
            items2[ownerRestaurant.endTime].Selected = true;
            ViewBag.startTime = items;
            ViewBag.endTime = items2;

            return View(ownerRestaurant);
        }
        public SelectListItem [] creatList()
        {
            SelectListItem[] items = new SelectListItem[24];
            for (int i = 0, j = 1; i < items.Length; i++)
            {
                items[i] = new SelectListItem();
                if (i < 12)
                    items[i].Text = (i + 1) + "  صباحا";
                else
                    items[i].Text = (j++) + "  مساءا";
                items[i].Value = (i + 1) + "";
            }
            return items;
        }
    }
}