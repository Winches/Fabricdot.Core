﻿using MediatR;

namespace Fabricdot.Infrastructure.Commands;

public interface ICommand : IRequest
{
    Guid Id { get; }
}

public interface ICommand<out TResult> : IRequest<TResult>
{
    Guid Id { get; }
}