using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace cocycle_admin.Models
{
    public class Routes
    {
        public int Id { get; set; }
        public int RouteType { get; set; }
        [Display(Name = "Add Image")]
        public string PosterImage { get; set; }
      //  public string PostCode { get; set; }
        [Display (Name ="Departure")]
        public string StartingPoint { get; set; }
        [Display(Name = "Arrival")]
        public string EndPoint { get; set; }

        [DataType(DataType.Time)]
        public DateTime StartTime { get; set; }

        [DataType(DataType.Time)]
        public DateTime EndTime { get; set; }

        [Display(Name = "City")]
        public int AreaId { get; set; }
        [Display(Name = "State")]
        public int StateId { get; set; }
        public int? PostCodeId { get; set; }
        public string Message { get; set; }
        public string CreatedBy { get; set; }
        public DateTime Created { get; set; }
        public string RequestedBy { get; set; }
        public DateTime RequestedDate { get; set; }
        public Boolean IsApproved { get; set; }
        public Boolean Addreturn { get; set; }
      //  public string RouteVideo { get; set; }
        public State State { get; set; }
        public Area Area { get; set; }
        public Boolean IsActive { get; set; }

        [ForeignKey("CreatedBy")]
        public ApplicationUser applicationUserCreated { get; set; }

        [ForeignKey("RequestedBy")]
        public ApplicationUser applicationUserRequested { get; set; }

        public List<RouteSchedule> RouteSchedule { get; set; }
        //  public RouteSchedule RouteSchedules { get; set; }

        //public virtual Area Area { get; set; }
        [Required(ErrorMessage = "please set schedule")]
        [NotMapped]
        public string hidInput { get; set; }
        public ICollection<FeedBack> FeedBack { get; set; }

        [ForeignKey("PostCodeId")]
        public virtual PostCode PostCode { get; set; }
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public Routes()
        {
            this.RequestedDate = DateTime.Now;
            this.Created = DateTime.Now;
        }
    }
}