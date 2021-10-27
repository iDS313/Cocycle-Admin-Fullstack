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
    public class PostCodesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Getpostcode(string searchTerm, int? areaid)
        {
            var p = db.postCodes;
            var a = areaid != null ? p.Where(x => x.AreaId == areaid) : p;
            var postcodes = searchTerm != null ? a.Where(x => x.PostCodeName.Contains(searchTerm) && x.IsActive == true) : a;
            return Json(postcodes.Select(x => new { id = x.Id, text = x.PostCodeName }).ToList(),
                JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetstateandArea(int postcode)
        {
            var postcodes = db.postCodes.Where(x => x.Id == postcode && x.IsActive == true).FirstOrDefault();
            return Json(postcodes, JsonRequestBehavior.AllowGet);
        }

        // GET: PostCodes
        public ActionResult Index()
        {
            var postCodes = db.postCodes.Include(p => p.Area).Include(p => p.State);
            return View(postCodes.Where(x => x.IsActive == true).ToList());
        }

        // GET: PostCodes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PostCode postCode = db.postCodes.Find(id);
            if (postCode == null)
            {
                return HttpNotFound();
            }
            return View(postCode);
        }

        // GET: PostCodes/Create
        public ActionResult Create()
        {
            ViewBag.AreaId = new SelectList(db.Areas, "Id", "AreaName");
            ViewBag.StateId = new SelectList(db.States, "Id", "StateName");
            return View();
        }

        // POST: PostCodes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,AreaId,StateId,PostCodeName,Created")] PostCode postCode)
        {
            if (ModelState.IsValid)
            {
                db.postCodes.Add(postCode);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AreaId = new SelectList(db.Areas, "Id", "AreaName", postCode.AreaId);
            ViewBag.StateId = new SelectList(db.States, "Id", "StateName", postCode.StateId);
            return View(postCode);
        }

        // GET: PostCodes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PostCode postCode = db.postCodes.Find(id);
            if (postCode == null)
            {
                return HttpNotFound();
            }
            ViewBag.AreaId = new SelectList(db.Areas, "Id", "AreaName", postCode.AreaId);
            ViewBag.StateId = new SelectList(db.States, "Id", "StateName", postCode.StateId);
            return View(postCode);
        }

        // POST: PostCodes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,AreaId,StateId,PostCodeName,Created")] PostCode postCode)
        {
            if (ModelState.IsValid)
            {
                db.Entry(postCode).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AreaId = new SelectList(db.Areas.Where(x => x.IsActive == true), "Id", "AreaName", postCode.AreaId);
            ViewBag.StateId = new SelectList(db.States.Where(x => x.IsActive == true), "Id", "StateName", postCode.StateId);
            return View(postCode);
        }

        // GET: PostCodes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PostCode postCode = db.postCodes.Find(id);
            if (postCode == null)
            {
                return HttpNotFound();
            }
            return View(postCode);
        }

        // POST: PostCodes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PostCode postCode = db.postCodes.Find(id);

            postCode.IsActive = false;
            db.Entry(postCode).State = EntityState.Modified;
            // db.postCodes.Remove(postCode);
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
