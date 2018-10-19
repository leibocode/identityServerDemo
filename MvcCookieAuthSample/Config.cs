using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcCookieAuthSample
{
    public class Config
    {
        /// <summary>
        /// 所以可以被访问的Resource
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("api1","API Application")
            };
        }

        //定义客户端
        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client()
                {
                    ClientId ="mvc",
                    ClientName ="MyMvc",
                    ClientUri="http://localhost:5001",
                    LogoUri="https://chocolatey.org/content/packageimages/aspnetcore-runtimepackagestore.2.0.0.png",
                    AllowRememberConsent =true,
                    AllowedGrantTypes = GrantTypes.Implicit,
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = {
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OpenId,
                    },
                    RedirectUris ={
                        "http://localhost:5001/signin-oidc"
                    },
                    PostLogoutRedirectUris ={
                        "http://localhost:5001/signout-callback-oidc"
                    },
                    RequireConsent =false
                }
            };
        }

        //定义 系统资源
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new  IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email()
            };
        }

        //测试
        public static List<TestUser> GetTestUsers()
        {
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "10000",
                    Username ="lb",
                    Password ="password"
                }
            };
        }
    }
}
