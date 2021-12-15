namespace Fabricdot.Domain.Entities
{
    public interface IHasConcurrencyStamp
    {
        /// <summary>
        ///     concurrency token
        /// </summary>
        string ConcurrencyStamp { get; set; }
    }
}