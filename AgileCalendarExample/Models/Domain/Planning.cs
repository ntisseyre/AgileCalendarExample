using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace AgileCalendarExample.Models.Domain
{
    public class Planning : AgileItemBase
    {
        #region IXmlSerializable Members

        public override void WriteXml(System.Xml.XmlWriter writer)
        {
            writer.WriteStartElement("planning");
            base.WriteXml(writer);
            writer.WriteEndElement();
        }

        #endregion
    }
}