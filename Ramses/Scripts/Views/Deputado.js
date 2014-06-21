$(document).ready(function () {
    // Se há o link para buscar os dados
    if (typeof url_pivot !== 'undefined') {
        // Busca a lista com os valores
        $.get(url_pivot, function (response) {
            // Array que vai manter os dados
            resultData = new Array();
            // Para cada valor que retornar pegar o dado e guardar no array (claro que peguei somente alguns dados)
            $.each(response, function (idx, result) {
                resultData.push({
                    txNomeParlamentar: result.nomeParlamentar,
                    sgUF: result.UFEleito,
                    sgPartido: result.LegendaPartidoEleito,
                    vlrLiquido: 1,
                    vlrDocumento: 1
                });
            });

            // Construção do PIVOT
            $("#pivot1").jbPivot(
                    {
                        // Campos do pivot (o nome do campo deve ser em minúsculo, só funcionou assim
                        fields: {
                            name: {
                                field: 'txNomeParlamentar',
                                sort: "asc",
                                showAll: false, // Deixar como false, senão ele fica replicando
                                agregateType: "distinct",
                                label: "Parlamentar"
                            },
                            partido: {
                                field: 'sgPartido',
                                sort: "asc",
                                showAll: true,
                                agregateType: "distinct",
                                label: "Partido"
                            },
                            estado: {
                                field: 'sgUF',
                                sort: "asc",
                                showAll: false,
                                agregateType: "distinct",
                                label: "Estado"
                            },
                            /* vlrdocumento: {
                                 field: 'vlrDocumento',
                                 sort: "asc",
                                 showAll: false,
                                 agregateType: "distinct",
                                 groupType: "none",
                                 label: "Valor Documento"
                             },
                             count: {
                                 agregateType: "count",
                                 groupType: "none",
                                 label: "Count"
                             },*/
                            sum: {
                                field: 'vlrDocumento',
                                agregateType: "sum",
                                groupType: "none",
                                label: "Soma"
                            }/*,
                            average: {
                                field: 'vlrDocumento',
                                agregateType: "average",
                                groupType: "none",
                                label: "Média",
                                formatter: function (V, f) {
                                    var res = null;
                                    if (typeof (V) === "number") {
                                        res = V.toFixed(2);
                                    }
                                    return res;
                                }
                            }*/
                        },
                        xfields: ["estado", "name"], // Quais campos que vamos colocar em X
                        yfields: ["partido"], // Quais campos que vamos colocar em Y
                        zfields: ["sum"],  // Campos de dados, só campos numéricos
                        data: resultData,
                        copyright: true,
                        summary: true,
                        l_all: "All",
                        l_unused_fields: "Available fields"
                    }
            );
        });
    }
});