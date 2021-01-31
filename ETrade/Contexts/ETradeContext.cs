using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Web;
using ETrade.Models;

namespace ETrade.Contexts
{
    public class ETradeContext:DbContext
    {
        public ETradeContext():base("ETrade")
        {

        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }

    }
}