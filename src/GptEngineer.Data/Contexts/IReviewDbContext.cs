namespace GptEngineer.Data.Contexts;

using Data.Entities;
using MongoDB.Driver;

public interface IReviewDbContext
{
    IMongoCollection<Review> Reviews { get; }

}