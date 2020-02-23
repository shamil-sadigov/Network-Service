using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IPWebService.Persistence
{
    public class ApplicationContext : DbContext
    {
        public DbSet<GeoObject> GetoObjects { get; set; }


        public ApplicationContext(DbContextOptions<ApplicationContext> ops) : base(ops)
        {

        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Id configuration

            modelBuilder.Entity<GeoObject>()
                        .HasKey(x => x.Id);

            modelBuilder.Entity<GeoObject>()
                        .Property(x => x.Id)
                        .ValueGeneratedOnAdd();

            #endregion

            #region IPAddress configuration

            modelBuilder.Entity<GeoObject>()
                        .HasIndex(x => x.IPAddress)
                        .IsUnique();

            modelBuilder.Entity<GeoObject>()
                        .Property(x => x.IPAddress)
                        .IsRequired();

            #endregion

            #region Point configuration

            modelBuilder.Entity<GeoObject>()
                         .Property(x => x.Point)
                         .IsRequired(false);

            #endregion

            #region City configuration

            modelBuilder.Entity<GeoObject>()
                        .Property(x => x.City)
                        .HasMaxLength(200);

            modelBuilder.Entity<GeoObject>()
                        .Property(x => x.City)
                        .IsRequired(false);

            #endregion

            #region Country configuration

            modelBuilder.Entity<GeoObject>()
                        .Property(x => x.Country)
                        .HasMaxLength(200);

            modelBuilder.Entity<GeoObject>()
                        .Property(x => x.Country)
                        .IsRequired(false);

            #endregion

            #region TimeZone configuration

            modelBuilder.Entity<GeoObject>()
                        .Property(x => x.TimeZone)
                        .IsRequired(false);

            modelBuilder.Entity<GeoObject>()
                        .Property(x => x.TimeZone)
                        .HasMaxLength(200);

            #endregion
        }
    }
}
