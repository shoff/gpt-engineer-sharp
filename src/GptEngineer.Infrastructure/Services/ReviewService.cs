namespace GptEngineer.Infrastructure.Services;

using Core.Stores;
using Data.Entities;

public class ReviewService : IReviewService
{
    private readonly IReviewStore reviewStore;

    public ReviewService(IReviewStore reviewStore)
    {
        this.reviewStore = reviewStore;
    }

    public Task<Review> GetReview()
    {
        return Task.FromResult(new Review());
    }

    public Task SaveReview(Review review)
    {
        return Task.FromResult(new Review());
    }
}