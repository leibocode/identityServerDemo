using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcCookieAuthSample.ViewModels
{
    public class InputConSentViewModel
    {
        public string Button { get; set; }
        public IEnumerable<string> ScopedConSented { get; set; }
        public bool RemberConsented { get; set; }

        public string ReturnUrl { get; set; }
    }
}
