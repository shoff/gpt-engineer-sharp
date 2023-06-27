namespace GptEngineer.Core.Stores;

using Data.Entities;

public interface IReviewStore
{
    Task<IEnumerable<IReview>> GetReviewsAsync(int page = 1, int pageSize = 10);
    Task InsertReviewAsync(IReview review);
    Task UpdateReviewAsync(object id, IReview updatedReview);
}