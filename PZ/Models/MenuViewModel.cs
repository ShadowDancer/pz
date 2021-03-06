﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Configuration;
using System.Runtime.Serialization.Diagnostics;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Json;

namespace PZ.Models
{
    public class MenuBundleViewModel
    {

        public MenuBundleViewModel()
        {
            using (var db = new PZEntities())
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
                                                          where dish.SubcategoryID == submenu.ID
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
				
				foreach (var menu in Menus)
				{
					foreach (var suborder in menu.Submenus)
					{
						foreach(var dish in suborder.Dishes)
						{
							dish.Price = db.Dish.Where(n => n.ID == dish.ID).Single().GetPrice();
						}
					}

				}
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