//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PZ.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Waiter
    {
        public Waiter()
        {
            this.Order = new HashSet<Order>();
        }
    
        public int ID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Adress { get; set; }
        public string City { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public Nullable<decimal> Reputation { get; set; }
        public Nullable<decimal> Salary { get; set; }
        public string Sex { get; set; }
    
        public virtual ICollection<Order> Order { get; set; }
    }
}
