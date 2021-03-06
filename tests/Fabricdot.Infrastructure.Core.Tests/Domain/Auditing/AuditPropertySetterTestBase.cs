﻿using System;
using System.Collections.Generic;
using Fabricdot.Domain.Core.Entities;
using Fabricdot.Domain.Core.SharedKernel;
using Fabricdot.Infrastructure.Core.Domain.Auditing;
using Fabricdot.Infrastructure.Core.Security;
using Fabricdot.Test.Shared;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace Fabricdot.Infrastructure.Core.Tests.Domain.Auditing
{
    public abstract class AuditPropertySetterTestBase : IntegrationTestBase
    {
        public class FakeAuditObject : FullAuditEntity<int>
        {
            public FakeAuditObject()
            {
            }

            public FakeAuditObject(
                DateTime creationTime,
                string creatorId,
                DateTime modificationTime,
                string modifierId,
                DateTime deletionTime,
                string deleterId,
                bool isDeleted)
            {
                CreationTime = creationTime;
                CreatorId = creatorId;
                LastModificationTime = modificationTime;
                LastModifierId = modifierId;
                DeletionTime = deletionTime;
                DeleterId = deleterId;
                IsDeleted = isDeleted;
            }
        }

        protected readonly ICurrentUser CurrentUser;
        protected readonly IAuditPropertySetter AuditPropertySetter;

        protected AuditPropertySetterTestBase()
        {
            AuditPropertySetter = ServiceProvider.GetRequiredService<IAuditPropertySetter>();
            CurrentUser = ServiceProvider.GetRequiredService<ICurrentUser>();
        }

        /// <inheritdoc />
        protected override void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IAuditPropertySetter, AuditPropertySetter>();
            var mock = new Mock<ICurrentUser>();
            mock.SetupGet(v => v.Id).Returns("1");
            mock.SetupGet(v => v.UserName).Returns("Jason");
            serviceCollection.AddScoped(_ => mock.Object);
        }

        public static FakeAuditObject GetAuditedObject(bool isDeleted = false)
        {
            return new FakeAuditObject(SystemClock.Now,
                "creatorId",
                SystemClock.Now,
                "modifierId",
                SystemClock.Now,
                "deleterId",
                isDeleted);
        }

        public static IEnumerable<object[]> GetAuditObjects()
        {
            yield return new object[] { new FakeAuditObject() };
            yield return new object[] { GetAuditedObject() };
            yield return new object[] { GetAuditedObject(true) };
        }

        public static IEnumerable<object[]> GetNonAuditObjects()
        {
            yield return new object[] { null };
            yield return new[] { new object() };
        }
    }
}