using EShop.Core.Interfaces;
using EShop.Domain.Entities;
using EShop.Domain.Interfaces;

namespace EShop.Core.Services
{
    public class SearchService : ISearchService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        public SearchService(IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
        }

        public List<Product> GetCategoryProducts(int categoryId, ref string categoryName)
        {
            categoryName = _categoryRepository.GetCategoryById(categoryId)?.Name!;
            return _categoryRepository.GetCategoryProducts(categoryId).ToList();
        }

        public List<Product> SearchProducts(string searchTerm, int? categoryId)
        {
            var products = _productRepository.GetProducts().ToList();
            if (categoryId.HasValue)
            {
                var category_products = _categoryRepository.GetCategoryProducts(categoryId.Value).ToList();
                if (category_products.Count == 0)
                {
                    products.Clear();
                    return products;
                }
                products = products.Where(q => category_products.Contains(q)).ToList();
            }


            var search_words = searchTerm.Split(' ');
            var products_match_score = new Dictionary<int, int>();

            foreach (var product in products)
            {
                int match_score = 0;

                foreach (var search_word in search_words)
                {
                    if (product.ProductName.ToLower().Contains(search_word))
                        match_score += 2;
                    if (product.ProductDescription.Contains(search_word))
                        match_score++;
                }

                products_match_score.Add(product.ProductId, match_score);
            }
            products.Clear();
            products_match_score = products_match_score
                .Where(q => q.Value >= 2).OrderByDescending(q => q.Value)
                .ToDictionary(q => q.Key, q => q.Value);
            products.AddRange(products_match_score.Select(q => _productRepository.GetProduct(q.Key)!).ToList());
            return products;
        }
    }
}
