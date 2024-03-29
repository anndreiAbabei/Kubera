﻿using IdentityServer4.EntityFramework.Options;
using Kubera.Data.Entities;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Kubera.Data.Data
{
    public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>, IApplicationDbContext
    {
        public virtual DbSet<Group> Groups { get; set; }

        public virtual DbSet<Currency> Currencies { get; set; }

        public virtual DbSet<Asset> Assets { get; set; }

        public ApplicationDbContext(DbContextOptions options, IOptions<OperationalStoreOptions> operationalStoreOptions) 
            : base(options, operationalStoreOptions)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>(b =>
            {
                b.Property(u => u.FullName).IsRequired(false);
                b.Property(u => u.Settings).IsRequired(false);
            });

            builder.Entity<Transaction>(b =>
            {
                b.Property(t => t.Amount).HasPrecision(32, 10);
                b.Property(t => t.Rate).HasPrecision(32, 10);
                b.Property(t => t.Fee).HasPrecision(32, 10);
            });
        }
    }
}
