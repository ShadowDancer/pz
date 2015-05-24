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
    
    public partial class Dish
    {
        public Dish()
        {
            this.SubOrder = new HashSet<SubOrder>();
            this.DishPrices = new HashSet<DishPrices>();
            this.DishRatings = new HashSet<DishRatings>();
            this.ShoppingCart = new HashSet<ShoppingCart>();
        }
    
        public int ID { get; set; }
        public string Description { get; set; }
        public string Comment { get; set; }
        public Nullable<decimal> Rating { get; set; }
        public string NameID { get; set; }
        public string ImageUrl { get; set; }
        public Nullable<int> SubcategoryID { get; set; }
    
        public virtual ICollection<SubOrder> SubOrder { get; set; }
        public virtual ICollection<DishPrices> DishPrices { get; set; }
        public virtual MenuSubcategory MenuSubcategory1 { get; set; }
        public virtual ICollection<DishRatings> DishRatings { get; set; }
        public virtual ICollection<ShoppingCart> ShoppingCart { get; set; }
    }
}