
namespace AgileCalendarExample.Models.DomainModels
{
    /// <summary>
    /// Domain model for the planning in agile
    /// </summary>
    public class Planning : AgileItemColoredBase
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