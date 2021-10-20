using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace cocycle_admin.Models
{
    public class Area
    {
        [Display(Name = "AreaId")]
        public int Id { get; set; }
        [Display(Name="State")]
        public int StateId { get; set; }
        [Display(Name = "City Name")]
        public string AreaName { get; set; }
        //public System.DateTime Created { get; set; }
        public int CreatedBy { get; set; }
        // public virtual State State{ get; set; }
        public ICollection<Routes> RoutesList { get; set; }
        public ICollection<Arranged> Arranged { get; set; }
        [NotMapped]
        public State state { get; set; }
        [NotMapped]
        public string StateName { get; set; }

        

    }
}