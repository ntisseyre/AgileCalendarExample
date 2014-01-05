
namespace AgileCalendarExample.Models.DomainModels
{
    /// <summary>
    /// Domain model for the holiday during sprints/planning
    /// </summary>
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