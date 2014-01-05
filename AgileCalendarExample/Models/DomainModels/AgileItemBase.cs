using System;
using System.IO;
using System.Reflection;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace AgileCalendarExample.Models.DomainModels
{
    /// <summary>
    /// Domain model for any agile item
    /// </summary>
    public abstract class AgileItemBase : IXmlSerializable
    {
        /// <summary>
        /// Name, for example "sprint 2"
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// The first date of the agile item.
        /// For example, "sprint 2" starts on the 1st of November 2014
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// The last date of the agile item.
        /// For example, "sprint 2" ends on the 10th of November 2014
        /// </summary>
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