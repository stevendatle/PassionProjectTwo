using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Infrastructure;

namespace PassionProjectTwo.Models
{
    public class MyDbContext : DbContext
    {

        //Instructions to set the models as tables in our DB
        public DbSet<WoWClass> Classes { get; set; }

        public DbSet<WoWComp> Comps { get; set; }

        public MyDbContext() : base("name=MyDBContext")
        {

        }

    }
}