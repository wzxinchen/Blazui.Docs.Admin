﻿using BlazAdmin;
using Blazui.Docs.Admin.Repository.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace Blazui.Docs.Admin.EFCore
{
    public class DocsDbContext : IdentityDbContext
    {
        public DocsDbContext()
        {

        }
        public DocsDbContext(DbContextOptions options) : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
            if (optionsBuilder.IsConfigured)
            {
                return;
            }
            optionsBuilder.UseNpgsql("Host=127.0.0.1;Port=5432;Username=postgres;Password=12345678;Database=blazui_docs");
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<RoleResource>()
                .HasKey(x => new { x.RoleId, x.ResourceId });

        }

        public virtual DbSet<RoleResource> RoleResources { get; set; }

        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductVersion> ProductVersions { get; set; }
        public virtual DbSet<VersionChange> VersionChanges { get; set; }
        public virtual DbSet<Repository.Model.Component> Components { get; set; }
        public virtual DbSet<ComponentParameter> ComponentParameters { get; set; }
        public virtual DbSet<ExportedType> ExportedTypes { get; set; }
        public virtual DbSet<ExportedProperty> ExportedProperties { get; set; }
        public virtual DbSet<ExportedTypeGenericParameter> ExportedTypeGenericParameters { get; set; }
        public virtual DbSet<ComponentVersion> ComponentVersions { get; set; }
    }
}
