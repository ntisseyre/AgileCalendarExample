
namespace AgileCalendarExample.Models.DomainModels
{
    /// <summary>
    /// Domain model for the sprint in agile
    /// </summary>
    public class Sprint : AgileItemColoredBase
    {
        #region IXmlSerializable Members

        public override void WriteXml(System.Xml.XmlWriter writer)
        {
            writer.WriteStartElement("sprint");
            base.WriteXml(writer);
            writer.WriteEndElement();
        }

        #endregion
    }
}