﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Fabricdot.Domain.Core.Auditing;

namespace Fabricdot.Infrastructure.Core.Domain.Events
{
    public interface IDomainEventsDispatcher
    {
        /// <summary>
        ///     Dispatch domain events from changes.
        /// </summary>
        /// <param name="changeInfos"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task DispatchEventsAsync(
            ICollection<EntityChangeInfo> changeInfos,
            CancellationToken cancellationToken = default);
    }
}