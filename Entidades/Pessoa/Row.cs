using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Ks.MonitorCPI.Entidades.Pessoa
{
	[XmlRoot(ElementName = "row")]
	public class Row
	{
		[XmlElement(ElementName = "CodigoSAP")]
		public string CodigoSAP { get; set; }
		[XmlElement(ElementName = "CodigoTOTVS__c")]
		public string CodigoTOTVS__c { get; set; }
		[XmlElement(ElementName = "Empresa")]
		public string Empresa { get; set; }
		[XmlElement(ElementName = "MarcadoEliminacao")]
		public string MarcadoEliminacao { get; set; }
		[XmlElement(ElementName = "RazaoSocial")]
		public string RazaoSocial { get; set; }
		[XmlElement(ElementName = "Grupo")]
		public string Grupo { get; set; }
		[XmlElement(ElementName = "GrupoContas")]
		public string GrupoContas { get; set; }
		[XmlElement(ElementName = "GRPCM")]
		public string GRPCM { get; set; }
		[XmlElement(ElementName = "CNPJ")]
		public string CNPJ { get; set; }
		[XmlElement(ElementName = "AreaCredito")]
		public string AreaCredito { get; set; }
		[XmlElement(ElementName = "LimiteCredito")]
		public string LimiteCredito { get; set; }
		[XmlElement(ElementName = "ContaCredito")]
		public string ContaCredito { get; set; }
		[XmlElement(ElementName = "OrganizacaoVendas")]
		public string OrganizacaoVendas { get; set; }
		[XmlElement(ElementName = "CanalDistribuicao")]
		public string CanalDistribuicao { get; set; }
		[XmlElement(ElementName = "CDCentroFornecedor")]
		public string CDCentroFornecedor { get; set; }
	}
}