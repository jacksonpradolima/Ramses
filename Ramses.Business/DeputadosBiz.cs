using Ramses.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml.Linq;

namespace Ramses.Business
{
    public class DeputadosBiz : BaseBusiness<Deputado>
    {
        protected override string OnGetErrorMessage(ErrorType error, Exception ex)
        {
            throw new NotImplementedException();
        }

        public void Load()
        {
            XDocument doc = XDocument.Load("C:/Users/Prado Lima/documents/visual studio 2012/Projects/Ramses/Ramses.Business/Arquivos/Deputados.xml");

            List<Deputado> listDeputado = new List<Deputado>();

            foreach (XElement element in doc.Descendants("Deputado"))
            {
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

                if (!listDeputado.Any(o => o.ideCadastro == deputado.ideCadastro))
                    listDeputado.Add(deputado);
            }

            listDeputado.Distinct();

            foreach (var item in listDeputado)
            {
                try
                {
                    Save(item);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }
    }
}
