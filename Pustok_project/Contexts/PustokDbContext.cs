using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Pustok_project.Models;

namespace Pustok_project.Contexts;
public class PustokDbContext:IdentityDbContext
{
    public PustokDbContext(DbContextOptions opt) : base(opt) { }
    public DbSet<Slider> Sliders { get; set; }
    public DbSet<Category> Category { get; set; }
    public DbSet<Product> Product { get; set; }
    public DbSet<ProductImage> ProductImages { get; set; }
    public DbSet<Author> Author { get; set; }
    public DbSet<Blog> Blog { get; set; }
    public DbSet<Tag> Tag { get; set; }
    public DbSet<TagBlog> TagBlog { get; set; }
    public DbSet<TagProduct> TagProduct { get; set; }
    public DbSet<Setting> Settings { get; set; }
    public DbSet<AppUser> AppUsers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Setting>()
            .HasData(new Setting
            {
                Id = 1,
                Address = "Baku, Yasamal, Isfendiyar Zulalov 16",
                Phone = "+994707094535",
                Email = "haha@gmail.com",
            });
        base.OnModelCreating(modelBuilder);
    }


}
