namespace GptEngineer.Infrastructure.Steps;

using Data.Stores;

public abstract class Step 
{
    protected readonly IPrePromptStore prePromptStore;

    protected Step(IPrePromptStore prePromptStore)
    {
        this.prePromptStore = prePromptStore;
    }

    protected string SetupSysPrompt()
    {
        return $"{this.prePromptStore[GENERATE]}\nUseful to know:\n{this.prePromptStore[PHILOSOPHY]}";
    }
}