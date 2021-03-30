﻿using IdentityModel;
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
        public static IEnumerable<Client> Clients =>
       new[]
       {
        // m2m client credentials flow client
        new Client
        {
          ClientId = "m2m.client",
          ClientName = "Client Credentials Client",

          AllowedGrantTypes = GrantTypes.ClientCredentials,
          ClientSecrets = {new Secret("secret".Sha256())},

          AllowedScopes = {"read", "write"}
        },
         new Client
                {
                    ClientId = "mvc_client",
                    AllowedGrantTypes = GrantTypes.Code,
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                     AllowedScopes = {"openid", "profile", "read"},
                     RedirectUris = { "https://localhost:44368/signin-oidc" },
             FrontChannelLogoutUri = "https://localhost:44368/signout-oidc" ,
          PostLogoutRedirectUris = {"https://localhost:44368/signout-callback-oidc" },
                },

        // interactive client using code flow + pkce
        new Client
        {
          ClientId = "interactive",
          ClientSecrets = {new Secret("secret".Sha256())},

          AllowedGrantTypes = GrantTypes.Code,

          RedirectUris = {"https://localhost:54554/signin-oidc"},
          FrontChannelLogoutUri = "https://localhost:54554/signout-oidc",
          PostLogoutRedirectUris = {"https://localhost:54554/signout-callback-oidc"},

          AllowOfflineAccess = true,
          AllowedScopes = {"openid", "profile", "read"},
          RequirePkce = true,
          RequireConsent = true,
          AllowPlainTextPkce = false
        },
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
                Name = "WebApi1",
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


        public static List<TestUser> GetUsers()
        {
            return new List<TestUser>
    {
        new TestUser
        {
            SubjectId = "1",
            Username = "user",
            Password = "user",
            Claims = new[]
            {
                new Claim("roleType", "CanReaddata")
            }
        },
        new TestUser
        {
            SubjectId = "2",
            Username = "admin",
            Password = "admin",
            Claims = new[]
            {
                new Claim("roleType", "CanUpdatedata")
            }
        }
    };
        }
    }
}
