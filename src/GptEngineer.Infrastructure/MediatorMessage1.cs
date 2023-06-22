namespace GptEngineer.Infrastructure;

public class MediatorMessage<TData> : MediatorMessage
{
    private TData? data;

    public MediatorMessage()
    {
        this.Data = default(TData);
        this.StatusCode = StatusCodes.EntityNotFound;
        this.Successful = true;
    }

    public MediatorMessage(TData data)
    {
        this.Data = data;
        if ((object)data == null)
        {
            this.Successful = true;
            this.StatusCode = StatusCodes.EntityNotFound;
        }
        else
        {
            this.StatusCode = StatusCodes.Complete;
        }
    }

    public MediatorMessage(Exception exception)
        : base(exception)
    {
    }

    public TData? Data
    {
        get => this.data;
        set
        {
            if (value == null)
            {
                return;
            }

            this.StatusCode = StatusCodes.Complete;
            this.data = value;
        }
    }
}