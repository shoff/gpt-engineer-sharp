namespace GptEngineer.Infrastructure.Services;

using Data.Entities;

public interface IReviewService
{
    Task<Review> GetReview();
    Task SaveReview(Review review);
}