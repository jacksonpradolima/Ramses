using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ramses.Utils
{
    public class ResponseMessage
    {
        public string Message { get; set; }
        public ResponseMessageTypeEnum Type { get; set; }
    }
}