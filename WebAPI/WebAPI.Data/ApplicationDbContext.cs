using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext() : base("DataConnectionStr")
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<ApplicationDbContext>());
        }
        //public DbSet<User> Users { get; set; }
    }
}
