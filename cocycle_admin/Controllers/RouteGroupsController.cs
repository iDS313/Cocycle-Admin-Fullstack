using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using cocycle_admin.Models;

namespace cocycle_admin.Controllers
{
    public class RouteGroupsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: RouteGroups
        public ActionResult Index()
        {
            ViewBag.Users = db.Users.ToList();
            ViewBag.Routes = db.Routes.ToList();
            ViewBag.postCodes = db.postCodes.ToList();
            return View(db.RouteGroups.Include(x=>x.routes).ToList());
        }
        public ActionResult ViewRequest()
        {
            ViewBag.Users = db.Users.ToList();
            ViewBag.Routes = db.Routes.ToList();
            ViewBag.postCodes = db.postCodes.ToList();
            return View(db.RouteGroups.Where(x=>x.IsApproved==false).Include(x => x.routes).ToList());
        }
        public ActionResult ViewRequestbyRoute()
        {
            ViewBag.Users = db.Users.ToList();
            ViewBag.Routes = db.Routes.ToList();
            var routes = db.Routes.ToList();
            ViewBag.postCodes = db.postCodes.ToList();
            return View(routes);
        }

        public ActionResult ViewMembers(int? routeid)
        {
            if (routeid == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var routeGroup = db.RouteGroups.Where(x=>x.RouteId == routeid);
            if (routeGroup == null)
            {
                // return HttpNotFound();
                TempData["message"] = "Members Not Found";
                return RedirectToAction("ViewRequestbyRoute");
            }

            ViewBag.Users = db.Users.ToList();
            ViewBag.Routes = db.Routes.ToList();
            ViewBag.postCodes = db.postCodes.ToList();
            var routeGroups = db.RouteGroups.Where(x=>x.RouteId==routeid).ToList();
            if (routeGroups.Count>0)
            {
                return View(routeGroups);
            }
            else
            {
                TempData["message"] = "Members Not Found";
                return RedirectToAction("ViewRequestbyRoute");
              
            }
            //return View(routeGroups);
        }

        // GET: RouteGroups/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RouteGroup routeGroup = db.RouteGroups.Find(id);
            if (routeGroup == null)
            {
                return HttpNotFound();
            }

            ViewBag.Users = db.Users.ToList();
            ViewBag.Routes = db.Routes.ToList();
            ViewBag.postCodes = db.postCodes.ToList();
            return View(routeGroup);
        }
        public ActionResult ViewRoute(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RouteGroup routeGroup = db.RouteGroups.Find(id);
            if (routeGroup == null)
            {
                return HttpNotFound();
            }
            var routeid = routeGroup.RouteId;
            return RedirectToAction("Details", "Routes",new { id=routeid});
          //  return View(routeGroup);
        }


        public ActionResult Approve(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RouteGroup routeGroup = db.RouteGroups.Where(x => x.Id == id).Include(x=>x.routes).FirstOrDefault();
            if (routeGroup == null)
            {
                return HttpNotFound();
            }
            routeGroup.IsApproved = true;
            routeGroup.ApproveBy = routeGroup.routes.CreatedBy;
            routeGroup.ApproveDate = DateTime.Now;
            db.SaveChanges();
            TempData["message"] = "Request Approved";
            return RedirectToAction("Index");
        }

        // GET: RouteGroups/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RouteGroups/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,RouteId,RequestBy,ApproveBy,RequestDate,ApproveDate,IsApproved")] RouteGroup routeGroup)
        {
            if (ModelState.IsValid)
            {
                db.RouteGroups.Add(routeGroup);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(routeGroup);
        }

        // GET: RouteGroups/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RouteGroup routeGroup = db.RouteGroups.Find(id);
            if (routeGroup == null)
            {
                return HttpNotFound();
            }
            return View(routeGroup);
        }

        // POST: RouteGroups/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,RouteId,RequestBy,ApproveBy,RequestDate,ApproveDate,IsApproved")] RouteGroup routeGroup)
        {
            if (ModelState.IsValid)
            {
                db.Entry(routeGroup).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(routeGroup);
        }

        // GET: RouteGroups/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RouteGroup routeGroup = db.RouteGroups.Find(id);
            if (routeGroup == null)
            {
                return HttpNotFound();
            }
            return View(routeGroup);
        }

        // POST: RouteGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RouteGroup routeGroup = db.RouteGroups.Find(id);
            db.RouteGroups.Remove(routeGroup);
            db.SaveChanges();
            TempData["message"] = "Deleted";
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
