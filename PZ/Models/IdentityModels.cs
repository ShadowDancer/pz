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
                using (var db = new PZEntities())
                {
                    var result = db.User.FirstOrDefault(n => n.Email == UserName);
                    return result;
                }
            }
        }

        public string Address
        {
            get { return PZUser.Adress; }
        }

        public string Email
        {
	        get
	        {
		        return PZUser.Email;
	        }
        }

        public string Age
        {
            get { return PZUser.Age.ToString(); }
        }

        public string Comment
        {
            get { return PZUser.Comment; }
        }

        public string City
        {
            get { return PZUser.City; }
        }

        public string Name
        {
            get { return PZUser.Name; }
        }

        public string Surname
        {
            get { return PZUser.Surname; }
        }

        public string Phone
        {
            get { return PZUser.Phone; }
        }

        public string Sex
        {
            get { return PZUser.Sex; }
        }

        public int ID
        {
            get { return PZUser.ID; }
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