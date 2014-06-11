using Ramses.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml.Linq;

namespace Ramses.Business
{
    public class CotaParlamentarBiz : BaseBusiness<CotaParlamentar>
    {
        protected override string OnGetErrorMessage(ErrorType error, Exception ex)
        {
            throw new NotImplementedException();
        }      
    }
}
