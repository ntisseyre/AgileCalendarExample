using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace AgileCalendarExample.Models.DomainModels
{
    public class Sprint : AgileItemColoredBase
    {
        #region IXmlSerializable Members

        public override void WriteXml(System.Xml.XmlWriter writer)
        {
            writer.WriteStartElement("sprint");
            base.WriteXml(writer);
            writer.WriteAttributeString("color", this.Color);
            writer.WriteEndElement();
        }

        #endregion
    }
}