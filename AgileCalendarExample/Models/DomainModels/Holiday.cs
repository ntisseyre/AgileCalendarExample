using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace AgileCalendarExample.Models.DomainModels
{
    public class Holiday : AgileItemColoredBase
    {
        #region IXmlSerializable Members

        public override void WriteXml(System.Xml.XmlWriter writer)
        {
            writer.WriteStartElement("holiday");
            base.WriteXml(writer);
            writer.WriteEndElement();
        }

        #endregion
    }
}