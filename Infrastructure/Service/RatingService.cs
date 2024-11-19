using Core.Entities;
using Core.Interfaces;

namespace Infrastructure.Service;

public class RatingService : IRatingService
{
    private readonly IRepository<Product> _productRepository;
    private readonly IRepository<Review> _reviewRepository;

    public RatingService(IRepository<Product> productRepository, IRepository<Review> reviewRepository)
    {
        _reviewRepository = reviewRepository;
        _productRepository = productRepository;
    }

    public async Task UpdateAverageRating(int productId)
    {
        var product = await _productRepository.GetbyIDAsync(productId);
        

        if (product != null)
        {
            var reviews = await _reviewRepository.ListAsync(x => x.ProductID == productId);

            if (reviews.Any())
            {
                product.AverageRating = reviews.Average(x => x.Rating);
            }
            else
            {
                product.AverageRating = 0;
            }
            
            await _productRepository.EditAsync(product);
        }
    }
}