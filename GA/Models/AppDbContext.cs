
using GA.Helper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GA.Models
{
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base (options)
        {

        }
       
        public DbSet<Students> students { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Box> Box { get; set; }
        public DbSet<StudentLog> StudentLogs { get; set; }
        public DbSet<ItemLog> itemLogs { get; set; }
        public DbSet<BoxLog> boxLogs { get; set; }
        public DbSet<ChangeNotifier> changeNotifier { get; set; }
        public DbSet<Notifications> notifications { get; set; }
        public DbSet<ItemCount> itemCount { get; set; }


        

    }
}
