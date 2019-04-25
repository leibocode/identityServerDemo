using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ids4.AuthServer.ViewModels
{
    public class InputConsentViewModel
    {
        public string Button { get; set; }
        public IEnumerable<string> ScopedsConsented { get; set; }
        public bool RememberConsent { get; set; }
        public string ReturnUrl { get; set; }
    }
}
