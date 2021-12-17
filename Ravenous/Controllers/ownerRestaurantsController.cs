using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Ravenous.Models;

namespace Ravenous.Controllers
{
    public class ownerRestaurantsController : Controller
    {
        private ravenousDBEntities db = new ravenousDBEntities();

        // GET: ownerRestaurants
        public ActionResult Index()
        {
            var ownerRestaurants = db.ownerRestaurants.Include(o => o.city1);
            return View(ownerRestaurants.ToList());
        }

        // GET: ownerRestaurants/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ownerRestaurant ownerRestaurant = db.ownerRestaurants.Find(id);
            if (ownerRestaurant == null)
            {
                return HttpNotFound();
            }
            return View(ownerRestaurant);
        }

        // GET: ownerRestaurants/Create
        public ActionResult Create()
        {
            ViewBag.city = new SelectList(db.cities, "Id", "cityName");
            return View();
        }

        // POST: ownerRestaurants/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,email,restaurantName,restaurantPhone,city,location,isAvailableForOccasion,isAvailableForKids,startTime,endTime,isApproved")] ownerRestaurant ownerRestaurant)
        {
            if (ModelState.IsValid)
            {
                db.ownerRestaurants.Add(ownerRestaurant);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.city = new SelectList(db.cities, "Id", "cityName", ownerRestaurant.city);
            return View(ownerRestaurant);
        }

        // GET: ownerRestaurants/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ownerRestaurant ownerRestaurant = db.ownerRestaurants.Find(id);
            if (ownerRestaurant == null)
            {
                return HttpNotFound();
            }
            ViewBag.city = new SelectList(db.cities, "Id", "cityName", ownerRestaurant.city);
            return View(ownerRestaurant);
        }

        // POST: ownerRestaurants/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,email,restaurantName,restaurantPhone,city,location,isAvailableForOccasion,isAvailableForKids,startTime,endTime,isApproved")] ownerRestaurant ownerRestaurant)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ownerRestaurant).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.city = new SelectList(db.cities, "Id", "cityName", ownerRestaurant.city);
            return View(ownerRestaurant);
        }

        // GET: ownerRestaurants/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ownerRestaurant ownerRestaurant = db.ownerRestaurants.Find(id);
            if (ownerRestaurant == null)
            {
                return HttpNotFound();
            }
            return View(ownerRestaurant);
        }

        // POST: ownerRestaurants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ownerRestaurant ownerRestaurant = db.ownerRestaurants.Find(id);
            db.ownerRestaurants.Remove(ownerRestaurant);
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
