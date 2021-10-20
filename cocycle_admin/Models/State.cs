using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cocycle_admin.Models
{
    public class State
    {
        public int Id { get; set; }
        public string StateName { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public ICollection<Routes> RoutesList { get; set; }
        public ICollection<Arranged> Arranged { get; set; }
        public State()
        {
            this.Created = DateTime.Now;
        }
    }
    public class StateList
    {
        public int StateId { get; set; }
        public string StateName { get; set; }
    }
}