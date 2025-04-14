using Microsoft.EntityFrameworkCore;
using PantryDjango.Models;

namespace PantryDjango.Data
{
    public class FoodItemContext : DbContext 
    { 
        public FoodItemContext(DbContextOptions<FoodItemContext> options) : base(options) 
        { 
        }

        public DbSet<FoodItem> FoodItems { get; set; } 

    }
}
