using BookStore.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public class ApplicationContext: IdentityDbContext<User>
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {        }
        public virtual DbSet<UserAdditionalInfo> UserAdditionalInfo { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Book> Books { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>()
                .HasOne(ui => ui.UserAdditionalInfo)
                .WithOne(u => u.User)
                .HasForeignKey<UserAdditionalInfo>();
            base.OnModelCreating(builder);

            builder.Entity<Book>()
                .HasOne(b=>b.Category)
                .WithMany(c=>c.Books);
        }

    }
}
