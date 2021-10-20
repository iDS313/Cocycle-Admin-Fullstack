using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace cocycle_admin.Models
{
    public class PostCode
    {

        public int Id { get; set; }
        [Display(Name="City")]
        public int AreaId { get; set; }
        [Display(Name = "State")]
        public int StateId { get; set; }
        [Display(Name = "Post Code")]
        public string PostCodeName { get; set; }
        public DateTime Created { get; set; }
        public State State { get; set; }
        public Area Area { get; set; }

        public PostCode()
        {
            this.Created = DateTime.Now;
        }
    }
}