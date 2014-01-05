using System;

namespace AgileCalendarExample.Models.DomainModels
{
    /// <summary>
    /// Domain model for the vacation of the team member during sprints/planning
    /// </summary>
    public class Vacation : AgileItemBase
    {
        /// <summary>
        /// Team member's icon
        /// </summary>
        public String TeamMemberIcon { get; set; }

        #region IXmlSerializable Members

        public override void ReadXml(System.Xml.XmlReader reader)
        {
            base.ReadXml(reader);
            this.TeamMemberIcon = reader.GetAttribute("teamMemberIcon");
        }

        public override void WriteXml(System.Xml.XmlWriter writer)
        {
            writer.WriteStartElement("vacation");
            base.WriteXml(writer);
            writer.WriteAttributeString("teamMemberIcon", this.TeamMemberIcon);
            writer.WriteEndElement();
        }

        #endregion
    }
}