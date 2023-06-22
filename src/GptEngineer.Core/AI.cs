﻿namespace GptEngineer;
using System.Diagnostics;
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

    public Dictionary<string, string> FSystem(string msg)
    {
        var result = new Dictionary<string, string>
        {
            { ROLE, SYSTEM },
            { CONTENT, msg }
        };

        return result;
    }

    public Dictionary<string, string> FUser(string msg)
    {
        return new Dictionary<string, string> { { ROLE, USER }, { CONTENT, msg } };
    }

    public Dictionary<string, string> FAssistant(string msg)
    {
        return new Dictionary<string, string> { { ROLE, ASSISTANT }, { CONTENT, msg } };
    }

    public async Task<List<Dictionary<string, string>>> Next(List<Dictionary<string, string>> messages, string? prompt = null)
    {
        if (!string.IsNullOrWhiteSpace(prompt))
        {
            messages.Add(new Dictionary<string, string> { { ROLE, USER }, { CONTENT, prompt } });
        }

        this.logger.LogInformation("Creating a new chat completion: {Messages}", messages);

        var x = messages.Select(m => new Message
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
            //Messages = new List<ChatMessage>
            //{
            //    ChatMessage.FromSystem(x.FirstOrDefault(f=>f.Role == SYSTEM)?.Content),
            //    ChatMessage.FromUser(x.FirstOrDefault(f=>f.Role == USER)?.Content),
            //    ChatMessage.FromAssistant(x.FirstOrDefault(f=>f.Role == ASSISTANT)?.Content),
            //    ChatMessage.FromUser(x.FirstOrDefault(f=>f.Role == USER)?.Content)
            //},
            Model = Models.Gpt_4,
            // MaxTokens = 50//optional
        });

        //var completionResult = Program.AIService.Completions.CreateCompletionAsStream(new CompletionCreateRequest()
        //{
        //    Prompt = "Once upon a time",
        //    MaxTokens = 2500
        //}, Models.Gpt_4);

        //await foreach (var completion in completionResult)
        //{
        //    if (completion.Successful)
        //    {
        //        Console.Write(completion.Choices.FirstOrDefault()?.Text);
        //    }
        //    else
        //    {
        //        if (completion.Error == null)
        //        {
        //            throw new Exception("Unknown Error");
        //        }

        //        Console.WriteLine($"{completion.Error.Code}: {completion.Error.Message}");
        //    }
        //}
        //Console.WriteLine("Complete");


        if (completionResult.Successful)
        {
            var x1 = completionResult.Choices.First().Message.Content;
        }

        List<string> chat = new();
        foreach (var chunk in completionResult.Choices)
        {
            // var delta = chunk[0]["delta"];// chunk["choices"][0]["delta"];
            var msg = chunk.Delta.Content ?? "";
            // TODO do something with this?
            Console.Write(msg);
            chat.Add(msg);
        }
        messages.Add(new Dictionary<string, string> { { ROLE, ASSISTANT }, { CONTENT, string.Join("", chat) } });
        this.logger.LogInformation("Chat completion finished: {Messages}", messages);
        return messages;
    }

    public async Task<List<Dictionary<string, string>>> NextAsync(List<Dictionary<string, string>> messages, string? prompt = null)
    {
        if (!string.IsNullOrWhiteSpace(prompt))
        {
            messages.Add(new Dictionary<string, string> { { ROLE, USER }, { CONTENT, prompt } });
        }

        this.logger.LogInformation("Creating a new chat completion: {Messages}", messages);

        var x = messages.Select(m => new Message
        {
            Role = m[ROLE],
            Content = m[CONTENT]
        }).ToList();

        var messageList = new List<ChatMessage>();

        foreach (var message in x)
        {
            messageList.Add(new ChatMessage(message.Role!, message.Content!));
        }

        var completionResult = this.openAIService.ChatCompletion.CreateCompletionAsStream(
            new ChatCompletionCreateRequest
            {
                Messages = messageList,
                //Messages = new List<ChatMessage>
                //{
                //    new(StaticValues.ChatMessageRoles.System, "You are a helpful assistant."),
                //    new(StaticValues.ChatMessageRoles.User, "Who won the world series in 2020?"),
                //    new(StaticValues.ChatMessageRoles.System, "The Los Angeles Dodgers won the World Series in 2020."),
                //    new(StaticValues.ChatMessageRoles.User, "Tell me a story about The Los Angeles Dodgers")
                //},
                Model = Models.ChatGpt3_5Turbo,
                MaxTokens = 150//optional
            });
        
        List<string> chat = new();

        await foreach (var completion in completionResult)
        {

            if (completion.Successful)
            {
                foreach (var chunk in completion.Choices)
                {
                    // var delta = chunk[0]["delta"];// chunk["choices"][0]["delta"];
                    var msg = chunk.Delta.Content ?? "";
                    // Console.Write(msg);
                    chat.Add(msg);
                }
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