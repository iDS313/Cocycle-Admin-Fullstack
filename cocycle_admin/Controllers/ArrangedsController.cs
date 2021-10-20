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
    public class ArrangedsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Arrangeds
        public ActionResult Index()
        {
            ViewBag.States = db.States.ToList();
            ViewBag.Areas = db.Areas.ToList();
            ViewBag.Users = db.Users.ToList();
            return View(db.Arrangeds.ToList());
        }
       
        public ActionResult ViewScheduled()
        {
            ViewBag.States = db.States.ToList();
            ViewBag.Areas = db.Areas.ToList();
            ViewBag.Users = db.Users.ToList();
            return View("Index",db.Arrangeds.Where(x => x.IsScheduled == true).ToList());
        }
        public ActionResult ViewCompleted()
        {
            ViewBag.States = db.States.ToList();
            ViewBag.Areas = db.Areas.ToList();
            ViewBag.Users = db.Users.ToList();
            return View("Index",db.Arrangeds.Where(x => x.RideCompleted == true).ToList());
        }
        // GET: Arrangeds/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Arranged arranged = db.Arrangeds.Find(id);
            Arranged arranged_details = db.Arrangeds.Include(x => x.FeedBack).Where(x=>x.Id==id).FirstOrDefault();
            if (arranged == null)
            {
                return HttpNotFound();
            }
            ViewBag.States = db.States.ToList();
            ViewBag.Areas = db.Areas.ToList();
            ViewBag.Users = db.Users.ToList();
            return View(arranged_details);
        }

        // GET: Arrangeds/Create
        public ActionResult Create()
        {
            return View();
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

            var users = from u in db.Users
                        where u.Roles.Any(r => r.RoleId == "2")
                        select u;
            ViewBag.Users = users;

            List<PostCode> postCodes = new List<PostCode>();
            postCodes = (from c in db.postCodes select c).ToList();
            ViewBag.postCodes = postCodes;

            List<Area> areas = new List<Area>();
            areas = (from c in db.Areas select c).ToList();
            ViewBag.areas = areas;
        }
        public ActionResult RequestRide()
        {

            filldropdown();
            return View();
        }

        [HttpPost]
        public ActionResult RequestRide(createrequest createrequest)
        {

            if (ModelState.IsValid)
            {
                Arranged ar = new Arranged();
                ar.RequestBy = createrequest.UserId;
                ar.AreaId = createrequest.AreaId;
                ar.OfferingDates = createrequest.OfferingDates;
                ar.StartPoint = createrequest.StartPoint;
                ar.StateId = createrequest.StateId;
                ar.PostCodeId = createrequest.PostCodeId;
                ar.RequestRemark = createrequest.RequestRemark;
                db.Arrangeds.Add(ar);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(createrequest);
        }


        // POST: Arrangeds/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,RouteId,RequestBy,ApprovedBy,StartPoint,PostCode,RequestingDates,OfferingDates,StartTime,RequestRemark,ApproveRemark,RequestDate,ApproveDate,StateId,AreaId,IsApproved,IsScheduled,RideCompleted")] Arranged arranged)
        {
            if (ModelState.IsValid)
            {
                db.Arrangeds.Add(arranged);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(arranged);
        }

        // GET: Arrangeds/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Arranged arranged = db.Arrangeds.Find(id);
            if (arranged == null)
            {
                return HttpNotFound();
            }
            return View(arranged);
        }

        // POST: Arrangeds/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,RouteId,RequestBy,ApprovedBy,StartPoint,PostCode,RequestingDates,OfferingDates,StartTime,RequestRemark,ApproveRemark,RequestDate,ApproveDate,StateId,AreaId,IsApproved,IsScheduled,RideCompleted")] Arranged arranged)
        {
            if (ModelState.IsValid)
            {
                db.Entry(arranged).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(arranged);
        }

        // GET: Arrangeds/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Arranged arranged = db.Arrangeds.Find(id);
            if (arranged == null)
            {
                return HttpNotFound();
            }
            return View(arranged);
        }

        // POST: Arrangeds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Arranged arranged = db.Arrangeds.Find(id);
            db.Arrangeds.Remove(arranged);
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
