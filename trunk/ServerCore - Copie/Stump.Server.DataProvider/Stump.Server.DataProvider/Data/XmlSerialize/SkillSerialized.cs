//using System;
//using System.IO;
//using System.Xml;
//using System.Xml.Schema;
//using System.Xml.Serialization;
//using Stump.Server.WorldServer.Skills;

//namespace Stump.Server.WorldServer.XmlSerialize
//{
//    public class SkillSerialized : IXmlSerializable
//    {
//        private SkillSerialized()
//        {
            
//        }

//        public SkillSerialized(SkillBase action)
//        {
//            Action = action;
//        }

//        public SkillBase Action;

//        public XmlSchema GetSchema()
//        {
//            return null;
//        }

//        public void ReadXml(XmlReader reader)
//        {
//            Type type = Type.GetType(reader.GetAttribute("type"));
//            reader.ReadStartElement();
//            Action = (SkillBase)new XmlSerializer(type).Deserialize(reader);
//            reader.ReadEndElement();
//        }

//        public void WriteXml(XmlWriter writer)
//        {
//            writer.WriteAttributeString("type", Action.GetType().ToString());
//            new XmlSerializer(Action.GetType()).Serialize(writer, Action);
//        }

//        public static implicit operator SkillBase(SkillSerialized actionSerialized)
//        {
//            return actionSerialized.Action;
//        }

//        public static implicit operator SkillSerialized(SkillBase action)
//        {
//            return new SkillSerialized(action);
//        }

//        public static void SerializeAction(SkillBase action, string xmlFile)
//        {
//            var actionSerialized = new SkillSerialized(action);
//            using (var writer = new StreamWriter(xmlFile))
//            {
//                var serializer = new XmlSerializer(actionSerialized.GetType());
//                serializer.Serialize(writer, actionSerialized);
//            }
//        }

//        public static SkillBase DeserializeAction(string xmlFile)
//        {
//            var actionSerialized = new SkillSerialized();
//            using (var reader = new StreamReader(xmlFile))
//            {
//                var serializer = new XmlSerializer(actionSerialized.GetType());
//                actionSerialized = (SkillSerialized)serializer.Deserialize(reader);
//            }

//            return actionSerialized.Action;
//        }
//    }
//}