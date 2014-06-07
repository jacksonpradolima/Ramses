using Ramses.Business;
using Ramses.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ramses.Controllers
{
    public class DeputadoController : BaseController
    {
        [HttpGet]
        public ActionResult Index()
        {
            ResponseMessage();
            return View();
        }

        /// <summary>
        /// Carrega a base de dados dos deputados
        /// </summary>
        /// <returns>Mensagem com status do processamento</returns>
        public ActionResult LoadDeputados()
        {
            try
            {
                new DeputadosBiz().Load();
                ResponseMessage("msg_load_action", ResponseMessageTypeEnum.notice);
            }
            catch (BizException e)
            {
                string message = e.Message;

                // If there isn't some defined message to the error
                if (string.IsNullOrEmpty(message))
                {
                    message = "msg_internal_error";
                }

                this.ResponseMessage(message, ResponseMessageTypeEnum.notice);
            }
            catch (Exception e)
            {
                this.ResponseMessage("msg_internal_error", ResponseMessageTypeEnum.notice);
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Renderiza uma grid
        /// </summary>
        /// <returns>Pacial contendo a codificação da grid</returns>
        public ActionResult GridDeputados()
        {
            return PartialView("_GridDeputados", new DeputadosBiz().GetAll());
        }
    }
}