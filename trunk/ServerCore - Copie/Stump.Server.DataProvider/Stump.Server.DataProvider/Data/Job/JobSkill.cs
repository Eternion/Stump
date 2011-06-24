
using System.Collections.Generic;
using Stump.DofusProtocol.Enums;

namespace Stump.Server.DataProvider.Data.Job
{
    public class JobSkill
    {
        public int SkillId { get; set; }

        public JobTemplate Job { get; set; }

        public int InteractiveId { get; set; }

        public bool IsForgemagus { get; set; }

        public bool IsRepair { get; set; }

        public bool AvailableInHouse { get; set; }

        public ItemTypeEnum ModifiableItemType { get; set; }

        public int GatheredRessource { get; set; }

        public List<int> CraftableItemIds { get; set; }
    }
}