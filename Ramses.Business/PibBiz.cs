using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ramses.Data;

namespace Ramses.Business
{
    public class PibBiz : BaseBusiness<PibPorUF>
    {
        protected override string OnGetErrorMessage(ErrorType error, Exception ex)
        {
            throw new NotImplementedException();
        }
    }
}
