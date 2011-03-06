//using System;
//using System.IO;
//using System.Xml;
//using System.Xml.Schema;
//using System.Xml.Serialization;

//namespace Stump.Server.WorldServer.Npcs
//{
//    public class NpcStartActionSerialized : IXmlSerializable
//    {
//        private NpcStartActionSerialized()
//        {
            
//        }

//        public NpcStartActionSerialized(NpcStartAction action)
//        {
//            Action = action;
//        }

//        public NpcStartAction Action;

//        public XmlSchema GetSchema()
//        {
//            return null;
//        }

//        public void ReadXml(XmlReader reader)
//        {
//            Type type = Type.GetType(reader.GetAttribute("type"));
//            reader.ReadStartElement();
//            Action = (NpcStartAction)new XmlSerializer(type).Deserialize(reader);
//            reader.ReadEndElement();
//        }

//        public void WriteXml(XmlWriter writer)
//        {
//            writer.WriteAttributeString("type", Action.GetType().ToString());
//            new XmlSerializer(Action.GetType()).Serialize(writer, Action);
//        }

//        public static implicit operator NpcStartAction(NpcStartActionSerialized actionSerialized)
//        {
//            return actionSerialized.Action;
//        }

//        public static implicit operator NpcStartActionSerialized(NpcStartAction action)
//        {
//            return new NpcStartActionSerialized(action);
//        }

//        public static void SerializeAction(NpcStartAction action, string xmlFile)
//        {
//            var actionSerialized = new NpcStartActionSerialized(action);
//            using (var writer = new StreamWriter(xmlFile))
//            {
//                var serializer = new XmlSerializer(actionSerialized.GetType());
//                serializer.Serialize(writer, actionSerialized);
//            }
//        }

//        public static NpcStartAction DeserializeAction(string xmlFile)
//        {
//            var actionSerialized = new NpcStartActionSerialized();
//            using (var reader = new StreamReader(xmlFile))
//            {
//                var serializer = new XmlSerializer(actionSerialized.GetType());
//                actionSerialized = (NpcStartActionSerialized)serializer.Deserialize(reader);
//            }

//            return actionSerialized.Action;
//        }
//    }
//}