using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ids4.AuthServer
{
    public class Config
    {
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("api","App Application")
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client()
                {
                    ClientId ="mvc",
                    ClientName = "mvc Client",
                    ClientUri ="http://localhost:5001",
                    LogoUri="https://chocolatey.org/content/packageimages/aspnetcore-runtimepackagestore.2.0.0.png",
                    AllowedGrantTypes = GrantTypes.Implicit,//模式:最简单的模式
                    ClientSecrets = {
                         new Secret("secret".Sha256())
                    },
                    AllowedScopes = {
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Email
                    }, 
                    RedirectUris = {//跳转登录到客户端的地址
                       "http://localhost:5001/signin-oidc"
                    },
                    PostLogoutRedirectUris = { //跳转登出到客户端的地址
                        "http://localhost:5001/signout-callback-oidc"
                    },
                    RequireConsent = true,
                    AllowRememberConsent = true
                     
                }
            };
        }

        public static List<TestUser> GetTestUsers()
        {
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "10000",
                    Username ="lb",
                    Password = "password"
                }
            };
        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email()
            };
        }
    }
}
