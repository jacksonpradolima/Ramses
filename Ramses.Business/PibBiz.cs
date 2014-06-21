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

        public List<CotaPib> GetAllCotaPib()
        {
            var list = (from cota in Context.CotaParlamentars
                            join pib in ObjectSet
                                  on cota.sgUF equals pib.uf into joinCotaPib
                            from cotaPib in joinCotaPib.DefaultIfEmpty()
                            select new CotaPib()
                            {
                                Ano = cota.numAno,
                                DataEmissao = cota.datEmissao,
                                Mes = cota.numMes,
                                NomeParlamentar = cota.txNomeParlamentar,
                                Partido = cota.sgPartido,
                                Pib = cotaPib.pib.HasValue ? cotaPib.pib.Value : 0,
                                Uf = cota.sgUF,
                                UfDesc = cotaPib.nomeuf,
                                ValorDocumento = cota.vlrDocumento,
                                ValorGlosa = cota.vlrGlosa,
                                ValorLiquido = cota.vlrLiquido
                            }).ToList();

            return list;
        }
    }
}
