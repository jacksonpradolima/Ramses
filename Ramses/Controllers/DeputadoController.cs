using Ramses.Business;
using Ramses.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ramses.Data;
using Ramses.Models;
using DotNet.Highcharts;
using DotNet.Highcharts.Options;
using DotNet.Highcharts.Enums;
using Ramses.Business.Classes;

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
        /// <returns>Parcial contendo a codificação da grid</returns>
        public ActionResult GridDeputados()
        {
            return PartialView("_GridDeputados", new DeputadosBiz().GetAll());
        }

        /// <summary>
        /// Renderiza um gráfico de pizza da qtde de deputados por estado
        /// </summary>
        /// <returns>Parcial contendo o gráfico dos deputados por estado</returns>
        public ActionResult DeputadoPorEstado()
        {
            //Lista de UF
            IEnumerable<PibPorUF> listaUF = new PibBiz().GetAll();
            List<UF_Count> listaDados = new DeputadosBiz().getCountByUF(listaUF.Select(i => i.uf).ToArray());

            //Arrays do gráfico
            var xData = listaDados.Select(i => i.uf).ToArray();
            var yData = listaDados.Select(i => new object[] { i.qtde}).ToArray();

            //objeto chart
            var chart = new Highcharts("chart")
                //tipo do gráfico
                .InitChart(new Chart { DefaultSeriesType = ChartTypes.Pie})
                //titulo
                .SetTitle(new Title { Text = Resources.Literals.lbl_grafico1_titulo })
                //subtitulo
                .SetSubtitle(new Subtitle { Text = Resources.Literals.lbl_grafico1_subtitulo})
                //carregar os valores do x
                .SetXAxis(new XAxis { Categories = xData })
                //Setar o titulo do Y
                .SetYAxis(new YAxis { Title = new YAxisTitle { Text = "Número de Deputados" } })
                .SetTooltip(new Tooltip
                {
                    Enabled = true,
                    Formatter = @"function() { return '<b>' + this.series.name+'</b><br/>'+this.x+': '+this.y;}"
                })
                .SetPlotOptions(new PlotOptions
                {
                    Line = new PlotOptionsLine
                    {
                        DataLabels = new PlotOptionsLineDataLabels
                        {
                            Enabled = true
                        },
                        EnableMouseTracking = true
                    }
                })
                //Carregar os valores do Y
                .SetSeries(new[]{
                    new Series{Name="Deputados por UF", Data = new DotNet.Highcharts.Helpers.Data(yData)}
                    //Pode criar uma segunda linha usando o comando acima
               }
               );

            return PartialView("_DeputadoPorEstado",chart);
            //return PartialView("_DeputadoPorEstado",new PibBiz().GetAll());
        }
    }
}