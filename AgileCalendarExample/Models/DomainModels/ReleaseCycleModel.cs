using AgileCalendarExample.App_GlobalResources;
using AgileCalendarExample.CalendarDomainLogic;
using AgileCalendarExample.Models.ViewModels.Agile;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace AgileCalendarExample.Models.DomainModels
{
    /// <summary>
    /// Class represents an agile release cycle
    /// </summary>
    [XmlRoot(ElementName = "releaseCycle", Namespace = "urn:supperslonic:agileCalendar")]
    public class ReleaseCycleModel : IXmlSerializable
    {
        public static readonly CultureInfo cultureInfo = new CultureInfo(Resources.CultureInfo);
        private static readonly XmlSerializer serializer = new XmlSerializer(typeof(ReleaseCycleModel));

        /// <summary>
        /// Planning during the release cycle
        /// </summary>
        public Planning Planning { get; set; }

        /// <summary>
        /// List of sprints during the release cycle
        /// </summary>
        public IList<Sprint> Sprints { get; set; }

        /// <summary>
        /// List of holidays during the release cycle
        /// </summary>
        public IList<Holiday> Holidays { get; set; }

        /// <summary>
        /// List of vacations during the release cycle
        /// </summary>
        public IList<Vacation> Vacations { get; set; }

        /// <summary>
        /// Sorts all the collections by startDate
        /// </summary>
        /// <returns>A pointer to the same object after the normolization</returns>
        public ReleaseCycleModel Normolize()
        {
            this.Sprints = this.Sprints.OrderBy(item => item.StartDate).ToList();
            this.Holidays = this.Holidays.OrderBy(item => item.StartDate).ToList();
            this.Vacations = this.Vacations.OrderBy(item => item.StartDate).ToList();

            return this;
        }

        #region Serialization Support

        public static ReleaseCycleModel GetRecord(string xml)
        {
            using (TextReader reader = new StringReader(xml))
            {
                ReleaseCycleModel result = serializer.Deserialize(reader) as ReleaseCycleModel;
                return result;
            }
        }

        public override string ToString()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ReleaseCycleModel));

            using (TextWriter writer = new StringWriter(ReleaseCycleModel.cultureInfo))
            {
                serializer.Serialize(writer, this);
                return writer.ToString();
            }
        }

        #endregion

        #region IXmlSerializable Members

        public XmlSchema GetSchema()
        {
            Assembly currentAssembly = Assembly.GetAssembly(typeof(ReleaseCycleModel));
            using (Stream xsdStream = currentAssembly.GetManifestResourceStream(currentAssembly.GetName() + ".AgileCalendar.xsd"))
                return XmlSchema.Read(xsdStream, null);
        }

        public void ReadXml(XmlReader reader)
        {
            this.Planning = new Planning();
            this.Sprints = new List<Sprint>();
            this.Holidays = new List<Holiday>();
            this.Vacations = new List<Vacation>();

            string startNodeName = reader.Name;
            while (reader.Read())
            {
                if (!reader.IsStartElement())
                {
                    if (startNodeName == reader.Name)
                    {
                        reader.Read();//Set pointer to the next node
                        return;
                    }
                    else
                    {
                        continue;
                    }
                }

                switch (reader.Name)
                {
                    case "planning":
                        this.Planning.ReadXml(reader);
                        break;

                    case "sprint":
                        Sprint sprint = new Sprint();
                        sprint.ReadXml(reader);
                        this.Sprints.Add(sprint);
                        break;

                    case "holiday":
                        Holiday holiday = new Holiday();
                        holiday.ReadXml(reader);
                        this.Holidays.Add(holiday);
                        break;

                    case "vacation":
                        Vacation vacation = new Vacation();
                        vacation.ReadXml(reader);
                        this.Vacations.Add(vacation);
                        break;

                    default:
                        throw new InvalidOperationException(string.Format(ReleaseCycleModel.cultureInfo, Resources.E_UnexpectedXmlElementWithParam, reader.Name));
                }
            }
        }

        public void WriteXml(XmlWriter writer)
        {
            this.Planning.WriteXml(writer);
            this.WriteXmlForList(writer, this.Sprints);
            this.WriteXmlForList(writer, this.Holidays);
            this.WriteXmlForList(writer, this.Vacations);
        }

        private void WriteXmlForList(XmlWriter writer, IEnumerable<AgileItemBase> list)
        {
            foreach (AgileItemBase item in this.Sprints)
                item.WriteXml(writer);
        }

        #endregion
    }
}