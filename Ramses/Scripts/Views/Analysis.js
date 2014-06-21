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
                    NomeParlamentar: result.NomeParlamentar,
                    Uf: result.Uf,
                    Partido: result.Partido,
                    Pib: result.Pib,
                    ValorDocumento: result.ValorDocumento
                });
            });

            // Construção do PIVOT
            $("#pivot1").jbPivot(
                    {
                        // Campos do pivot (o nome do campo deve ser em minúsculo, só funcionou assim
                        fields: {
                            name: {
                                field: 'NomeParlamentar',
                                sort: "asc",
                                showAll: false, // Deixar como false, senão ele fica replicando
                                agregateType: "distinct",
                                label: "Parlamentar"
                            },
                            partido: {
                                field: 'Partido',
                                sort: "asc",
                                showAll: true,
                                agregateType: "distinct",
                                label: "Partido"
                            },
                            estado: {
                                field: 'Uf',
                                sort: "asc",
                                showAll: false,
                                agregateType: "distinct",
                                label: "Estado"
                            },
                            pib: {
                                field: 'Pib',
                                sort: "asc",
                                showAll: false,
                                agregateType: "distinct",
                                label: "PIB"
                            },
                            valordocumento: {
                                field: 'ValorDocumento',
                                sort: "asc",
                                showAll: false,
                                agregateType: "distinct",
                                groupType: "none",
                                label: "Valor Documento"
                            }
                        },
                        xfields: ["estado", "name"], // Quais campos que vamos colocar em X
                        yfields: ["partido"], // Quais campos que vamos colocar em Y
                        zfields: ["pib"],  // Campos de dados, só campos numéricos
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