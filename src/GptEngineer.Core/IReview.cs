namespace GptEngineer.Data.Entities;

public interface IReview
{
    bool Ran { get; set; }
    bool Perfect { get; set; }
    bool Works { get; set; }
    string? Comments { get; set; }
    string? Raw { get; set; }
    DateTime Created { get; set; }
    DateTime? Updated { get; set; }
}