using cocycle_admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace cocycle_admin.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        [Authorize]
        public ActionResult Index()
        {

            DashboardModel dashboardModel = new DashboardModel();
            dashboardModel.TotalUsers = db.Users.Count();
            dashboardModel.RidesArranged = db.Arrangeds.Where(x => x.IsApproved == true).Count();
            dashboardModel.RidesCompleted =db.Arrangeds.Where(x => x.RideCompleted == true).Count();
            dashboardModel.RidesRequested = db.Arrangeds.Where(x=>x.IsApproved==false).Count();
            dashboardModel.TotalRides= db.Arrangeds.Count();
            dashboardModel.TotalRoutes = db.Routes.Count();
            dashboardModel.ActiveRoutes = db.Routes.Count();
            dashboardModel.NoviceUser = db.Users.Where(x => x.Roles.Any(r => r.RoleId == "2")).Count();
            dashboardModel.CyclistUser = db.Users.Where(x => x.Roles.Any(r => r.RoleId == "1")).Count();
            return View(dashboardModel);
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
    }
}