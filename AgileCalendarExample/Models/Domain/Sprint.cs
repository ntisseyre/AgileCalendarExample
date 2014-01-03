using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace AgileCalendarExample.Models.Domain
{
    public class Sprint : AgileItemBase
    {
        public String Color { get; set; }

        #region IXmlSerializable Members

        public virtual void ReadXml(System.Xml.XmlReader reader)
        {
            base.ReadXml(reader);
            this.Color = reader.GetAttribute("color");
        }

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