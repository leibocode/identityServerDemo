using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcCookieAuthSample.ViewModels
{
    public class ConSentViewModel: InputConSentViewModel
    {
        public string clientId { get; set; }
        public string ClientName { get; set; }
        public string ClientUrl { get; set; }
        public string ClientLogoUrl { get; set; }

        public IEnumerable<ScopeViewModel> IdentitySceops { get; set; }
        public IEnumerable<ScopeViewModel> ResourceScopes { get; set; }
    }
}
