using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using cocycle_admin.Models;
using System.Collections.Generic;
using System.Net;
using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace cocycle_admin.Controllers
{
    public class ManageUserController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public ManageUserController()
        {
        }

        public ManageUserController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        // GET: ManageUser
        public ActionResult Index()
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
            List<UserType> UserTypes = new List<UserType>()
                    {
                    new UserType() {UserTypeId = 0, UserTypeText="Cyclist" },
                    new UserType() {UserTypeId = 1, UserTypeText="Novice" }
                    };
            ViewBag.UserTypes = UserTypes;

            List<PostCode> postCodes = new List<PostCode>();
            postCodes = (from c in db.postCodes select c).ToList();
            ViewBag.postCodes = postCodes;

            List<Area> areas = new List<Area>();
            areas = (from c in db.Areas select c).ToList();
            ViewBag.areas = areas;
        }
        public ActionResult AddUser()
        {
            filldropdown();
            return View();
        }

        public ActionResult ChangeRole()
        {
            List<AllUserViewModel> modellist = new List<AllUserViewModel>();
            var userManager = new UserManager<ApplicationUser, string>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            if (userManager.Users.ToList().Count != 0)
            {
                userManager.Users.ToList().ForEach(u =>
                {
                    AllUserViewModel model = new AllUserViewModel();
                    model.User = u;
                    model.Roles = getallRoles(u.Roles.FirstOrDefault().RoleId);
                    modellist.Add(model);
                });
            }
            return View(modellist);
        }
        public ActionResult Register()
        {
            ViewBag.Roles = getallRoles();

            return View();
        }
        public SelectList getallRoles(string selectedRoleId = "1")
        {
            return new SelectList(db.Roles.ToList(), "Id", "Name", selectedRoleId);
        }
        public ActionResult UpdateRole(string UserId, string RoleId)
        {
            var userManager = new UserManager<ApplicationUser, string>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var roleManager = new RoleManager<IdentityRole, string>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
            var olduser = userManager.FindById(UserId);
            var oldrole = roleManager.FindById(olduser.Roles.FirstOrDefault().RoleId);
            var role = roleManager.FindById(RoleId);
            userManager.RemoveFromRole(UserId, oldrole.Name);
            var result = userManager.AddToRole(UserId, role.Name);
            return Json(result.Succeeded, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // ApplicationUser user = db.Users.Find(id);
            ApplicationUser user = UserManager.FindById(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            filldropdown();
            ViewBag.Roles = db.Roles.Where(x => x.Name != "Admin").ToList();

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Exclude = null)] ApplicationUser user)
        {
            if (ModelState.IsValid)
            {
                var SavedUser = await UserManager.FindByIdAsync(user.Id);
                user.SecurityStamp = SavedUser.SecurityStamp;
                user.PasswordHash = SavedUser.PasswordHash;
                user.UserName = SavedUser.UserName;
                user.Id = SavedUser.Id;
                SavedUser.Address = user.Address;
                SavedUser.PhoneNumber = user.PhoneNumber;
                SavedUser.StateId = user.StateId;
                SavedUser.AreaId = user.AreaId;
                SavedUser.IsAvailable = user.IsAvailable;
                //SavedUser.TravellingDistance = user.TravellingDistance;
                SavedUser.PostCodeId = user.PostCodeId;
                var result = await UserManager.UpdateAsync(SavedUser);
                return RedirectToAction("ViewAll");
            }

            filldropdown();
            return View(User);
        }
        public ActionResult ViewAll()   
        {
            // Get the list of Users in this Role
            var users = new List<ApplicationUser>();
            // Get the list of Users in this Role
            foreach (var user in UserManager.Users.ToList())
            {
                ApplicationUser us = db.Users.Where(u => u.UserName.Equals(user.UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                var userStore = new UserStore<ApplicationUser>(db);
                var userManager = new UserManager<ApplicationUser>(userStore);
                var rolename = userManager.GetRoles(user.Id).FirstOrDefault();
                user.RoleName = rolename;
                users.Add(user);
            }

            ViewBag.Users = users;
            ViewBag.UserCount = users.Count();
            var allusers = db.Users.Include(x=>x.Roles).ToList();
           
            return View(users);
        }
       


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddUser(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    Name = model.Name,
                    Address = model.Address,
                    AreaId = model.AreaId,
                    StateId = model.StateId,
                    IsAvailable = true,
                   // TravellingDistance = model.TravellingDistance,
                    PhoneNumber = model.PhoneNumber,
                    PostCodeId= model.PostCodeId
                };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    string[] usertypearr = { "cyclist", "learner" };
                    IdentityResult roleresult = await UserManager.AddToRoleAsync(user.Id, usertypearr[Convert.ToInt32(model.UserType)]);
                    // await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");
                    TempData["message"] = "Saved";
                    return RedirectToAction("ViewAll", "ManageUser");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: /Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            if (ModelState.IsValid)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                try
                {
                    var user = await UserManager.FindByIdAsync(id);
                    var logins = user.Logins;
                    var rolesForUser = await UserManager.GetRolesAsync(id);
                    using (var transaction = db.Database.BeginTransaction())
                    {
                        foreach (var login in logins.ToList())
                        {
                            await UserManager.RemoveLoginAsync(login.UserId, new UserLoginInfo(login.LoginProvider, login.ProviderKey));
                        }
                        if (rolesForUser.Count() > 0)
                        {
                            foreach (var item in rolesForUser.ToList())
                            {
                                // item should be the name of the role
                                var result = await UserManager.RemoveFromRoleAsync(user.Id, item);
                            }
                        }
                        await UserManager.DeleteAsync(user);
                        transaction.Commit();
                    }
                }
                catch (Exception ee)
                {
                    TempData["message"] = "Delete_cascade";
                    Response.Write(ee.Message);
                    //throw;
                }
                return RedirectToAction("ViewAll");
            }
            else
            {
                return View();
            }
        }

        public ActionResult ResetPassword(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ResetPasswordViewModelCustom resetPasswordViewModelCustom = new ResetPasswordViewModelCustom();
            resetPasswordViewModelCustom.UserId = user.Id;
            return View(resetPasswordViewModelCustom);
        }


        [HttpPost, ActionName("ResetPassword")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPasswordConfirmed(ResetPasswordViewModelCustom model)
        {
            if (model.UserId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            try
            {
                var user = await UserManager.FindByIdAsync(model.UserId);
                string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                var result = await UserManager.ResetPasswordAsync(user.Id, code, model.Password);
                if (result.Succeeded)
                {
                    TempData["message"] = "Password_reset";
                    return RedirectToAction("ViewAll");
                }
            }
            catch (Exception ee)
            {
               // throw;
            }
            return RedirectToAction("ViewAll");
        }

        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.States = db.States.ToList();
            ViewBag.Areas = db.Areas.ToList();
            ViewBag.Users = db.Users.ToList();
            ViewBag.postCodes = db.postCodes.ToList();

            ApplicationUser us = db.Users.Where(u => u.UserName.Equals(user.UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            var userStore = new UserStore<ApplicationUser>(db);
            var userManager = new UserManager<ApplicationUser>(userStore);
            var rolename = userManager.GetRoles(user.Id).FirstOrDefault();
            user.RoleName = rolename;

            return View(user);
        }


        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}