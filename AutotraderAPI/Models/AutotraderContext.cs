using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AutotraderAPI.Models;

public partial class AutotraderContext : DbContext
{
    public AutotraderContext()
    {
    }

    public AutotraderContext(DbContextOptions<AutotraderContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Car> Cars { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Car>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("cars");

            entity.Property(e => e.Brand)
                .HasMaxLength(30)
                .HasDefaultValueSql("'NULL'");
            entity.Property(e => e.Color)
                .HasMaxLength(10)
                .HasDefaultValueSql("'NULL'");
            entity.Property(e => e.CreatedTime)
                .HasDefaultValueSql("'current_timestamp()'")
                .HasColumnType("datetime");
            entity.Property(e => e.Myear)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("date")
                .HasColumnName("MYear");
            entity.Property(e => e.Type)
                .HasMaxLength(30)
                .HasDefaultValueSql("'NULL'");
            entity.Property(e => e.UpdatedTime)
                .HasDefaultValueSql("'current_timestamp()'")
                .HasColumnType("datetime");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
