namespace GptEngineer.Core;

using GptEngineer.Core.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenAI.Interfaces;
using OpenAI.ObjectModels;
using OpenAI.ObjectModels.RequestModels;

public class AI : IAI
{
    private readonly IOpenAIService openAIService;
    private readonly AIOptions options;
    private const string ROLE = "role";
    private const string USER = "user";
    private const string CONTENT = "content";
    private const string ASSISTANT = "assistant";
    private const string SYSTEM = "system";
    private readonly ILogger<AI> logger;

    public AI(
        ILogger<AI> logger,
        IOptions<AIOptions> options,
        IOpenAIService openAIService)
    {
        this.logger = logger;
        this.openAIService = openAIService;
        this.options = options.Value;
    }

    public async Task<List<Dictionary<string, string>>> Start(string system, string user)
    {
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

        return await this.NextAsync(messages);
    }

    public Dictionary<string, string> AsSystemRole(string msg)
    {
        var result = new Dictionary<string, string>
        {
            { ROLE, SYSTEM },
            { CONTENT, msg }
        };

        return result;
    }

    public Dictionary<string, string> AsUserRole(string msg)
    {
        return new Dictionary<string, string> { { ROLE, USER }, { CONTENT, msg } };
    }

    public Dictionary<string, string> AsAssistantRole(string msg)
    {
        return new Dictionary<string, string> { { ROLE, ASSISTANT }, { CONTENT, msg } };
    }

    // Use the NextAsync method below instead of this one for now
    public async Task<List<Dictionary<string, string>>> Next(List<Dictionary<string, string>> messages, string? prompt = null)
    {
        if (!string.IsNullOrWhiteSpace(prompt))
        {
            messages.Add(new Dictionary<string, string> { { ROLE, USER }, { CONTENT, prompt } });
        }

        this.logger.LogInformation("Creating a new chat completion: {Messages}", messages);

        var x = messages.Select(m => new GptMessage
        {
            Role = m[ROLE],
            Content = m[CONTENT]
        }).ToList();

        var messageList = new List<ChatMessage>();

        foreach (var message in x)
        {
            messageList.Add(new ChatMessage(message.Role, message.Content));
        }
        
        var completionResult = await this.openAIService.ChatCompletion
            .CreateCompletion(new ChatCompletionCreateRequest
        {
            // convert the messages to a list of ChatMessage objects
            Messages = messageList,
            Model = Models.Gpt_4,
        });

        if (completionResult.Successful)
        {
            var x1 = completionResult.Choices.First().Message.Content;
        }

        List<string> chat = new();
        foreach (var chunk in completionResult.Choices)
        {
            var msg = chunk.Delta.Content ?? "";
            // TODO do something with this?
            Console.Write(msg);
            chat.Add(msg);
        }
        messages.Add(new Dictionary<string, string> { { ROLE, ASSISTANT }, { CONTENT, string.Join("", chat) } });
        this.logger.LogInformation("Chat completion finished: {Messages}", messages);
        return messages;
    }

    // TODO validate that this is generating the correct prompt
    public async Task<List<Dictionary<string, string>>> NextAsync(List<Dictionary<string, string>> messages, string? prompt = null)
    {
        if (!string.IsNullOrWhiteSpace(prompt))
        {
            messages.Add(new Dictionary<string, string> { { ROLE, USER }, { CONTENT, prompt } });
        }

        this.logger.LogInformation("Creating a new chat completion: {Messages}", messages);

        var x = messages.Select(m => new GptMessage
        {
            Role = m[ROLE],
            Content = m[CONTENT]
        }).ToList();

        var messageList = x.Select(message => new ChatMessage(message.Role!, message.Content!)).ToList();
        
        var completionResult = this.openAIService.ChatCompletion.CreateCompletionAsStream(
            new ChatCompletionCreateRequest
            {
                Messages = messageList,
                Model = Models.ChatGpt3_5Turbo,
                MaxTokens = 150//optional
            });
        
        List<string> chat = new();

        await foreach (var completion in completionResult)
        {

            if (completion.Successful)
            {
                chat.AddRange(completion.Choices.Select(chunk => chunk.Delta.Content ?? ""));
                messages.Add(new Dictionary<string, string> { { ROLE, ASSISTANT }, { CONTENT, string.Join("", chat) } });
                
                // TODO send to hub
                Console.Write(completion.Choices.First().Message.Content);
            }
            else
            {
                if (completion.Error == null)
                {
                    throw new Exception("Unknown Error");
                }

                Console.WriteLine($"{completion.Error.Code}: {completion.Error.Message}");
            }
        }
        
        // TODO send to HUB
        chat.ForEach(c=>Console.WriteLine());

        return messages;
    }
}