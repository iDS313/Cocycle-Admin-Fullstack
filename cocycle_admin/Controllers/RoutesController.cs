using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using cocycle_admin.Models; 
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;

namespace cocycle_admin.Controllers
{
    public class RoutesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Routes
        public ActionResult Index()
        {
            ViewBag.States = db.States.ToList();
            ViewBag.Areas = db.Areas.ToList();
            ViewBag.Users = db.Users.ToList();
            ViewBag.postCodes = db.postCodes.ToList();
            return View(db.Routes.ToList());
        }
        public ActionResult ViewRequested()
        {
            ViewBag.States = db.States.ToList();
            ViewBag.Areas = db.Areas.ToList();
            ViewBag.Users = db.Users.ToList();
            ViewBag.postCodes = db.postCodes.ToList();
            return View(db.Routes.Where(x => x.IsApproved == false).ToList());
        }

        // GET: Routes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Routes routes = db.Routes.Find(id);
            if (routes == null)
            {
                return HttpNotFound();
            }
            ViewBag.States = db.States.ToList();
            ViewBag.Areas = db.Areas.ToList();
            ViewBag.Users = db.Users.ToList();
            ViewBag.postCodes = db.postCodes.ToList();
            return View(routes);
        }

        public void filldropdown()
        {
            List<StateList> liststate = new List<StateList>();
            var lststate = db.States.ToList();
            foreach (var s in lststate)
            {
                StateList stateList = new StateList
                {
                    StateId = Convert.ToInt32(s.Id),
                    StateName = s.StateName
                };
                liststate.Add(stateList);
            }
            ViewBag.States = liststate;
            List<UserType> UserTypes = new List<UserType>()
                    {
                    new UserType() {UserTypeId = 0, UserTypeText="Cyclist" },
                    new UserType() {UserTypeId = 1, UserTypeText="Novice" }
                    };
            ViewBag.UserTypes = UserTypes;

            List<Area> areas = new List<Area>();
            areas = (from c in db.Areas select c).ToList();
            ViewBag.areas = areas;

            List<PostCode> postCodes = new List<PostCode>();
            postCodes = (from c in db.postCodes select c).ToList();
            ViewBag.postCodes = postCodes;
        }
        // GET: Routes/Create
        public ActionResult Create()
        {

            filldropdown();

            return View();
        }

        // POST: Routes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,RouteType,PosterImage,StartingPoint,EndPoint,StartTime,EndTime,AreaId,StateId,Message,CreatedBy,Created,RequestedBy,RequestedDate,IsApproved,Addreturn,hidInput,PostCodeId,Description")] Routes routes)
        {
            HttpPostedFileBase file = Request.Files["PosterImage"];
            //if (file.FileName != "")
            //{
            //    var fileName = Path.GetFileName(file.FileName);
            //    var guid = Guid.NewGuid().ToString();
            //    var path = Path.Combine(Server.MapPath("~/Content/Images"), guid + fileName);
            //    file.SaveAs(path);
            //    routes.PosterImage = "../Content/Images/" + guid + fileName;
            //}
            var currentUser = User.Identity.GetUserId();
                routes.CreatedBy = currentUser;
                routes.Created = DateTime.Now;
                routes.IsApproved = true;
                routes.IsActive = true;
            if (ModelState.IsValid)
                {
                    db.Routes.Add(routes);
                    db.SaveChanges();
                    var schedule = routes.hidInput;
                    var schedules = schedule.Replace(@"\", string.Empty);
                    var data = JsonConvert.DeserializeObject<List<RouteSchedule>>(schedule);
                    foreach (var obj in data)
                    {
                        obj.RouteId = routes.Id;
                    }
                    foreach (var obj in data)
                    {
                        db.RouteSchedules.Add(obj);
                    }
                    db.SaveChanges();
                TempData["message"] = "Saved";
                return RedirectToAction("Index");
                }

            filldropdown();

            return View(routes);
           
        }


        // GET: Routes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Routes routes = db.Routes.Include(x => x.RouteSchedule).Where(x=>x.Id==id).FirstOrDefault();

            ViewBag.schedulelist = JsonConvert.SerializeObject(routes.RouteSchedule.ToList());
            
            if (routes == null)
            {
                return HttpNotFound();
            }

            filldropdown();
            return View(routes);
        }

        // POST: Routes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,RouteType,PosterImage,StartingPoint,EndPoint,StartTime,EndTime,AreaId,StateId,Message,CreatedBy,Created,RequestedBy,RequestedDate,IsApproved,Addreturn,hidInput,PostCodeId,Description")] Routes routes)
        {
            if (ModelState.IsValid)
            {
                var currentUser = User.Identity.GetUserId();
                routes.CreatedBy = currentUser;
                routes.Created = DateTime.Now;
                db.Entry(routes).State = EntityState.Modified;
                db.SaveChanges();
                TempData["message"] = "Updated";
                return RedirectToAction("Index");
            }
            filldropdown();
            //ViewBag.schedulelist = JsonConvert.SerializeObject(routes.RouteSchedule.ToList());
            return View(routes);
        }

        // GET: Routes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Routes routes = db.Routes.Find(id);
            if (routes == null)
            {
                return HttpNotFound();
            }
            ViewBag.States = db.States.ToList();
            ViewBag.Areas = db.Areas.ToList();
            ViewBag.Users = db.Users.ToList();
            ViewBag.postCodes = db.postCodes.ToList();
            return View(routes);
        }

        // GET: Routes/Delete/5
        public ActionResult ApproveRequested(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Routes routes = db.Routes.Find(id);
            if (routes == null)
            {
                return HttpNotFound();
            }
            routes.IsApproved = true;
            routes.hidInput = "route";
            var currentUser = User.Identity.GetUserId();
            routes.CreatedBy = currentUser;
            db.SaveChanges();
            TempData["message"] = "Route Approved";
            return RedirectToAction("Index");
        }

        // POST: Routes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Routes routes = db.Routes.Find(id);
                db.Routes.Remove(routes);
                db.SaveChanges();
                TempData["message"] = "Delete";
            }
            catch (Exception e)
            {
                TempData["message"] = "Delete_cascade";
                Response.Write(e.Message);
               // throw;
            }
            
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
