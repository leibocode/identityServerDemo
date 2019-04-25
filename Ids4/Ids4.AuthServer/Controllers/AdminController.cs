﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ids4.AuthServer.Controllers
{
    [Authorize]
    public class AdminController:Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
