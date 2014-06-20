$(document).ready(function () {
    // Se há o link para buscar os dados
    if (typeof url_pivot_menor !== 'undefined') {
        // Busca a lista com os valores
        $.get(url_pivot_menor, function (response) {
            // Array que vai manter os dados
            resultData = new Array();
            // Para cada valor que retornar pegar o dado e guardar no array (claro que peguei somente alguns dados)
            $.each(response, function (idx, result) {
                resultData.push({
                    txNomeParlamentar: result.txNomeParlamentar,
                    sgUF: result.sgUF,
                    sgPartido: result.sgPartido,
                    vlrLiquido: result.vlrLiquido,
                    vlrDocumento: result.vlrDocumento
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
                            state: {
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
                                label: "Sum"
                            },
                            average: {
                                field: 'vlrDocumento',
                                agregateType: "average",
                                groupType: "none",
                                label: "Average",
                                formatter: function (V, f) {
                                    var res = null;
                                    if (typeof (V) === "number") {
                                        res = V.toFixed(2);
                                    }
                                    return res;
                                }
                            }
                        },
                        xfields: ["state", "name"], // Quais campos que vamos colocar em X
                        yfields: ["partido"], // Quais campos que vamos colocar em Y
                        zfields: ["sum", "average"],  // Campos de dados, só campos numéricos
                        data: resultData,
                        copyright: true,
                        summary: true,
                        l_all: "All",
                        l_unused_fields: "Available fields"
                    }
            );
        });
    }
    // Se há o link para buscar os dados
    if (typeof url_pivot_maior !== 'undefined') {
        // Busca a lista com os valores
        $.get(url_pivot_maior, function (response) {
            // Array que vai manter os dados
            resultData = new Array();
            // Para cada valor que retornar pegar o dado e guardar no array (claro que peguei somente alguns dados)
            $.each(response, function (idx, result) {
                resultData.push({
                    txNomeParlamentar: result.txNomeParlamentar,
                    sgUF: result.sgUF,
                    sgPartido: result.sgPartido,
                    vlrLiquido: result.vlrLiquido,
                    vlrDocumento: result.vlrDocumento
                });
            });

            // Construção do PIVOT
            $("#pivot2").jbPivot(
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
                            state: {
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
                                label: "Sum"
                            },
                            average: {
                                field: 'vlrDocumento',
                                agregateType: "average",
                                groupType: "none",
                                label: "Average",
                                formatter: function (V, f) {
                                    var res = null;
                                    if (typeof (V) === "number") {
                                        res = V.toFixed(2);
                                    }
                                    return res;
                                }
                            }
                        },
                        xfields: ["state", "name"], // Quais campos que vamos colocar em X
                        yfields: ["partido"], // Quais campos que vamos colocar em Y
                        zfields: ["sum", "average"],  // Campos de dados, só campos numéricos
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