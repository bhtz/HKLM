using System;
using Microscope.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Microscope.Infrastructure
{
    public class MicroscopeDbContext : DbContext
    {
        public virtual DbSet<Analytic> Analytics { get; set; }
        
        public MicroscopeDbContext()
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {            
            modelBuilder.Entity<Analytic>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Dimension)
                    .IsRequired()
                    .HasColumnType<string>("jsonb");
            });

            modelBuilder.Entity<RemoteConfig>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Dimension)
                    .IsRequired()
                    .HasColumnType<string>("jsonb");
            });
        }
    }
}
