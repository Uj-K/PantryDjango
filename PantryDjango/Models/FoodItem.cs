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
        
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public DateTime ExpirationDate { get; set; }
        public int Quantity { get; set; }
        public string Unit { get; set; } // e.g., "kg", "g", "L", "mL", etc.
        public string Category { get; set; } // e.g., "Dairy", "Meat", "Vegetable", etc.
        public bool IsExpired { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
