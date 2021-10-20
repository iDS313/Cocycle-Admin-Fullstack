using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace cocycle_admin.Models
{
    [NotMapped]
    public class DashboardModel
    {
        public int TotalRoutes { get; set; }
        public int ActiveRoutes { get; set; }
        public int ExpiredRoutes { get; set; }
        public int TotalRides { get; set; }
        public int RidesCompleted { get; set; }
        public int RidesArranged { get; set; }
        public int RidesRequested { get; set; }
        public int TotalUsers { get; set; }
        public int NoviceUser { get; set; }
        public int CyclistUser { get; set; }


    }
}