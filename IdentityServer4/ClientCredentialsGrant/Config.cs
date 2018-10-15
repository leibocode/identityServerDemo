using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientCredentialsGrant
{
    public class Config
    {
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("api","")
            };
        }

        public static IEnumerable<Client> GetClients()
        {

            return new List<Client>
            {
                 //Oauth-客户端模式
                new Client()
                {
                   ClientId ="client",
                   AllowedGrantTypes = GrantTypes.ClientCredentials,
                   ClientSecrets ={
                        new Secret("secret".Sha256())
                    },
                   AllowedScopes ={
                        "api"
                    }
                },
                new Client()
                {
                    ClientId = "pwdClient",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    ClientSecrets = {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes ={
                        "api"
                    }
                }
            };
        }

        public static List<TestUser> GetTestUsers()
        {
            return new List<TestUser>
            {
                new TestUser()
                {
                    SubjectId = "1",
                    Username ="lb",
                    Password ="123456"
                }
            };
        }


    }
}
