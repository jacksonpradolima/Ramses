using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DotNet.Highcharts;
using DotNet.Highcharts.Enums;
using DotNet.Highcharts.Options;
using Ramses.Models;

namespace Ramses.Controllers
{
    public class ChartExampleController : Controller
    {
     
        public ActionResult Index()
        {
            //lista com os dados
            var transactionCounts = new List<HcExample>
            {
                new HcExample(){Monthname="Jan",count=30},
                new HcExample(){Monthname="Fev",count=10},
                new HcExample(){Monthname="Mar",count=40},
                new HcExample(){Monthname="Abr",count=80},
                new HcExample(){Monthname="Mai",count=60}

            };

            //Arrays do gráfico
            var xData = transactionCounts.Select(i => i.Monthname).ToArray();
            var yData = transactionCounts.Select(i => new object[]{i.count}).ToArray();

            //objeto chart
            var chart = new Highcharts("chart")
                //tipo do gráfico
                .InitChart(new Chart { DefaultSeriesType = ChartTypes.Line })
                //titulo
                .SetTitle(new Title { Text = "Exemplo" })
                //subtitulo
                .SetSubtitle(new Subtitle { Text = "Subtitulo" })
                //carregar os valores do x
                .SetXAxis(new XAxis { Categories = xData })
                //Setar o titulo do Y
                .SetYAxis(new YAxis { Title = new YAxisTitle { Text = "Número de Transações" } })
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
                        EnableMouseTracking = false
                    }
                })
                //Carregar os valores do Y
                .SetSeries(new[]{
                    new Series{Name="Hora", Data = new DotNet.Highcharts.Helpers.Data(yData)}
                    //Pode criar uma segunda linha usando o comando acima
               }
               );

            return View(chart);
        }
	}
}