namespace GptEngineer.Core.Extensions;

public static class EventExtensions
{
    public static void Raise(this EventHandler? handler, object sender, EventArgs args)
    {
        if (handler == null)
        {
            // just return
            return;
        }
        ArgumentNullException.ThrowIfNull(sender, nameof(sender));
        ArgumentNullException.ThrowIfNull(args, nameof(args));
        handler.Invoke(sender, args);
    }

    public static void Raise<T>(this EventHandler<T>? handler, object sender, T args)
        where T : EventArgs?
    {
        if (handler == null)
        {
            // just return
            return;
        }

        ArgumentNullException.ThrowIfNull(sender, nameof(sender));
        ArgumentNullException.ThrowIfNull(args, nameof(args));
        handler.Invoke(sender, args);
    }
}