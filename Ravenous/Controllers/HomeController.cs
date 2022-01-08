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
        private  ravenousDBEntities db = new ravenousDBEntities();
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
        public ActionResult Logout()
        {
            Session.Abandon();

            return RedirectToAction("Index");
        }





        // GET: Create Restaurant
        public ActionResult CreateRestaurant()
        {
            ViewBag.city = new SelectList(db.cities, "Id", "cityName");
            return View();
        }


        // POST: Create Restaurant
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateRestaurant([Bind(Include = "Id,email,restaurantName,restaurantPhone,city,location,isAvailableForOccasion,isAvailableForKids,startTime,endTime,isApproved")] ownerRestaurant ownerRestaurant, HttpPostedFileBase imgFile)
        {
            if (ModelState.IsValid)
            {
                if (imgFile == null)
                {
                    ModelState.AddModelError("image", "* صـورة الـمـطـعـم مطلوبة");
                    
                    ViewBag.city = new SelectList(db.cities, "Id", "cityName", ownerRestaurant.city);

                    return View(ownerRestaurant);
                }

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



            return View(ownerRestaurant);
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
                                .Where(a => a.restaurantId == restaurantId && a.available == true);
            return View(meals.ToList());
        }






        // GET: Restaurant Page
        public ActionResult RestaurantPage()
        {
            Session["restaurantId"] = 3;
            Session["email"] = db.ownerRestaurants.Find(3).email;
            Session["restaurantName"] = db.ownerRestaurants.Find(3).restaurantName;
            Session["restaurantImage"] = db.ownerRestaurants.Find(3).image;
            return View();
        }


        // GET: Restaurant Details
        public ActionResult RestaurantDetails()
        {
            int restaurantId = int.Parse(Session["restaurantId"].ToString());
            ownerRestaurant restaurant = db.ownerRestaurants.Find(restaurantId);

            if (restaurant == null)
            {
                return HttpNotFound();
            }

            return View(restaurant);
        }



        // GET: Edit Restaurant ? restaurantId
        public ActionResult EditRestaurant()
        {
            int id = int.Parse(Session["restaurantId"].ToString());

            ownerRestaurant ownerRestaurant = db.ownerRestaurants.Find(id);
            if (ownerRestaurant == null)
            {
                return HttpNotFound();
            }

            ViewBag.city = new SelectList(db.cities, "Id", "cityName", ownerRestaurant.city);
            Session["image"] = ownerRestaurant.image;
            return View(ownerRestaurant);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditRestaurant([Bind(Include = "email,restaurantName,restaurantPhone,city,location,isAvailableForOccasion,isAvailableForKids,startTime,endTime")] ownerRestaurant ownerRestaurant, HttpPostedFileBase imgFile)
        {
            if (imgFile == null)
                ownerRestaurant.image = Session["image"].ToString();
            else
            {
                string path = "";
                if (imgFile.FileName.Length > 0)
                {
                    path = "~/Content/images/" + Path.GetFileName(imgFile.FileName);
                    imgFile.SaveAs(Server.MapPath(path));
                }

                ownerRestaurant.image = path;
            }

            ownerRestaurant.Id = int.Parse(Session["restaurantId"].ToString());
            ownerRestaurant.email = Session["email"].ToString();
            ownerRestaurant.isApproved = true;

            if (ModelState.IsValid)
            {
                db.Entry(ownerRestaurant).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("RestaurantDetails");
            }
            ViewBag.city = new SelectList(db.cities, "Id", "cityName", ownerRestaurant.city);
            return View(ownerRestaurant);
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
        public ActionResult CreateMeal([Bind(Include = "mealName,mealPrice,category,available")] meal meal, HttpPostedFileBase imgFile)
        {
            int restaurantId;

            if (ModelState.IsValid)
            {
                if ( imgFile == null)
                {
                    ModelState.AddModelError("mealImage", "* صـورة الـوجـبـة مـطـلـوبـة");
                    ViewBag.category = new SelectList(db.mealCategories, "Id", "type", meal.category);
                    restaurantId = int.Parse(Session["restaurantId"].ToString());
                    ViewBag.restaurantId = db.ownerRestaurants.Find(restaurantId).restaurantName;
                   
                    return View(meal);
                }

                meal.restaurantId = int.Parse(Session["restaurantId"].ToString());

                string path = "";
                if (imgFile.FileName.Length > 0)
                {
                    path = "~/Content/images/" + Path.GetFileName(imgFile.FileName);
                    imgFile.SaveAs(Server.MapPath(path));
                }

                meal.mealImage = path;

                db.meals.Add(meal);

                db.SaveChanges();

                return RedirectToAction("OwnerRestaurantMealsMenu");
            }

            ViewBag.category = new SelectList(db.mealCategories, "Id", "type", meal.category);
            restaurantId = int.Parse(Session["restaurantId"].ToString());
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

            Session["restaurantId"] = meal.restaurantId;
            Session["Id"] = meal.Id;
            Session["mealImage"] = meal.mealImage;


            return View(meal);
        }





        // POST: meals/EditMeal/ restaurantId , Id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditMeal([Bind(Include = "mealName,mealPrice,category,available")] meal meal, HttpPostedFileBase imgFile)
        {
            meal.restaurantId = int.Parse( Session["restaurantId"].ToString());
            meal.Id = int.Parse(Session["Id"].ToString());
            if(imgFile == null)
                meal.mealImage = Session["mealImage"].ToString();
            else
            {
                string path = "";
                if (imgFile.FileName.Length > 0)
                {
                    path = "~/Content/images/" + Path.GetFileName(imgFile.FileName);
                    imgFile.SaveAs(Server.MapPath(path));
                }

                meal.mealImage = path;
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(meal).State = EntityState.Modified;
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                return RedirectToAction("OwnerRestaurantMealsMenu");
            }
            ViewBag.category = new SelectList(db.mealCategories, "Id", "type", meal.category);
            ViewBag.restaurantId = new SelectList(db.ownerRestaurants, "Id", "restaurantName", meal.restaurantId);
            return View(meal);
        }



    }
}
