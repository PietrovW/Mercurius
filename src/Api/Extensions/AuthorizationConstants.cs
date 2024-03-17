namespace Api.Extensions;

public static class AuthorizationConstants
{
    public static class Roles
    {
        public const string AspNetCoreRole = "realm-role";

        public const string RealmRole = "realm-role";

        public const string ClientRole = "client-role";
    }

    public static class Policies
    {
        public const string RequireAspNetCoreRole = nameof(RequireAspNetCoreRole);

        public const string RequireRealmRole = nameof(RequireRealmRole);

        public const string RequireClientRole = nameof(RequireClientRole);

        public const string RequireToBeInKeycloakGroupAsReader =
            nameof(RequireToBeInKeycloakGroupAsReader);
    }
}
