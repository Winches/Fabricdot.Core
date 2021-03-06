﻿using Ardalis.Specification;

namespace Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Entities
{
    public sealed class AuthorFilterSpecification : Specification<Author>
    {
        public AuthorFilterSpecification(string lastName)
        {
            Query.Where(v => v.LastName == lastName);
        }
    }
}