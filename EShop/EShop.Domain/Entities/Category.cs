using System.ComponentModel.DataAnnotations;

namespace EShop.Domain.Entities
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = null!;

        [Required, MaxLength(100)]
        public string CategoryLogoName { get; set; } = null!;

        public ICollection<CategoryToProduct> CategoryToProducts { get; set; } = null!;
    }
}
