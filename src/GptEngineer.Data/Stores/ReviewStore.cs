namespace GptEngineer.Infrastructure.Services;

using Core.Stores;
using Data.Contexts;
using Data.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

public class ReviewStore : IReviewStore
{
    private readonly IReviewDbContext dbContext;

    public ReviewStore(IReviewDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<IEnumerable<IReview>> GetReviewsAsync(int page = 1, int pageSize = 10)
    {
        // If page or pageSize is less than 1, reset them to their default values
        page = page < 1 ? 1 : page;
        pageSize = pageSize < 1 ? 10 : pageSize;

        // Skip the documents for the previous pages and take the documents for the current page
        var reviews = await this.dbContext.Reviews
            .Find(_ => true)
            .Skip((page - 1) * pageSize)
            .Limit(pageSize)
            .ToListAsync();

        return reviews;
    }

    public async Task InsertReviewAsync(IReview review)
    {
        if (review == null)
        {
            throw new ArgumentNullException(nameof(review));
        }

        await this.dbContext.Reviews.InsertOneAsync((Review)review);
    }

    public async Task UpdateReviewAsync(object id, IReview updatedReview)
    {
#pragma warning disable CS8073
        if (id == null)
#pragma warning restore CS8073
        {
            throw new ArgumentNullException(nameof(id));
        }

        if (updatedReview == null)
        {
            throw new ArgumentNullException(nameof(updatedReview));
        }

        var filter = Builders<Review>.Filter.Eq("_id", (ObjectId)id);
        await this.dbContext.Reviews.ReplaceOneAsync(filter, (Review)updatedReview);
    }
}