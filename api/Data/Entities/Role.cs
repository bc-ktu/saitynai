namespace api.Data.Entities
{
    public static class Roles
    {
        public const string Admin = nameof(Admin);
        public const string RegisteredUser = nameof(RegisteredUser);

        public static readonly IReadOnlyCollection<string> All = new[] { Admin, RegisteredUser };
    }
}
