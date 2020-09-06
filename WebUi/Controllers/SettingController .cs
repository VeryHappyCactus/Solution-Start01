using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebUi.Controllers
{
    
    public class SettingController : Controller
    {
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }
    
        [Authorize(Roles = "admin")]
        public IActionResult Test()
        {
            return View();
        }
    
    }
}
