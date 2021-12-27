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
    public class mealsController : Controller
    {
        private ravenousDBEntities db = new ravenousDBEntities();

        // GET: meals
        public ActionResult Index()
        {

            var meals = db.meals.Include(m => m.mealCategory).Include(m => m.ownerRestaurant);
            return View(meals.ToList());
        }

        // GET: meals/Details/5
        public ActionResult Details(int? restaurantId, int? Id)
        {
            if (restaurantId == null || Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            meal meal = db.meals.Find(restaurantId,Id);
            if (meal == null)
            {
                return HttpNotFound();
            }
            return View(meal);
        }

        // GET: meals/Create
        public ActionResult Create()
        {
            ViewBag.category = new SelectList(db.mealCategories, "Id", "type");
            ViewBag.restaurantId = new SelectList(db.ownerRestaurants, "Id", "restaurantName");
            return View();
        }

        // POST: meals/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "restaurantId,Id,mealName,mealPrice,category,available")] meal meal)
        {
            if (ModelState.IsValid)
            {
                db.meals.Add(meal);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.category = new SelectList(db.mealCategories, "Id", "type", meal.category);
            ViewBag.restaurantId = new SelectList(db.ownerRestaurants, "Id", "restaurantName", meal.restaurantId);
            return View(meal);
        }




        // GET: meals/Edit/5
        public ActionResult Edit(int? restaurantId,int? Id)
        {
            if (restaurantId == null ||Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            meal meal = db.meals.Find(restaurantId,Id);
            if (meal == null)
            {
                return HttpNotFound();
            }
            ViewBag.category = new SelectList(db.mealCategories, "Id", "type", meal.category);
            ViewBag.restaurantId = new SelectList(db.ownerRestaurants, "Id", "restaurantName", meal.restaurantId);
            return View(meal);
        }



        // POST: meals/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "restaurantId,Id,mealName,mealPrice,category,available")] meal meal)
        {
            if (ModelState.IsValid)
            {
                db.Entry(meal).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.category = new SelectList(db.mealCategories, "Id", "type", meal.category);
            ViewBag.restaurantId = new SelectList(db.ownerRestaurants, "Id", "restaurantName", meal.restaurantId);
            return View(meal);
        }

        // GET: meals/Delete/5
        public ActionResult Delete(int? restaurantId, int? Id)
        {
            if (restaurantId == null || Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            meal meal = db.meals.Find(restaurantId,Id);
            if (meal == null)
            {
                return HttpNotFound();
            }
            return View(meal);
        }

        // POST: meals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int restaurantId, int Id)
        {
            meal meal = db.meals.Find(restaurantId,Id);
            db.meals.Remove(meal);
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
