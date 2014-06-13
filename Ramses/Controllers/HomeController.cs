using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Ramses.Utils;

namespace Ramses.Controllers
{
    public class HomeController : BaseController
    {
        [HttpGet]
        public ActionResult Index()
        {            
            ResponseMessage();
            return View();
        }
    }
}