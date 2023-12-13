using Microsoft.EntityFrameworkCore;
using Pustok_project.Models;

namespace Pustok_project.Contexts;
public class PustokDbContext:DbContext
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



}
