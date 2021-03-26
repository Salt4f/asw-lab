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
        public DbSet<Contribucio> Contribucions { get; set; }
        public DbSet<Usuari> Usuaris { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            /*modelBuilder.Entity<MultiplePrimaryKeyModel>()
                .HasKey(m => new { m.ID_1, m.ID_2 });*/

            modelBuilder.Entity<Contribucio>()
            .HasOne(c => c.Author)
            .WithMany(a => a.Contribucions);
        }

    }
}
