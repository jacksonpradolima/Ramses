using Ramses.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ramses.Controllers
{
    public class AnalysisController : Controller
    {
        //
        // GET: /Analysis/
        public ActionResult Index()
        {
            return View();
        }


        /// <summary>
        /// Renderiza uma grid
        /// </summary>
        /// <returns>Parcial contendo a codificação da grid</returns>
        public ActionResult GridAnalysis()
        {
            return PartialView("_GridAnalysis", new PibBiz().GetAllCotaPib());
        }

        /// <summary>
        /// Renderiza uma pivot
        /// </summary>
        /// <returns>View contendo a codificação da pivot</returns>
        public ActionResult PivotAnalysis()
        {
            return View("PivotAnalysis");
        }

        public JsonResult ListAnalysisPivot()
        {
            var list = new PibBiz().GetAllCotaPib().Take(10).ToList();

            return Json(list, JsonRequestBehavior.AllowGet);
        }
	}
}