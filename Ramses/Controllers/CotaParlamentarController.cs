using Ramses.Business;
using Ramses.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ramses.Controllers
{
    public class CotaParlamentarController : BaseController
    {
        [HttpGet]
        public ActionResult Index()
        {
            ResponseMessage();
            return View();
        }
     
        /// <summary>
        /// Renderiza uma grid
        /// </summary>
        /// <returns>Pacial contendo a codificação da grid</returns>
        public ActionResult GridCotaParlamentar()
        {
            return PartialView("_GridCotaParlamentar", new CotaParlamentarBiz().GetAll());
        }

        /// <summary>
        /// Renderiza uma pivot
        /// </summary>
        /// <returns>View contendo a codificação da pivot</returns>
        public ActionResult PivotCotaParlamentar()
        {
            return View("PivotCotaParlamentar", new CotaParlamentarBiz().GetAll());
        }
    }
}