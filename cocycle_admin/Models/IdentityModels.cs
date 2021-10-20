using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace cocycle_admin.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            this.Created = DateTime.Now;
        }
        public string Name { get; set; }
        public string Address { get; set; }
        [Display (Name="City")]
        public int AreaId { get; set; }
        public int StateId { get; set; }
        public int? PostCodeId { get; set; }
        public Boolean IsAvailable { get; set; }
        public string TravellingDistance { get; set; }
        [NotMapped]
        public string RoleName { get; set; }
        public DateTime Created { get; set; }
        public State State { get; set; }
        public Area Area { get; set; }
        [ForeignKey("PostCodeId")]
        public PostCode PostCode { get; set; }
        public string ImageAddress { get; set; }
        public string Description { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<cocycle_admin.Models.Arranged> Arrangeds { get; set; }

        public System.Data.Entity.DbSet<cocycle_admin.Models.Area> Areas { get; set; }

        public System.Data.Entity.DbSet<cocycle_admin.Models.EmailAccount> EmailAccounts { get; set; }

        public System.Data.Entity.DbSet<cocycle_admin.Models.FeedBack> FeedBacks { get; set; }

        public System.Data.Entity.DbSet<cocycle_admin.Models.RouteGroup> RouteGroups { get; set; }

        public System.Data.Entity.DbSet<cocycle_admin.Models.Routes> Routes { get; set; }

        public System.Data.Entity.DbSet<cocycle_admin.Models.RouteSchedule> RouteSchedules { get; set; }

        public System.Data.Entity.DbSet<cocycle_admin.Models.State> States { get; set; }
        public DbSet<PostCode> postCodes { get; set; }

    }
}