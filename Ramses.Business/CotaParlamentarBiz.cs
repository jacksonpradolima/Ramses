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

            /// <summary>
        /// Retornar os que mais gastaram
        /// </summary>
        /// <returns>Retorna array no estilo ( {PT,20} , {PTdoB,10}, necessário para uso no gráfico Point (PIE)</returns>
        public string[,] pegarOsQueGastamMais()
        {
            var agrupamento = this.GetAll()
                              .GroupBy(i => i.txNomeParlamentar)
                              .Select(g => new
                              {
                                  NOME = g.Key,
                                  TOTAL = g.Sum(i => Convert.ToDecimal(i.vlrDocumento))
                              })
                              .OrderByDescending(g => Convert.ToDecimal(g.TOTAL))
                              .Take(10);

            string[,] dados = new string[agrupamento.ToList().Count, 2];
            int c = 0;
            foreach (var linha in agrupamento)
            {
                dados[c, 0] = linha.NOME;
                dados[c, 1] = linha.TOTAL.ToString();
                c++;
            }


            //Retornar
            return dados;
        }

        /// <summary>
        /// Retornar os que menoss gastaram
        /// </summary>
        /// <returns>Retorna array no estilo ( {PT,20} , {PTdoB,10}, necessário para uso no gráfico Point (PIE)</returns>
        public string[,] pegarOsQueGastamMenos()
        {
            var agrupamento = this.GetAll()
                              .GroupBy(i => i.txNomeParlamentar)
                              .Select(g => new
                              {
                                  NOME = g.Key,
                                  TOTAL = g.Sum(i => Convert.ToDecimal(i.vlrDocumento))
                              })
                              .OrderBy(g => Convert.ToDecimal(g.TOTAL))
                              .Take(10);

            string[,] dados = new string[agrupamento.ToList().Count, 2];
            int c = 0;
            foreach (var linha in agrupamento)
            {
                dados[c, 0] = linha.NOME;
                dados[c, 1] = linha.TOTAL.ToString();
                
                c++;
            }


            //Retornar
            return dados;
        }
    }
}
