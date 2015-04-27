using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PZ.Models
{
    public class MenuBundleViewModel
    {
        public MenuBundleViewModel()
        {
            using (PZEntities db = new PZEntities())
            {
                var query = from menu in db.Menu
                            orderby menu.Category
                            select new MenuViewModel()
                            {
                                ID = menu.ID,
                                Description = menu.Category,
                                Submenus = (from submenu in db.MenuSubcategory
                                            where submenu.MenuID == menu.ID
                                            orderby submenu.Subcategory
                                            select new SubmenuViewModel()
                                            {
                                                ID = submenu.ID,
                                                MenuID = submenu.MenuID,
                                                Description = submenu.Subcategory,
                                                Dishes = (from dish in db.Dish
                                                          where dish.MenuID == submenu.ID
                                                          select new DishViewModel()
                                                          {
                                                              ID = dish.ID,
                                                              Name = dish.NameID,
                                                              Price = 0,
                                                          }
                                                              ).ToList(),
                                            }).ToList(),
                            };
                Menus = query.ToList();
            }
        }

        public List<MenuViewModel> Menus { get; set; }
    }

    public class MenuViewModel
    {
        public int ID { get; set; }
        public string Description { get; set; }
        public List<SubmenuViewModel> Submenus { get; set; }
    }

    public class SubmenuViewModel
    {
        public int ID { get; set; }
        public int MenuID { get; set; }
        public string Description { get; set; }
        public List<DishViewModel> Dishes { get; set; }
    }

    public class DishViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }

}