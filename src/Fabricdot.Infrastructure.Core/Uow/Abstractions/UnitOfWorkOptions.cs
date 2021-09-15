using System.Data;

namespace Fabricdot.Infrastructure.Core.Uow.Abstractions
{
    public class UnitOfWorkOptions
    {
        /// <summary>
        ///     Default value is false.
        /// </summary>
        public bool IsTransactional { get; set; }

        /// <summary>
        ///     Default value is ReadCommitted.
        /// </summary>
        public IsolationLevel IsolationLevel { get; set; } = IsolationLevel.ReadCommitted;

        public UnitOfWorkOptions Clone() => (UnitOfWorkOptions)MemberwiseClone();
    }
}