using System.ComponentModel.DataAnnotations;
using Ardalis.GuardClauses;

namespace Fabricdot.Infrastructure.Queries;

public abstract class PageQueryBase<TResult> : Query<TResult>
{
    /// <summary>
    ///     page number
    /// </summary>
    [Required]
    [Range(1, int.MaxValue)]
    public int Index { get; set; }

    /// <summary>
    ///     page size
    /// </summary>
    [Required]
    [Range(1, 1000)]
    public int Size { get; set; }

    protected PageQueryBase(int index, int size)
    {
        Guard.Against.OutOfRange(index, nameof(index), 1, int.MaxValue);
        Guard.Against.OutOfRange(size, nameof(size), 1, 1000);

        Index = index;
        Size = size;
    }

    protected PageQueryBase()
    {
    }

    public virtual int GetOffset() => Size * (Index - 1);
}
