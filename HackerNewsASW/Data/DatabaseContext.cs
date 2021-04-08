using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HackerNewsASW.Models;
using Microsoft.EntityFrameworkCore;

namespace HackerNewsASW.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        //public DbSet<Model> Models { get; set; }
        public DbSet<Contribution> Contributions { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<Ask> Asks { get; set; }
        public DbSet<Comment> Comments { get; set; }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            /*modelBuilder.Entity<MultiplePrimaryKeyModel>()
                .HasKey(m => new { m.ID_1, m.ID_2 });*/

            modelBuilder.Entity<Contribution>()
            .HasOne(c => c.Author)
            .WithMany(a => a.Contributions);

            modelBuilder.Entity<Contribution>()
            .Navigation(c => c.Author)
            .UsePropertyAccessMode(PropertyAccessMode.Property);
        }

    }
}
