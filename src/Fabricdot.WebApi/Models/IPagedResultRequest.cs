namespace Fabricdot.WebApi.Models;

public interface IPagedResultRequest
{
    /// <summary>
    ///     page index
    /// </summary>
    int Index { get; set; }

    /// <summary>
    ///     page size
    /// </summary>
    int Size { get; set; }
}
