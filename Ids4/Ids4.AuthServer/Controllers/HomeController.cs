using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ids4.AuthServer.Controllers
{
    public class HomeController: Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}
