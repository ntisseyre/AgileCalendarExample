using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AgileCalendarExample.Models.DomainModels
{
    public class AgileItemColoredBase : AgileItemBase
    {
        public String Color { get; set; }

        #region IXmlSerializable Members

        public override void ReadXml(System.Xml.XmlReader reader)
        {
            base.ReadXml(reader);
            this.Color = reader.GetAttribute("color");
        }

        #endregion
    }
}