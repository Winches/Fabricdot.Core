namespace Fabricdot.Domain.Core.Entities
{
    public interface IHasConcurrencyStamp
    {
        /// <summary>
        ///     concurrency token
        /// </summary>
        string ConcurrencyStamp { get; set; }
    }
}