using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Ks.MonitorCPI.Entidades.Produtos
{
	[XmlRoot(ElementName = "row")]
	public class Row
	{
		[XmlElement(ElementName = "CodigoSAP__c")]
		public string CodigoSAP__c { get; set; }
		[XmlElement(ElementName = "CodigoTOTVS__c")]
		public string CodigoTOTVS__c { get; set; }
		[XmlElement(ElementName = "Name")]
		public string Name { get; set; }
		[XmlElement(ElementName = "EAN__c")]
		public string EAN__c { get; set; }
		[XmlElement(ElementName = "BloqueioRegulatorio__c")]
		public string BloqueioRegulatorio__c { get; set; }
		[XmlElement(ElementName = "IsentoICMS__c")]
		public string IsentoICMS__c { get; set; }
		[XmlElement(ElementName = "Refrigerado__c")]
		public string Refrigerado__c { get; set; }
		[XmlElement(ElementName = "Controlado_c")]
		public string Controlado_c { get; set; }
		[XmlElement(ElementName = "Convenios__c")]
		public string Convenios__c { get; set; }
		[XmlElement(ElementName = "Obsoleto__c")]
		public string Obsoleto__c { get; set; }
		[XmlElement(ElementName = "PesoBruto__c")]
		public string PesoBruto__c { get; set; }
		[XmlElement(ElementName = "PesoLiquido__c")]
		public string PesoLiquido__c { get; set; }
		[XmlElement(ElementName = "TipoProduto__c")]
		public string TipoProduto__c { get; set; }
		[XmlElement(ElementName = "SubstanciaAtiva__c")]
		public string SubstanciaAtiva__c { get; set; }
		[XmlElement(ElementName = "Lancamento__c")]
		public string Lancamento__c { get; set; }
		[XmlElement(ElementName = "CategoriaAnvisa__c")]
		public string CategoriaAnvisa__c { get; set; }
		[XmlElement(ElementName = "UnidadeVenda__c")]
		public string UnidadeVenda__c { get; set; }
		[XmlElement(ElementName = "RegistroMS__c")]
		public string RegistroMS__c { get; set; }
		[XmlElement(ElementName = "ListaPISCOFINS__c")]
		public string ListaPISCOFINS__c { get; set; }
		[XmlElement(ElementName = "CodigoFabricante__c")]
		public string CodigoFabricante__c { get; set; }
		[XmlElement(ElementName = "NomeFabricante__c")]
		public string NomeFabricante__c { get; set; }
		[XmlElement(ElementName = "UFFabricante__c")]
		public string UFFabricante__c { get; set; }
		[XmlElement(ElementName = "TransporteRefrigerado__c")]
		public string TransporteRefrigerado__c { get; set; }
		[XmlElement(ElementName = "PadraoEmbalagem__c")]
		public string PadraoEmbalagem__c { get; set; }
		[XmlElement(ElementName = "EANHistorico__c")]
		public string EANHistorico__c { get; set; }
		[XmlElement(ElementName = "RelevanteOnco")]
		public string RelevanteOnco { get; set; }
		[XmlElement(ElementName = "FabricanteDataCompra")]
		public string FabricanteDataCompra { get; set; }
		[XmlElement(ElementName = "CDCodigo")]
		public string CDCodigo { get; set; }
		[XmlElement(ElementName = "CodigoOrigem__c")]
		public string CodigoOrigem__c { get; set; }
		[XmlElement(ElementName = "DataImplantacao__c")]
		public string DataImplantacao__c { get; set; }
		[XmlElement(ElementName = "Controlado")]
		public string Controlado { get; set; }
		[XmlElement(ElementName = "CodGrupoMercadoria__c")]
		public string CodGrupoMercadoria__c { get; set; }
		[XmlElement(ElementName = "CodCondicaoEstocagem__c")]
		public string CodCondicaoEstocagem__c { get; set; }
		[XmlElement(ElementName = "ClassificacaoFiscal__c")]
		public string ClassificacaoFiscal__c { get; set; }
		[XmlElement(ElementName = "Marca")]
		public string Marca { get; set; }
	}

}