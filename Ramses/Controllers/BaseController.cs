using Ramses.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ramses.Controllers
{
    public abstract class BaseController : Controller
    {
        #region Response Message

        /// <summary>
        /// Seta um TempData para armazenar a mensagem desejada e o tipo de exibição
        /// </summary>
        /// <param name="message">Mensagem</param>
        /// <param name="type">Tipo de exibição</param>
        public void ResponseMessage(string message, ResponseMessageTypeEnum type)
        {
            TempData["ReturnMessage"] = new ResponseMessage { Message = Resources.Literals.ResourceManager.GetString(message), Type = type };
        }

        /// <summary>
        /// Pega o que está no TempDate e seta a ViewBag de mensagens
        /// </summary>
        public void ResponseMessage()
        {
            var response = (ResponseMessage)TempData["ReturnMessage"];

            if (response != null)
            {
                switch (response.Type)
                {
                    case ResponseMessageTypeEnum.warning:
                        ViewBag.ErrorMessage = response.Message;
                        break;
                    case ResponseMessageTypeEnum.error:
                        ViewBag.ErrorMessage = response.Message;
                        break;
                    case ResponseMessageTypeEnum.success:
                        ViewBag.SuccessMessage = response.Message;
                        break;
                    case ResponseMessageTypeEnum.notice:
                        ViewBag.AlertMessage = response.Message;
                        break;
                }

                TempData["ReturnMessage"] = null;
            }
        }

        #endregion Response Message
    }
}