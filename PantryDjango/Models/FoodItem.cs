using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace PantryDjango.Models
{
    /// <summary>  
    /// Represents a food item in the pantry or fridge.  
    /// </summary>  
    public class FoodItem
    {
        // unique identifier for the food item  
        [Key]
        public int Id { get; set; }

        // name of the food item  
        public required string Name { get; set; }

        // optional description of the food item  
        public string? Description { get; set; }

        // date when the food will be expired  
        public DateTime ExpirationDate { get; set; }

        // number of items if the food item is more than one  
        public int Quantity { get; set; }

        // unit of measurement for the food item (e.g., kg, g, L, mL, etc.)  
        public string? Unit { get; set; }

        // category of the food item (e.g., Dairy, Meat, Vegetable, etc.)  
        public required string Category { get; set; }

        // date when the food item was added to the pantry or fridge  
        public DateOnly AddedAt { get; set; } = DateOnly.FromDateTime(DateTime.Now);

        // date when the food item was last updated  
        public DateOnly UpdatedAt { get; set; } = DateOnly.FromDateTime(DateTime.Now);

        // location where the food item is stored (e.g., pantry, fridge, freezer, etc.)  
        public string? location { get; set; }
    }
}
