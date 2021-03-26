using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Json;

namespace IdentityServer
{
    public class ServerConfiguration
    {
        public static List<Client> Clients = new List<Client>
        {
                new Client
                {
                    ClientId = "m2m.client",
                    ClientName = "Client Credentials Client",
                     ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedGrantTypes = new List<string> { GrantType.ClientCredentials },

                    AllowedScopes = { "WebApplication1.api", "write", "read" },
                    Claims = new List<ClientClaim>
                    {
                        new ClientClaim("companyName", "John Doe LTD")
                        //more custom claims depending on the logic of the api
                    },
                    AllowedCorsOrigins = new List<string>
                    {
                        "https://localhost:5001",
                    },
                    AccessTokenLifetime = 86400
                },
                 // interactive client using code flow + pkce
                new Client
                {
                  ClientId = "WebApp.MVC",
                  ClientSecrets = {new Secret("secret".Sha256())},

                  AllowedGrantTypes = new List<string> { GrantType.Implicit },

                  RedirectUris = {"https://localhost:44341/signin-oidc"},
                  FrontChannelLogoutUri = "https://localhost:44341/signout-oidc",
                  PostLogoutRedirectUris = {"https://localhost:44341/signout-callback-oidc"},

                  //AllowOfflineAccess = true,
                  AllowedScopes = {"openid", "profile", "read"},
                  RequirePkce = true,
                  RequireConsent = true,
                  AllowPlainTextPkce = false
                }
        };

        public static IEnumerable<IdentityResource> IdentityResources => new[]
                   {
                        new IdentityResources.OpenId(),
                        new IdentityResources.Profile(),
                        new IdentityResource
                        {
                          Name = "role",
                          UserClaims = new List<string> {"role"}
                        }
                       };

        public static List<ApiResource> ApiResources = new List<ApiResource>
        {
            new ApiResource
            {
                Name = "WebApplication1.api",
                DisplayName = "My Fancy Secured API",
                Scopes = new List<string>
                {
                    "write",
                    "read"
                }
            }
        };

        public static IEnumerable<ApiScope> ApiScopes = new List<ApiScope>
        {
            new ApiScope("read"),
            new ApiScope("write")
        };


        public static List<TestUser> Users
        {
            get
            {
                var address = new
                {
                    street_address = "One Hacker Way",
                    locality = "Heidelberg",
                    postal_code = 69118,
                    country = "Germany"
                };

                return new List<TestUser>
                {
                    new TestUser
                    {
                        SubjectId = "818727",
                        Username = "alice",
                        Password = "alice",
                        Claims =
                        {
                            new Claim(JwtClaimTypes.Name, "Alice Smith"),
                            new Claim(JwtClaimTypes.GivenName, "Alice"),
                            new Claim(JwtClaimTypes.FamilyName, "Smith"),
                            new Claim(JwtClaimTypes.Email, "AliceSmith@email.com"),
                            new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                            new Claim(JwtClaimTypes.WebSite, "http://alice.com"),
                            new Claim(JwtClaimTypes.Address, JsonSerializer.Serialize(address), IdentityServerConstants.ClaimValueTypes.Json)
                        }
                    },
                    new TestUser
                    {
                        SubjectId = "88421113",
                        Username = "bob",
                        Password = "bob",
                        Claims =
                        {
                            new Claim(JwtClaimTypes.Name, "Bob Smith"),
                            new Claim(JwtClaimTypes.GivenName, "Bob"),
                            new Claim(JwtClaimTypes.FamilyName, "Smith"),
                            new Claim(JwtClaimTypes.Email, "BobSmith@email.com"),
                            new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                            new Claim(JwtClaimTypes.WebSite, "http://bob.com"),
                            new Claim(JwtClaimTypes.Address, JsonSerializer.Serialize(address), IdentityServerConstants.ClaimValueTypes.Json)
                        }
                    }
                };
            }
        }
    }
}
