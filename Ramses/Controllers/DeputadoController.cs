﻿using Ramses.Business;
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

            //Array2d com nro de linhas igual ao count e 2 colunas
            string[,] dados = new DeputadosBiz().getCountByUF(listaUF.Select(i => i.uf).ToArray());

            //objeto chart
            var chart = new Highcharts("chartEstado")
                //tipo do gráfico
                .InitChart(new Chart { DefaultSeriesType = ChartTypes.Pie, PlotShadow = false })
                //titulo
                .SetTitle(new Title { Text = Resources.Literals.lbl_grafico1_titulo })
                //subtitulo
                .SetSubtitle(new Subtitle { Text = Resources.Literals.lbl_grafico1_subtitulo })

                //Setar o titulo do Y
                .SetYAxis(new YAxis { Title = new YAxisTitle { Text = "Número de Deputados" } })
                .SetTooltip(new Tooltip { Formatter = "function() { return '<b>'+ this.point.name +'</b>: '+ this.y +' Deputados'; }" })
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
                    new Series{Type = ChartTypes.Pie,Name="Deputados por UF", Data = new DotNet.Highcharts.Helpers.Data(dados)}
                    //Pode criar uma segunda linha usando o comando acima
               }
               );

            return PartialView("_DeputadoPorEstado", chart);

        }

        /// <summary>
        /// Renderiza um gráfico de pizza da qtde de deputados por partido
        /// </summary>
        /// <returns>Parcial contendo o gráfico dos deputados por partido</returns>
        public ActionResult DeputadoPorPartido()
        {
            //Array2d com nro de linhas igual ao count e 2 colunas
            string[,] dados = new DeputadosBiz().getCountByPartido();

            //objeto chart
            var chart = new Highcharts("chartPartido")
                //tipo do gráfico
                .InitChart(new Chart { DefaultSeriesType = ChartTypes.Pie, PlotShadow = false })
                //titulo
                .SetTitle(new Title { Text = Resources.Literals.lbl_grafico2_titulo })
                //subtitulo
                .SetSubtitle(new Subtitle { Text = Resources.Literals.lbl_grafico2_subtitulo })

                //Setar o titulo do Y
                .SetYAxis(new YAxis { Title = new YAxisTitle { Text = "Número de Deputados" } })
                .SetTooltip(new Tooltip { Formatter = "function() { return '<b>'+ this.point.name +'</b>: '+ this.y +' Deputados'; }" })
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
                    new Series{Type = ChartTypes.Pie,Name="Deputados por Partido", Data = new DotNet.Highcharts.Helpers.Data(dados)}
                    //Pode criar uma segunda linha usando o comando acima
               }
               );

            return PartialView("_DeputadoPorPartido", chart);

        }

        /// <summary>
        /// Renderiza uma pivot
        /// </summary>
        /// <returns>View contendo a codificação da pivot</returns>
        public ActionResult PivotDeputado()
        {
            return View("PivotDeputado");
        }

        public JsonResult ListDeputadosPivot()
        {
            var list = new DeputadosBiz().GetAll();

            return Json(list, JsonRequestBehavior.AllowGet);
        }
    }
}