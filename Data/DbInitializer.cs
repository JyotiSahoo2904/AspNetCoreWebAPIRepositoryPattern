using AspNetCoreWebAPIRepositoryPattern.Models;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreWebAPIRepositoryPattern.Data;

public static class DbInitializer
{
    public static void Initialize(AppDbContext context)
    {
       
        context.Database.EnsureCreated();

       
        if (context.Products.Any())
        {
            return; 
        }

        var products = new Product[]
        {
            new Product { Name = "Laptop", Price = 1200.00M ,Description="Laptop",Version="1"},
            new Product { Name = "Smartphone", Price = 800.00M,Description="Smartphone",Version="1" },
            new Product { Name = "Tablet", Price = 400.00M,Description="Tablet",Version="1" },
            new Product { Name = "Car", Price = 4200.00M ,Description="Car",Version="2"},
            new Product { Name = "Bike", Price = 1800.00M,Description="Bike",Version="2" },
            new Product { Name = "Heavy Machinary", Price = 40000.00M,Description="Heavy Machinary",Version="2" },
        };

        foreach (Product p in products)
        {
            context.Products.Add(p);
        }        

        context.SaveChanges();
    }
}