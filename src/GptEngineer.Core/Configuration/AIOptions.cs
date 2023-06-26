namespace GptEngineer.Core.Configuration;

using FluentValidation;

public class AIOptions : IValidatable
{
    private static readonly Validator validator = new();

    public string? ProjectPath { get; set; }
    public string Model { get; set; } = "gpt-4";
    public bool DeleteExisting { get; set; }
    public double Temperature { get; set; } = 0.1;
    public string StepsConfig { get; set; } = "default";
    public bool Verbose { get; set; }
    public string RunPrefix { get; set; } = "";
    public void Validate()
    {
        validator.ValidateAndThrow(this);
    }

    private class Validator : AbstractValidator<AIOptions>
    {
        public Validator()
        {
            RuleFor(x => x.ProjectPath)
                .NotEmpty()
                .WithMessage("ProjectPath must not be null, empty string or whitespace")
                .Must(x => x != null && x.EndsWith("\\"))
                .WithMessage("ProjectPath must end with '\\'");

            RuleFor(x => x.Model)
                .NotEmpty()
                .WithMessage("Model must not be null, empty string or whitespace");

            // For bool property like DeleteExisting, you can create rule if
            // you want it to be true or false by default
            // RuleFor(x => x.DeleteExisting).Equal(true)
            // .WithMessage("DeleteExisting must be true");

            RuleFor(x => x.Temperature)
                .InclusiveBetween(0.0, 1.0)
                .WithMessage("Temperature must be between 0.0 and 1.0");

            RuleFor(x => x.StepsConfig)
                .NotEmpty()
                .WithMessage("StepsConfig must not be null, empty string or whitespace");
            
            RuleFor(x => x.RunPrefix)
                .Length(0, 100)
                .WithMessage("RunPrefix must be between 0 and 100 characters long");
        }
    }
}