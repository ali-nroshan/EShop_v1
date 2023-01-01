namespace EShop.Core.ViewModels
{
    public class ProductBaseOfferViewModel
    {
        public string OfferTypeName { get; set; } = null!;
        public List<ProductItemViewModel> ProductItems { get; set; } = new List<ProductItemViewModel>();
    }

    public class ProductOfferDataViewModel
    {
        public int ProductId { get; set; }
        public decimal Price { get; set; }
        public decimal TotalSale { get; set; }
        public float Score { get; set; }
    }

    public class CategoryBaseOfferViewModel
    {
        public string OfferTypeName { get; set; } = null!;

        public string OfferTypeCode { get; set; } = null!;

        public List<CategoryBaseOfferItemViewModel> OfferItems { get; set; } = new List<CategoryBaseOfferItemViewModel>();
    }

    public class CategoryBaseOfferItemViewModel
    {


        public int CategoryId { get; set; }

        public string CategoryName { get; set; } = null!;

        public List<ProductItemViewModel> ProductItems { get; set; } = new List<ProductItemViewModel>();
    }

    public enum CategoryBaseOfferOption
    {
        SpecialOffer,
        NewOffer,
        TopScoresOffer
    }

    public enum ProductBaseOfferOption
    {
        RelativeOffer,
        TopSalesOffer
    }
}
