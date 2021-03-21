using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HackerNewsASW.Models;
using Microsoft.EntityFrameworkCore;

namespace HackerNewsASW.Data
{
    public class ReportsContext : DbContext
    {
        public ReportsContext(DbContextOptions<ReportsContext> options) : base(options)
        {
        }

        //public DbSet<Model> Models { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            /*modelBuilder.Entity<MultiplePrimaryKeyModel>()
                .HasKey(m => new { m.ID_1, m.ID_2 });*/
        }

    }
}
