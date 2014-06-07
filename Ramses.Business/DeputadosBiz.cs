using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ramses.Business
{
    public class DeputadosBiz
    {
        public void ObterDeputados()
        {
            new Data.Deputados().GetAllDeputados();
        }
    }
}
