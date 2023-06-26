namespace GptEngineer.Core;

using GptEngineer.Core.Events;
using GptEngineer.Core.Extensions;
using Microsoft.Extensions.Logging;
using OpenAI.Interfaces;
using OpenAI.ObjectModels;
using OpenAI.ObjectModels.RequestModels;

public class AI : IAI
{
    private const string ROLE = "role";
    private const string USER = "user";
    private const string CONTENT = "content";
    private const string ASSISTANT = "assistant";
    private const string SYSTEM = "system";
    private readonly ILogger<AI> logger;
    private readonly IOpenAIService openAIService;

    public AI(
        ILogger<AI> logger,
        IOpenAIService openAIService)
    {
        this.logger = logger;
        this.openAIService = openAIService;
    }

    public event EventHandler<ChatCompletionEventArgs>? CompletionReceived;

    public async Task<IEnumerable<Dictionary<string, string>>> Start(string system, string user)
    {
        ArgumentException.ThrowIfNullOrEmpty(system, nameof(system));
        ArgumentException.ThrowIfNullOrEmpty(user, nameof(user));

        List<Dictionary<string, string>> messages = new()
        {
            new Dictionary<string, string>
            {
                { ROLE, SYSTEM },
                { CONTENT, system }
            },
            new Dictionary<string, string>
            {
                { ROLE, USER },
                { CONTENT, user }
            }
        };

        return await NextAsync(messages);
    }

    public Dictionary<string, string> AsSystemRole(string message)
    {
        ArgumentException.ThrowIfNullOrEmpty(message, nameof(message));
        var result = new Dictionary<string, string>
        {
            { ROLE, SYSTEM },
            { CONTENT, message }
        };

        return result;
    }

    public Dictionary<string, string> AsUserRole(string message)
    {
        ArgumentException.ThrowIfNullOrEmpty(message, nameof(message));
        return new Dictionary<string, string> { { ROLE, USER }, { CONTENT, message } };
    }

    public Dictionary<string, string> AsAssistantRole(string message)
    {
        ArgumentException.ThrowIfNullOrEmpty(message, nameof(message));
        return new Dictionary<string, string> { { ROLE, ASSISTANT }, { CONTENT, message } };
    }

    // TODO validate that this is generating the correct prompt
    public async Task<IEnumerable<Dictionary<string, string>>> NextAsync(
        IEnumerable<Dictionary<string, string>> messages,
        string? prompt = null)
    {
        ArgumentNullException.ThrowIfNull(messages, nameof(messages));

        var messageDictionaryList = (List<Dictionary<string, string>>)messages;
        if (!string.IsNullOrWhiteSpace(prompt))
        {
            messageDictionaryList.Add(new Dictionary<string, string> { { ROLE, USER }, { CONTENT, prompt } });
        }

        this.logger.LogInformation("Creating a new chat completion: {Messages}", messages);

        var gptMessages = messages.Select(m => new GptMessage
        {
            Role = m[ROLE],
            Content = m[CONTENT]
        }).ToList();

        var messageList = gptMessages.Select(
            message => new ChatMessage(message.Role!, message.Content!)).ToList();

        var completionResult = this.openAIService.ChatCompletion.CreateCompletionAsStream(
            new ChatCompletionCreateRequest
            {
                Messages = messageList,
                Model = Models.ChatGpt3_5Turbo,
                MaxTokens = 150 //optional
            });

        List<string> chat = new();

        await foreach (var completion in completionResult)
        {
            if (completion.Successful)
            {
                var chunk = completion.Choices.Select(chunk => chunk.Delta.Content);
                var enumerable = chunk as string[] ?? chunk.ToArray();
                if (enumerable?.Any() != true)
                {
                    return messages ?? new List<Dictionary<string, string>>();
                }
                chat.AddRange(enumerable);
                messageDictionaryList.Add(new Dictionary<string, string>
                    { { ROLE, ASSISTANT }, { CONTENT, string.Join("", chat) } });

                // TODO send to hub via event
                CompletionReceived.Raise(this, new ChatCompletionEventArgs
                {
                    Message = completion.Choices.First().Message.Content
                });
            }
            else
            {
                if (completion.Error == null)
                {
                    throw new Exception("Unknown Error");
                }

                CompletionReceived.Raise(this, new ChatCompletionEventArgs
                {
                    Message = $"{completion.Error.Code}: {completion.Error.Message}"
                });
            }
        }
        return messages;
    }

}