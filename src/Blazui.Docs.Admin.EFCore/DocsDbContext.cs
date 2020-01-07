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
            if (optionsBuilder.IsConfigured)
            {
                return;
            }
            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.UseNpgsql("Host=127.0.0.1;Port=5432;Username=postgres;Password=12345678;Database=blazui_docs");
        }
    }
}
