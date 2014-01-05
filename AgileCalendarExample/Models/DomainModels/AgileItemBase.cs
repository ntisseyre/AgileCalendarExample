using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace AgileCalendarExample.Models.DomainModels
{
    public abstract class AgileItemBase : IXmlSerializable
    {
        public String Name { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        #region IXmlSerializable Members

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            Assembly currentAssembly = Assembly.GetAssembly(typeof(ReleaseCycleModel));
            using (Stream xsdStream = currentAssembly.GetManifestResourceStream(currentAssembly.GetName() + ".AgileCalendar.xsd"))
                return XmlSchema.Read(xsdStream, null);
        }

        public virtual void ReadXml(System.Xml.XmlReader reader)
        {
            this.Name = reader.GetAttribute("name");
            this.StartDate = DateTime.ParseExact(reader.GetAttribute("start"), "yyyy-MM-dd", null);
            this.EndDate = DateTime.ParseExact(reader.GetAttribute("end"), "yyyy-MM-dd", null);
        }

        public virtual void WriteXml(System.Xml.XmlWriter writer)
        {
            writer.WriteAttributeString("name", this.Name);
            writer.WriteAttributeString("start", this.StartDate.ToString("yyyy-MM-dd"));
            writer.WriteAttributeString("end", this.EndDate.ToString("yyyy-MM-dd"));
        }

        #endregion
    }
}