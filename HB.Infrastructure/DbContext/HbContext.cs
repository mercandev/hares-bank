using System;
using HB.Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace HB.Infrastructure.DbContext
{
    public class HbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public HbContext(DbContextOptions<HbContext> options) : base(options)
        {

        }

        public DbSet<Customers> Customers { get; set; }
        public DbSet<Accounts> Accounts { get; set; }
        public DbSet<Address> Address { get; set; }
        public DbSet<BranchOffices> BranchOffices { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<Documents> Documents { get; set; }
        public DbSet<Organisations> Organisations { get; set; }
    }
}

