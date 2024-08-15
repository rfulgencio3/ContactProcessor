using ContactProcessor.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace ContactProcessor.Infrastructure.Data
{
    public class ContactDbContext : DbContext
    {
        public DbSet<Contact> Contacts { get; set; }

        public ContactDbContext(DbContextOptions<ContactDbContext> options) : base(options)
        {
        }
    }
}
