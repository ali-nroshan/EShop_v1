namespace EShop.Domain.Entities
{
    public class CategoryToProduct
    {
        public int CategoryId { get; set; }
        public int ProductId { get; set; }


        public Category Category { get; set; } = null!;
        public Product Product { get; set; } = null!;
    }
}
