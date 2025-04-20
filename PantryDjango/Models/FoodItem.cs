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
        [Required]
        [StringLength(100)]
        public required string Name { get; set; }

        // optional description of the food item  
        [StringLength(500)]
        public string? Description { get; set; }

        // date when the food will be expired  
        public DateTime ExpirationDate { get; set; }

        // number of items if the food item is more than one  
        [Range(0, int.MaxValue, ErrorMessage = "Quantity must be a positive number.")]
        public int Quantity { get; set; }

        // unit of measurement for the food item (e.g., kg, g, L, mL, etc.)  
        [RegularExpression("kg|g|L|mL|pcs", ErrorMessage = "Unit must be one of the following: kg, g, L, mL, pcs.")]
        public string? Unit { get; set; }

        
        // date when the food item was added to the pantry or fridge  
        public DateTime AddedAt { get; set; } 

        // date when the food item was last updated  
        public DateTime UpdatedAt { get; set; } 
        
        // food category 
        public enum FoodCategory
        {
            Dairy,
            Meat,
            Vegetable,
            Fruit,
            Grain,
            Other
        }

        public FoodCategory Category { get; set; }


        // location of the food item 
        public enum StorageLocation
        {
            Pantry,
            Fridge,
            Freezer,
            Other
        }

        public StorageLocation Location { get; set; }

    }
}
