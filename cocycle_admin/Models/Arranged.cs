using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace cocycle_admin.Models
{
    public class Arranged
    {

        public int Id { get; set; }
        public int? RouteId { get; set; }
        public string RequestBy { get; set; }
        public string ApprovedBy { get; set; }
        public string StartPoint { get; set; }
       // public string PostCode { get; set; }
        public string RequestingDates { get; set; }
        public string OfferingDates { get; set; }

        //  public string EndPoint { get; set; }

        [DataType(DataType.Date)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime StartTime { get; set; }

        //[DataType(DataType.Date)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        //public DateTime? EndTime { get; set; }
        public string RequestRemark { get; set; }
        public string ApproveRemark { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime ApproveDate { get; set; }

        [ForeignKey("ApprovedBy")]
        public ApplicationUser applicationUserApproved { get; set; }

        [ForeignKey("RequestBy")]
        public ApplicationUser applicationUserRequested { get; set; }

        public int? PostCodeId { get; set; }
        public int StateId { get; set; }
        [Display(Name="City")]
        public int AreaId { get; set; }
        public State State { get; set; }
        public Area Area { get; set; }
        public Boolean IsApproved { get; set; }
        public Boolean IsScheduled { get; set; }

        [ForeignKey("RouteId")]
        public Routes routes { get; set; }
        public ICollection<FeedBack> FeedBack { get; set; }
        public Arranged()
        {
            this.StartTime = DateTime.Now;
            this.ApproveDate = DateTime.Now;
            this.RequestDate = DateTime.Now;

        }
        public Boolean RideCompleted { get; set; }
        [ForeignKey("PostCodeId")]
        public virtual PostCode PostCode { get; set; }

    }

    public class createrequest
    {
        [Display(Name="State")]
        public int StateId { get; set; }
        [Display(Name = "Area")]
        public int AreaId { get; set; }
        [Display(Name = "Novice Users")]
        public string UserId { get; set; }
        [Display(Name = "Request Message")]
        public string RequestRemark { get; set; }
        [Display(Name = "Start Point")]
        public string StartPoint { get; set; }
        public int PostCodeId { get; set; }
        public string OfferingDates { get; set; }

    }
}