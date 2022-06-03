using System;
using System.Collections.Generic;
using System.Linq;
using Ardalis.GuardClauses;

namespace Fabricdot.Authorization.Permissions
{
    public class PermissionGroup
    {
        private readonly List<Permission> _permissions = new();

        private string _displayName = null!;

        public string Name { get; private set; }

        public string DisplayName
        {
            get => _displayName;
            set
            {
                _displayName = Guard.Against.NullOrWhiteSpace(value, nameof(DisplayName));
            }
        }

        public IReadOnlyCollection<Permission> Permissions => _permissions.AsReadOnly();

        public PermissionGroup(
            string name,
            string displayName)
        {
            Name = Guard.Against.NullOrWhiteSpace(name, nameof(name));
            DisplayName = displayName;
        }

        public Permission AddPermission(
            PermissionName name,
            string displayName,
            string? description = null)
        {
            if (FindPermission(name) is not null)
                throw new InvalidOperationException("Permission is already exists.");

            var permission = new Permission(
                name,
                displayName,
                description);
            _permissions.Add(permission);
            return permission;
        }

        public void RemovePermission(Permission permission)
        {
            Guard.Against.Null(permission, nameof(permission));
            _permissions.Remove(permission);
        }

        public Permission? FindPermission(PermissionName name)
        {
            return _permissions.SingleOrDefault(v => v.Name == name);
        }

        public override string ToString() => $"PermissionGroup:{Name}";
    }
}