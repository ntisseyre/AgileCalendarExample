using System;

namespace AgileCalendarExample.Models.DomainModels
{
    /// <summary>
    /// Domain model for any agile item with custom color setting
    /// </summary>
    public class AgileItemColoredBase : AgileItemBase
    {
        /// <summary>
        /// Color name for the agile item.
        /// For example, "sprint 2" is of red color
        /// </summary>
        public String Color { get; set; }

        #region IXmlSerializable Members

        public override void ReadXml(System.Xml.XmlReader reader)
        {
            base.ReadXml(reader);
            this.Color = reader.GetAttribute("color");
        }

        public override void WriteXml(System.Xml.XmlWriter writer)
        {
            base.WriteXml(writer);
            writer.WriteAttributeString("color", this.Color);
        }

        #endregion
    }
}