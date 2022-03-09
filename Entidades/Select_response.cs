using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Ks.MonitorCPI.Entidades
{
	[XmlRoot(ElementName = "select_response")]
	public class Select_response
	{
		[XmlElement(ElementName = "row")]
		public List<Row> Row { get; set; }
	}

}