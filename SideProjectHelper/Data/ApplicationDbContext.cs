using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SideProjectHelper.Models;

namespace SideProjectHelper.Data;

public class ApplicationDbContext : IdentityDbContext<User>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    public DbSet<Project> Project { get; set; }
    
    // protected override void OnModelCreating(ModelBuilder modelBuilder)
    // {
    //     modelBuilder.Entity<User>()
    //         .HasNoKey()
    //         .HasMany(e => e.Projects)
    //         .WithOne(e => e.User)
    //         .HasForeignKey("UserId")
    //         .IsRequired();
    // }
}