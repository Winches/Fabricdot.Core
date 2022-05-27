namespace Fabricdot.Authorization.Permissions
{
    public static class StandardPermissions
    {
        public static class Operations
        {
            public const string Create = nameof(Create);

            public const string Update = nameof(Update);

            public const string Delete = nameof(Delete);

            public const string Read = nameof(Read);
        }

        public const string Separator = ".";

        public static readonly PermissionName Superuser = new(nameof(Superuser));
    }
}