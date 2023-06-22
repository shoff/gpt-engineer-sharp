using System.ComponentModel;

namespace GptEngineer.Infrastructure;

using FluentValidation;

public class MediatorMessage
{

    public List<Exception> Errors { get; } = new List<Exception>();

    public MediatorMessage()
    {
        this.Successful = true;
        this.StatusCode = StatusCodes.Complete;
    }

    public MediatorMessage(Exception exception)
    {
        this.StatusCode = exception is ValidationException ? StatusCodes.InvalidRequest : StatusCodes.ServerError;
        this.Successful = false;
        this.AddException(exception);
    }
    
    public void AddException(Exception? exception)
    {
        if (exception == null)
        {
            return;
        }

        this.Errors.Add(exception);
        if (this.StatusCode < (StatusCodes)300)
        {
            this.StatusCode = StatusCodes.ServerError;
        }
    }
    
    public StatusCodes StatusCode { get; set; }

    public bool Successful { get; set; }

    public Type? HandlerType { get; set; }
    
    public enum StatusCodes
    {
        [Description] None = 0,
        [Description("Complete")] Complete = 200, // 0x000000C8
        [Description("EntityCreated")] EntityCreated = 201, // 0x000000C9
        [Description("Accepted")] Accepted = 202, // 0x000000CA
        [Description("EntityNotFound")] EntityNotFound = 204, // 0x000000CC
        [Description("NotModified")] NotModified = 304, // 0x00000130
        [Description("InvalidRequest")] InvalidRequest = 400, // 0x00000190
        [Description("Unauthorized")] Unauthorized = 401, // 0x00000191
        [Description("Forbidden")] Forbidden = 403, // 0x00000193
        [Description("NotFound")] NotFound = 404, // 0x00000194
        [Description("ServerError")] ServerError = 500, // 0x000001F4
        [Description("NotImplemented")] NotImplemented = 501, // 0x000001F5
        [Description("ServiceUnavailable")] ServiceUnavailable = 503, // 0x000001F7
    }
}