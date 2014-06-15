using Ramses.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml.Linq;
using Ramses.Business.Classes;

namespace Ramses.Business
{
    public class DeputadosBiz : BaseBusiness<Deputado>
    {
        protected override string OnGetErrorMessage(ErrorType error, Exception ex)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Método que carrega os deputados através de um arquivo xml para a base
        /// </summary>
        public void Load()
        {
            XDocument doc = XDocument.Load("C:/Users/Prado Lima/documents/visual studio 2012/Projects/Ramses/Ramses.Business/Arquivos/Deputados.xml");

            List<Deputado> listDeputado = new List<Deputado>();

            // Busca os deputados na base
            var listOldDeputados = GetAll();

            // Verifica os elementos do xml
            foreach (XElement element in doc.Descendants("Deputado"))
            {
                // Cria um objeto do tipo Deputado
                Deputado deputado = new Deputado()
                {
                    Anexo = element.Element("Anexo").Value,
                    Condicao = element.Element("Condicao").Value,
                    Fone = element.Element("Fone").Value,
                    Gabinete = element.Element("Gabinete").Value,
                    ideCadastro = element.Element("ideCadastro").Value,
                    LegendaPartidoEleito = element.Element("LegendaPartidoEleito").Value,
                    Matricula = element.Element("Matricula").Value,
                    nomeParlamentar = element.Element("nomeParlamentar").Value,
                    numLegislatura = element.Element("numLegislatura").Value,
                    Profissao = element.Element("Profissao").Value,
                    Sexo = element.Element("SEXO").Value,
                    SiTuacaoMandato = element.Element("SiTuacaoMandato").Value,
                    UFEleito = element.Element("UFEleito").Value
                };

                // Não adiciona os deputados já cadastrados
                if (!listDeputado.Any(o => string.Compare(o.ideCadastro, deputado.ideCadastro, true) == 0) 
                 && !listOldDeputados.Any(o => string.Compare(o.ideCadastro, deputado.ideCadastro, true) == 0))
                {
                    listDeputado.Add(deputado);
                    Context.AddToDeputados(deputado);
                }
            }

            // Salva todos de uma vez só
            if (Context.HasPendingChanges())
                this.Context.SaveChanges();
        }

        /// <summary>
        /// Retornar a quantidade de deputados por UF
        /// </summary>
        /// <param name="array">Array com estados Somente a sigla</param>
        /// <returns>Retorna array no estilo ( {SC,20} , {PR,10}, necessário para uso no gráfico Point (PIE)</returns>
        public string[,] getCountByUF(string[] array)
        {
            IEnumerable<Deputado> listaDep = new DeputadosBiz().GetAll();
            List<UF_Count> qtdeDep = new List<UF_Count>();

            foreach (string s in array)
            {
                //adicionar na lista
                qtdeDep.Add(new UF_Count()
                                    {
                                        uf = s,//recebe a sigla do estado
                                        qtde = listaDep.Count( i=> i.UFEleito.Equals(s.ToUpper()))//armazenar a qtde
                                    }
                            );
            }

            string[,] dados = new string[qtdeDep.Count, 2];
            for (int i = 0; i < qtdeDep.Count; i++)
            {
                dados[i, 0] = qtdeDep.ElementAt(i).uf;
                dados[i, 1] = qtdeDep.ElementAt(i).qtde.ToString();
            }


            //Retornar
            return dados;
        }
    }
}
