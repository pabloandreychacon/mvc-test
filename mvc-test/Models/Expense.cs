using System.ComponentModel.DataAnnotations;

namespace mvc_test.Models
{
    public class Expense
    {
        public int Id { get; set; }
        public decimal Price { get; set; }

        [Required]
        public string? Description { get; set; }
    }
}
