using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ravenous.Models;
using System.IO;
using System.Data.Entity;
using System.Net;

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
        public ActionResult CreateRestaurant([Bind(Include = "Id,email,restaurantName,restaurantPhone,city,location,isAvailableForOccasion,isAvailableForKids,startTime,endTime,image,isApproved")] ownerRestaurant ownerRestaurant, HttpPostedFileBase imgFile)
        {
            if (ModelState.IsValid)
            {
                ownerRestaurant.isApproved = false;
                
                string path = "";
                if(imgFile.FileName.Length > 0)
                {
                    path = "~/Content/images/"+ Path.GetFileName(imgFile.FileName);
                    imgFile.SaveAs(Server.MapPath(path));
                }

                ownerRestaurant.image = path;

                db.ownerRestaurants.Add(ownerRestaurant);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.city = new SelectList(db.cities, "Id", "cityName", ownerRestaurant.city);


            SelectListItem[] items = creatList();
            SelectListItem[] items2 = creatList();
            items[ownerRestaurant.startTime].Selected = true;
            items2[ownerRestaurant.endTime -1].Selected = true;
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
        // GET: All Restaurants
        public ActionResult AllRestaurants()
        {
            List<ownerRestaurant> ownerRestaurants = db.ownerRestaurants.Where(o => o.isApproved == true).ToList();
            return View(ownerRestaurants);
        }

        // GET: Admin Approve
        public ActionResult AdminApprove()
        {
            List<ownerRestaurant> ownerRestaurants = db.ownerRestaurants.OrderBy(o => o.isApproved).ToList();
            return View(ownerRestaurants);
        }

        // POST: Admin Approve
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdminApprove(int? Id, string approve, string disApprove)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ownerRestaurant ownerRestaurant = db.ownerRestaurants.Find(Id);
            if (ownerRestaurant == null)
            {
                return HttpNotFound();
            }
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(approve))
                {
                    ownerRestaurant.isApproved = true;
                }

                else if (!string.IsNullOrEmpty(disApprove))
                {
                    ownerRestaurant.isApproved = false;
                }

                db.Entry(ownerRestaurant).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("AdminApprove");
            }
            ViewBag.city = new SelectList(db.cities, "Id", "cityName", ownerRestaurant.city);
            return RedirectToAction("AdminApprove");
        }







        // GET: meals
        public ActionResult UserMealsMenu(int? restaurantId)
        {
            if (restaurantId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var meals = db.meals.Include(m => m.mealCategory)
                                .Include(m => m.ownerRestaurant)
                                .Where(a => a.restaurantId == restaurantId && a.available == true)
                                .OrderBy(a => a.mealCategory.type);
            return View(meals.ToList());
        }






        // GET: Restaurant Page
        public ActionResult RestaurantPage()
        {
            Session["restaurantId"] = 3;
            return View();
        }






        // GET: meals
        public ActionResult OwnerRestaurantMealsMenu()
        {
            int restaurantId = int.Parse(Session["restaurantId"].ToString());
            

            var meals = db.meals.Include(m => m.mealCategory)
                                .Include(m => m.ownerRestaurant)
                                .Where(a => a.restaurantId == restaurantId).OrderBy(a=>a.mealCategory.type);

            //int x = 5;
            return View(meals.ToList());
        }





        // GET: meals/CreateMeal
        public ActionResult CreateMeal()
        {
            ViewBag.category = new SelectList(db.mealCategories, "Id", "type");

            int restaurantId = int.Parse(Session["restaurantId"].ToString());
            ViewBag.restaurantId = db.ownerRestaurants.Find(restaurantId).restaurantName;
            return View();
        }



        // POST: meals/CreateMeal
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateMeal([Bind(Include = "mealName,mealPrice,category,available")] meal meal, HttpPostedFileBase []imgFiles)
        {
            if (ModelState.IsValid)
            {
                meal.restaurantId = int.Parse(Session["restaurantId"].ToString());

                db.meals.Add(meal);

                db.SaveChanges();

                int mealId = db.meals.Where(a => a.restaurantId == meal.restaurantId
                                                && a.mealName == meal.mealName).FirstOrDefault().Id;

                foreach (var imgFile in imgFiles)
                {
                    meal_images image = new meal_images();
                    image.restaurantId = meal.restaurantId;
                    image.mealId = mealId;

                    string path = "";
                    if (imgFile.FileName.Length > 0)
                    {
                        path = "~/Content/images/" + Path.GetFileName(imgFile.FileName);
                        imgFile.SaveAs(Server.MapPath(path));
                    }

                    image.mealImage = path;
                    meal.meal_images.Add(image);
                }

                db.SaveChanges();

                return RedirectToAction("OwnerRestaurantMealsMenu");
            }

            ViewBag.category = new SelectList(db.mealCategories, "Id", "type", meal.category);
            int restaurantId = int.Parse(Session["restaurantId"].ToString());
            ViewBag.restaurantId = db.ownerRestaurants.Find(restaurantId).restaurantName;
            return View(meal);
        }







        // GET: meals/EditMeal/ restaurantId , Id
        public ActionResult EditMeal(int? restaurantId, int? Id)
        {
            if (restaurantId == null || Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            meal meal = db.meals.Find(restaurantId, Id);
            if (meal == null)
            {
                return HttpNotFound();
            }
            ViewBag.category = new SelectList(db.mealCategories, "Id", "type", meal.category);
            ViewBag.restaurantId = new SelectList(db.ownerRestaurants, "Id", "restaurantName", meal.restaurantId);
            return View(meal);
        }





        // POST: meals/EditMeal/ restaurantId , Id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditMeal([Bind(Include = "restaurantId,Id,mealName,mealPrice,category,available")] meal meal)
        {
            if (ModelState.IsValid)
            {
                db.Entry(meal).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("OwnerRestaurantMealsMenu");
            }
            ViewBag.category = new SelectList(db.mealCategories, "Id", "type", meal.category);
            ViewBag.restaurantId = new SelectList(db.ownerRestaurants, "Id", "restaurantName", meal.restaurantId);
            return View(meal);
        }





    }
}