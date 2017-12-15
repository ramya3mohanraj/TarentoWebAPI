using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class WebApiDbContext : DbContext
    {
        public WebApiDbContext() : base("DemoDbConn")
        {

        }

        public DbSet<User> Users { get; set; }
        
    }
}
