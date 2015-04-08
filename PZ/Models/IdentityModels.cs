using Microsoft.AspNet.Identity.EntityFramework;
using System.Linq;

namespace PZ.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class UserViewModel : IdentityUser
    {
        public User PZUser
        {
            get
            {
                using(PZEntities db = new PZEntities())
                {
                    return db.User.FirstOrDefault(n => n.Email == UserName);
                }

            }
        }
    }

    public class ApplicationDbContext : IdentityDbContext<UserViewModel>
    {
        public ApplicationDbContext()
            : base("DefaultConnection")
        {
        }
    }
}