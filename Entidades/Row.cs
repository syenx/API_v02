using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Ks.MonitorCPI.Entidades
{
    [XmlRoot(ElementName = "row")]
    public class Row
    {
        [XmlElement(ElementName = "TABLE_ID")]
        public string TABLE_ID { get; set; }
        [XmlElement(ElementName = "BATCH_ID")]
        public string BATCH_ID { get; set; }
        [XmlElement(ElementName = "MESSAGE")]
        public string MESSAGE { get; set; }

        [XmlElement(ElementName = "PROC_DATE_ENVIO")]
        public DateTime PROC_DATE_ENVIO { get; set; }
        [XmlElement(ElementName = "PROC_DATE_RETORNO")]
        public DateTime PROC_DATE_RETORNO { get; set; }
        [XmlElement(ElementName = "STATUS")]
        public string STATUS { get; set; }



    }
}