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
    
    public partial class Reservation_List
    {
        public int ID { get; set; }
        public System.DateTime From { get; set; }
        public System.DateTime To { get; set; }
        public Nullable<int> UserID { get; set; }
        public Nullable<int> TableID { get; set; }
    
        public virtual Table Table { get; set; }
        public virtual User User { get; set; }
    }
}
