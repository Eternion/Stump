
using System.Linq;
using Stump.Server.BaseServer.Manager;

namespace Stump.Server.WorldServer.Fights
{
    public class FightManager : InstanceManager<Fight>
    {
        /// <summary>
        ///   Remove the fight
        /// </summary>
        /// <param name = "fight"></param>
        /// <returns></returns>
        public static bool RemoveFight(Fight fight)
        {
            fight.Dispose();

            return RemoveInstance(fight);
        }

        /// <summary>
        ///   Create the fight
        /// </summary>
        /// <param name = "fight"></param>
        /// <returns>fightId</returns>
        public static int CreateFight(Fight fight)
        {
            return CreateInstance(fight);
        }

        #region GetFight Methods

        public static Fight GetFightById(int fightId)
        {
            return GetInstanceById(fightId);
        }

        public static Fight GetFightByCharacterId(int chrId)
        {
            return (from entry in m_instances.Values
                    where entry.SourceGroup.GetMemberById(chrId) != null || entry.TargetGroup.GetMemberById(chrId) != null
                    select entry).FirstOrDefault();
        }

        public static Fight GetFightByGroupId(int groupId)
        {
            return ( from entry in m_instances.Values
                     where entry.SourceGroup.Id == groupId || entry.TargetGroup.Id == groupId
                     select entry ).FirstOrDefault();
        }

        #endregion
    }
}