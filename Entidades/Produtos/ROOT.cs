using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Ks.MonitorCPI.Entidades.Produtos
{
	[XmlRoot(ElementName = "ROOT")]
	public class ROOT
	{
		[XmlElement(ElementName = "select_response")]
		public Select_response Select_response { get; set; }
	}
}