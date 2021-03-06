﻿using System.Reflection;
using Fabricdot.Domain.Core.Auditing;
using Fabricdot.Domain.Core.SharedKernel;
using Fabricdot.Infrastructure.Core.Security;

namespace Fabricdot.Infrastructure.Core.Domain.Auditing
{
    public class AuditPropertySetter : IAuditPropertySetter
    {
        protected ICurrentUser CurrentUser { get; }

        public AuditPropertySetter(ICurrentUser currentUser)
        {
            CurrentUser = currentUser;
        }

        public void SetCreationProperties(object targetObject)
        {
            SetCreationTime(targetObject);
            SetCreatorId(targetObject);
        }

        public void SetModificationProperties(object targetObject)
        {
            SetLastModificationTime(targetObject);
            SetLastModifierId(targetObject);
        }

        public void SetDeletionProperties(object targetObject)
        {
            SetIsDeleted(targetObject);
            SetDeletionTime(targetObject);
            SetDeleterId(targetObject);
        }

        private void SetCreationTime(object targetObject)
        {
            if (!(targetObject is IHasCreationTime objectWithCreationTime))
                return;

            if (objectWithCreationTime.CreationTime == default)
                Setter(targetObject, nameof(IHasCreationTime.CreationTime), SystemClock.Now);
        }

        private void SetCreatorId(object targetObject)
        {
            if (string.IsNullOrWhiteSpace(CurrentUser.Id) ||
                !(targetObject is IHasCreatorId objectWithCreationId))
                return;

            if (objectWithCreationId.CreatorId == default)
                Setter(targetObject, nameof(IHasCreatorId.CreatorId), CurrentUser.Id);
        }

        private void SetLastModificationTime(object targetObject)
        {
            if (targetObject is IHasModificationTime objectWithModificationTime)
                Setter(objectWithModificationTime, nameof(IHasModificationTime.LastModificationTime), SystemClock.Now);
        }

        private void SetLastModifierId(object targetObject)
        {
            if (!(targetObject is IHasModifierId))
                return;

            if (string.IsNullOrWhiteSpace(CurrentUser.Id))
            {
                Setter(targetObject, nameof(IHasModifierId.LastModifierId), null);
                return;
            }

            Setter(targetObject, nameof(IHasModifierId.LastModifierId), CurrentUser.Id);
        }

        private void SetIsDeleted(object targetObject)
        {
            if (targetObject is ISoftDelete softDeleteObject && !softDeleteObject.IsDeleted)
            {
                Setter(targetObject, nameof(ISoftDelete.IsDeleted), true);
            }
        }

        private void SetDeletionTime(object targetObject)
        {
            if (targetObject is IHasDeletionTime objectWithDeletionTime && objectWithDeletionTime.DeletionTime == null)
            {
                Setter(targetObject, nameof(IHasDeletionTime.DeletionTime), SystemClock.Now);
            }
        }

        private void SetDeleterId(object targetObject)
        {
            if (!(targetObject is IHasDeleterId objectWithDeleterId) ||
                !string.IsNullOrWhiteSpace(objectWithDeleterId.DeleterId))
                return;

            if (string.IsNullOrWhiteSpace(CurrentUser.Id))
            {
                Setter(targetObject, nameof(IHasDeleterId.DeleterId), null);
                return;
            }

            Setter(targetObject, nameof(IHasDeleterId.DeleterId), CurrentUser.Id);
        }

        private static void Setter(
            object targetObject,
            string propertyName,
            object val)
        {
            targetObject.GetType()
                .GetProperty(propertyName)
                ?.SetValue(targetObject, val, BindingFlags.Public | BindingFlags.Instance, null, null, null!);
        }
    }
}