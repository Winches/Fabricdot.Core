﻿namespace Fabricdot.WebApi.Models;

public interface IListResult<T>
{
    ICollection<T> List { get; set; }
}