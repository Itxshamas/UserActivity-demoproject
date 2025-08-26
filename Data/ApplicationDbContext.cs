using Microsoft.EntityFrameworkCore;
using DemoProje.Models;

namespace DemoProje.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Users> Users { get; set; }
        public DbSet<Roles> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //  Users -> Roles (Many-to-One)
            modelBuilder.Entity<Users>()
                .HasOne(u => u.Role)           // ek User ka ek Role hoga
                .WithMany(r => r.Users)        // ek Role me multiple Users ho sakte hain
                .HasForeignKey(u => u.RoleId)  // Foreign Key
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
