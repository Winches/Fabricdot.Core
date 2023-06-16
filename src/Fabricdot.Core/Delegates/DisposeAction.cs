namespace Fabricdot.Core.Delegates;

/// <summary>
///     This class can be used to provide an action when
///     Dispose method is called.
/// </summary>
public class DisposeAction : IDisposable
{
    private readonly Action _action;
    private bool _disposedValue;

    /// <summary>
    ///     Creates a new <see cref="DisposeAction" /> object.
    /// </summary>
    /// <param name="action">Action to be executed when this object is disposed.</param>
    public DisposeAction(Action action)
    {
        _action = action ?? throw new ArgumentNullException(nameof(action));
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            _action();
            _disposedValue = true;
        }
    }
}
