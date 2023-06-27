namespace GptEngineer.Data.Contexts;

using Entities;
using GptEngineer.Data.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

public class ReviewDbContext : IReviewDbContext
{
    private readonly IMongoDatabase db;
    private readonly ReviewOptions options;

    public ReviewDbContext(IMongoClient client, IOptions<ReviewOptions> options)
    {
        ArgumentNullException.ThrowIfNull(client);
        ArgumentNullException.ThrowIfNull(options);
        if (string.IsNullOrWhiteSpace(options.Value.DatabaseName))
        {
            throw new ArgumentException($"DatabaseName is missing in {nameof(ReviewOptions)}");
        }

        db = client.GetDatabase(options.Value.DatabaseName);
        this.options = options.Value;
    }

    public IMongoCollection<Review> Reviews =>
        db.GetCollection<Review>(options.ReviewCollectionName);
}