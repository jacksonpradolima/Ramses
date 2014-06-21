using Ramses.Business;
using Ramses.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using DotNet.Highcharts;
using DotNet.Highcharts.Options;
using DotNet.Highcharts.Enums;

namespace Ramses.Controllers
{
    public class CotaParlamentarController : BaseController
    {
        [HttpGet]
        public ActionResult Index()
        {

            ResponseMessage();
            return View();
        }

        /// <summary>
        /// Renderiza um gráfico de pizza dos que mais gastaram
        /// </summary>
        /// <returns>Parcial contendo o gráfico dos que mais gastaram</returns>
        public ActionResult CotaMaior()
        {
            //Array2d com nro de linhas igual ao count e 2 colunas
            string[,] dados = new CotaParlamentarBiz().pegarOsQueGastamMais();

            //objeto chart
            var chart = new Highcharts("chartMaior")
                //tipo do gráfico
                .InitChart(new Chart { DefaultSeriesType = ChartTypes.Pie, PlotShadow = false })
                //titulo
                .SetTitle(new Title { Text = Resources.Literals.lbl_grafico4_titulo })
                //subtitulo
                .SetSubtitle(new Subtitle { Text = Resources.Literals.lbl_grafico4_subtitulo })

                //Setar o titulo do Y
                .SetYAxis(new YAxis { Title = new YAxisTitle { Text = "" } })
                .SetTooltip(new Tooltip { Formatter = "function() { return '<b>'+ this.point.name +'</b>: '+ 'R$' + this.y ; }" })
                .SetPlotOptions(new PlotOptions
                {
                    Pie = new PlotOptionsPie
                    {
                        AllowPointSelect = true,
                        DataLabels = new PlotOptionsPieDataLabels
                        {
                            Formatter = "function() { return '<b>'+ this.point.name +'</b>: '+ this.y; }",
                            Enabled = true
                        },
                        EnableMouseTracking = true
                    }
                })
                //Carregar os valores do Y
                .SetSeries(new[]{
                    new Series{Type = ChartTypes.Pie,Name="Deputados que gastam mais", Data = new DotNet.Highcharts.Helpers.Data(dados)}
                    //Pode criar uma segunda linha usando o comando acima
               }
               );

            return PartialView("_CotaMaior", chart);

        }

        /// <summary>
        /// Renderiza um gráfico de pizza dos que menos gastaram
        /// </summary>
        /// <returns>Parcial contendo o gráfico dos que menos gastaram</returns>
        public ActionResult CotaMenor()
        {
            //Array2d com nro de linhas igual ao count e 2 colunas
            string[,] dados = new CotaParlamentarBiz().pegarOsQueGastamMenos();

            //objeto chart
            var chart = new Highcharts("chartMenor")
                //tipo do gráfico
                .InitChart(new Chart { DefaultSeriesType = ChartTypes.Pie, PlotShadow = false })
                //titulo
                .SetTitle(new Title { Text = Resources.Literals.lbl_grafico3_titulo })
                //subtitulo
                .SetSubtitle(new Subtitle { Text = Resources.Literals.lbl_grafico3_subtitulo })

                //Setar o titulo do Y
                .SetYAxis(new YAxis { Title = new YAxisTitle { Text = "" } })
                .SetTooltip(new Tooltip { Formatter = "function() { return '<b>'+ this.point.name +'</b>: '+ 'R$' + this.y ; }" })
                .SetPlotOptions(new PlotOptions
                {
                    Pie = new PlotOptionsPie
                    {
                        AllowPointSelect = true,
                        DataLabels = new PlotOptionsPieDataLabels
                        {
                            Formatter = "function() { return '<b>'+ this.point.name +'</b>: '+ this.y; }",
                            Enabled = true
                        },
                        EnableMouseTracking = true
                    }
                })
                //Carregar os valores do Y
                .SetSeries(new[]{
                    new Series{Type = ChartTypes.Pie,Name="Deputados que gastam menos", Data = new DotNet.Highcharts.Helpers.Data(dados)}
                    //Pode criar uma segunda linha usando o comando acima
               }
               );

            return PartialView("_CotaMenor", chart);

        }

        /// <summary>
        /// Renderiza uma grid
        /// </summary>
        /// <returns>Pacial contendo a codificação da grid</returns>
        public ActionResult GridCotaParlamentar()
        {
            return PartialView("_GridCotaParlamentar", new CotaParlamentarBiz().GetAll());
        }

        /// <summary>
        /// Renderiza uma pivot
        /// </summary>
        /// <returns>View contendo a codificação da pivot</returns>
        public ActionResult PivotCotaParlamentar()
        {
            return View("PivotCotaParlamentar");
        }

        public ActionResult ListCotaParlamentarMaior()
        {
            var list = new CotaParlamentarBiz().GetMore();

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ListCotaParlamentarMenor()
        {
            var list = new CotaParlamentarBiz().GetLess();

            return Json(list, JsonRequestBehavior.AllowGet);
        }
    }
}