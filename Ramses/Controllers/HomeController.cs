using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Ramses.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
    }
}