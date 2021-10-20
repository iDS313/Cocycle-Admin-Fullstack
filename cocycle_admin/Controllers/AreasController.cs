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
    public class AreasController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Areas
        public ActionResult Index()
        {
          
            ViewBag.States = db.States.ToList();
            List<Area> areas = new List<Area>();
            var allareas = db.Areas.ToList();
            foreach (var item in allareas)
            {
                Area a = new Area();
                a.Id = item.Id;
                a.AreaName = item.AreaName;
                a.StateId = item.StateId;
                a.StateName = db.States.Where(x => x.Id == item.StateId).Select(x => x.StateName).FirstOrDefault();
                areas.Add(a);
            }
               
            return View(areas);
        }

        // GET: Areas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Area area = db.Areas.Find(id);
            if (area == null)
            {
                return HttpNotFound();
            }
            return View(area);
        }
        // RouteAttribute["State/GetByState/Stateid"]
        public ActionResult GetByState(int? Stateid)
        {
            List<Area> Areas = new List<Area>();
            if (Stateid > 0)
            {
                Areas = db.Areas.Where(x => x.StateId == Stateid).ToList();
            }
            else
            {
                Areas = db.Areas.ToList();
            }

            return Json(Areas, JsonRequestBehavior.AllowGet);

        }
        // GET: Areas/Create
        public ActionResult Create()
        {
            List<State> States = new List<State>();
            States = (from c in db.States select c).ToList();
            ViewBag.States = States;
            return View();
        }

        // POST: Areas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,StateId,AreaName,CreatedBy")] Area area)
        {
            if (ModelState.IsValid)
            {
                var checkexist = db.Areas.Any(x => x.AreaName == area.AreaName);
                if (!checkexist)
                {
                    db.Areas.Add(area);
                    db.SaveChanges();
                    TempData["message"] = "Saved";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["message"] = "All already Exists";
                    return RedirectToAction("Index",area);
                }
            }
            return View(area);
        }

        // GET: Areas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Area area = db.Areas.Find(id);
            if (area == null)
            {
                return HttpNotFound();
            }

            List<State> States = new List<State>();
            States = (from c in db.States select c).ToList();
            ViewBag.States = States;
            return View(area);
        }

        // POST: Areas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,StateId,AreaName,CreatedBy")] Area area)
        {
            if (ModelState.IsValid)
            {
                db.Entry(area).State = EntityState.Modified;
                db.SaveChanges();
                TempData["message"] = "Updated";
                return RedirectToAction("Index");
            }
            return View(area);
        }

        // GET: Areas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Area area = db.Areas.Find(id);
            if (area == null)
            {
                return HttpNotFound();
            }
            return View(area);
        }

        // POST: Areas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Area area = db.Areas.Find(id);
            db.Areas.Remove(area);
            db.SaveChanges();
            TempData["message"] = "Delete";
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
