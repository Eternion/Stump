using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using NLog;
using Stump.DofusProtocol.Classes;
using Stump.DofusProtocol.D2oClasses;
using Stump.Server.WorldServer.Entities;
using Stump.Server.WorldServer.Skills;

namespace Stump.Server.WorldServer.Global.Maps
{
    public class InteractiveObject
    {
        protected static Logger logger = LogManager.GetCurrentClassLogger();

        private InteractiveObject()
        {
            
        }

        public InteractiveObject(uint elementId, int elementType, Dictionary<uint, SkillBase> skills)
        {
            ElementId = elementId;
            ElementType = elementType;
            Skills = skills;
        }

        public InteractiveObject(uint elementId, int elementType, Dictionary<uint, SkillBase> skills, CellLinked cell)
        {
            ElementId = elementId;
            ElementType = elementType;
            Skills = skills;
            Cell = cell;
        }


        [XmlIgnore]
        public CellLinked Cell
        {
            get;
            set;
        }

        public uint ElementId
        {
            get;
            private set;
        }

        public int ElementType
        {
            get;
            private set;
        }

        [XmlIgnore]
        public Dictionary<uint, SkillBase> Skills
        {
            get;
            private set;
        }

        public SkillBase GetSkill(uint skillId)
        {
            return Skills.ContainsKey(skillId) ? Skills[skillId] : null;
        }

        public void ExecuteSkill(Character executer, uint skillId)
        {
            try
            {
                if (!Skills.ContainsKey(skillId) || !Skills[skillId].IsEnabled(executer))
                    throw new Exception(string.Format("Skill {0} is not implemented", skillId));

                Skills[skillId].Execute(this, executer);
            }
            catch (Exception e)
            {
                logger.Error("Can't execute skill '{0}' : {1}", skillId, e.Message);
            }
        }

        private InteractiveElement m_cachedElement;
        public InteractiveElement ToNetworkElement(WorldClient client)
        {
            if (m_cachedElement != null)
                return m_cachedElement;

            var enabledSkills = new List<InteractiveElementSkill>();
            var disabledSkills = new List<InteractiveElementSkill>();

            foreach (var skillBase in Skills)
            {
                if (skillBase.Value.IsEnabled(client.ActiveCharacter))
                {
                    enabledSkills.Add(new InteractiveElementSkill(skillBase.Value.SkillId, skillBase.Key));
                }
                else
                {
                    disabledSkills.Add(new InteractiveElementSkill(skillBase.Value.SkillId, skillBase.Key));
                }
            }
            
            return m_cachedElement = new InteractiveElement(ElementId, ElementType, enabledSkills, disabledSkills);
        }
    }
}