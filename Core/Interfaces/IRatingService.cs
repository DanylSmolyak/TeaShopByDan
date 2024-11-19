namespace Core.Interfaces;

public interface IRatingService
{
    Task UpdateAverageRating(int productId);
}