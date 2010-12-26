using System;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Stump.Server.WorldServer.Actions
{
    public class ActionSerialized : IXmlSerializable
    {
        private ActionSerialized()
        {
            
        }

        public ActionSerialized(ActionBase action)
        {
            Action = action;
        }

        public ActionBase Action;

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            Type type = Type.GetType(reader.GetAttribute("type"));
            reader.ReadStartElement();
            Action = (ActionBase)new XmlSerializer(type).Deserialize(reader);
            reader.ReadEndElement();
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("type", Action.GetType().ToString());
            new XmlSerializer(Action.GetType()).Serialize(writer, Action);
        }

        public static implicit operator ActionBase(ActionSerialized actionSerialized)
        {
            return actionSerialized.Action;
        }

        public static implicit operator ActionSerialized(ActionBase action)
        {
            return new ActionSerialized(action);
        }

        public static void SerializeAction(ActionBase action, string xmlFile)
        {
            var actionSerialized = new ActionSerialized(action);
            using (var writer = new StreamWriter(xmlFile))
            {
                var serializer = new XmlSerializer(actionSerialized.GetType());
                serializer.Serialize(writer, actionSerialized);
            }
        }

        public static ActionBase DeserializeAction(string xmlFile)
        {
            var actionSerialized = new ActionSerialized();
            using (var reader = new StreamReader(xmlFile))
            {
                var serializer = new XmlSerializer(actionSerialized.GetType());
                actionSerialized = (ActionSerialized) serializer.Deserialize(reader);
            }

            return actionSerialized.Action;
        }
    }
}