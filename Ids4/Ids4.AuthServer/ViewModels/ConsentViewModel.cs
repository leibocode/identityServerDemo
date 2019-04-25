using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ids4.AuthServer.ViewModels
{
    public class ConsentViewModel:InputConsentViewModel
    {
        //public string ClientId { get; set; }
        //public string ClientUrl { get; set; }
        //public string ClientName { get; set; }
        //public string ClientLogoUrl { get; set; }

        //public IEnumerable<ScopeViewModel> IdentityScorpes { get; set; }
        //public IEnumerable<ScopeViewModel> ResourceScorpes { get; set; }
        public string ClientId { get; set; }
        public string ClientName { get; set; }
        public string ClientUrl { get; set; }
        public string ClientLogoUrl { get; set; }
        public bool AllowRememberConsent { get; set; }

        public IEnumerable<ScopeViewModel> IdentityScopes { get; set; }
        public IEnumerable<ScopeViewModel> ResourceScopes { get; set; }
    }
}
