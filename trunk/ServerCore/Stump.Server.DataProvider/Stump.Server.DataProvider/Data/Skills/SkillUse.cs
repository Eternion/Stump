using System;
using Stump.Server.DataProvider.Data.Actions;
using Stump.Server.WorldServer.Actions;
using Stump.Server.WorldServer.Entities;
using Stump.Server.WorldServer.Global.Maps;
using Stump.Server.WorldServer.XmlSerialize;

namespace Stump.Server.WorldServer.Skills
{
    /// <summary>
    /// Skill for all interactive object without specifical actions like a door
    /// </summary>
    public class SkillUse : SkillBase
    {
        private SkillUse()
        {
            
        }

        public SkillUse(uint duration = 0u, string condition = "", params ActionSerialized[] actions)
        {
            Duration = duration;
            Condition = condition;
            ActionsSerialized = actions;
        }

        public override uint SkillId
        {
            get { return 184; }
        }

        public ActionSerialized[] ActionsSerialized;

        public string Condition = "";

        public override bool IsEnabled(Character executer)
        {
            // todo : use condition to determin if this skill is enabled

            return true;
        }

        public override void Execute(InteractiveObject interactiveObject, Character executer)
        {
            foreach (var actionBase in ActionsSerialized)
            {
                ActionBase.ExecuteAction(actionBase, interactiveObject, executer);
            }
        }
    }
}