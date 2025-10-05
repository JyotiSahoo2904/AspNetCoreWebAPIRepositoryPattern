using AspNetCoreWebAPIRepositoryPattern.Models;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreWebAPIRepositoryPattern.Data;

public sealed class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Product> Products { get; set; }
   

}