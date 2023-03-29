using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace box.api.Data.Entities;

public partial class BoxContext : DbContext
{
    public BoxContext()
    {
    }

    public BoxContext(DbContextOptions<BoxContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TFile> TFiles { get; set; }

    public virtual DbSet<TProject> TProjects { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TFile>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__T_File__3213E83F0DBA6CF0");

            entity.ToTable("T_File");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreateTime).HasColumnName("create_time");
            entity.Property(e => e.Filename)
                .HasMaxLength(255)
                .HasColumnName("filename");
            entity.Property(e => e.IsActive)
                .HasDefaultValueSql("((1))")
                .HasColumnName("is_active");
            entity.Property(e => e.ProjectId).HasColumnName("project_id");
            entity.Property(e => e.UpdateTime).HasColumnName("update_time");

            entity.HasOne(d => d.Project).WithMany(p => p.TFiles)
                .HasForeignKey(d => d.ProjectId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_T_File_T_Project1");
        });

        modelBuilder.Entity<TProject>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__T_Projec__3213E83F2C98923D");

            entity.ToTable("T_Project");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
