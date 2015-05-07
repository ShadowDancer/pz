using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PZ.Controllers.Api.DTO;

namespace PZ.Controllers
{
    public class WebController : ApiController
    {
        public object Get(string type)
        {
            return Get(type, null);
        }


        // GET api/<controller>/type/id
        public object Get(string type, int? id)
        {
            using (var db = new PZ.Models.PZEntities())
            {
                switch (type)
                {
                    case "Dish":
                        {
                            var query = from d in db.Dish
                                        select new DishDTO
                                        {
                                            id = d.ID,
                                            //name = d.Name,
                                            //price = d.Price,
                                            description = d.Description,
                                            menuid = d.SubcategoryID,
                                        };
                            if (id != null)
                                query = query.Where(n => n.id == id);
                        
                            return new DishBundleDTO() { dishes = query.ToArray() };
                        }
                    case "Menu":
                        {
                            return(object) new PZ.Models.MenuBundleViewModel().Menus;
                        }
                }
            }




            return null;
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}