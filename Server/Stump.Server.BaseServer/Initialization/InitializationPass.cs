namespace Stump.Server.BaseServer.Initialization
{
    public enum InitializationPass : byte
    {
        Database,
        Any,
        /// <summary>
        /// DiscriminatorManager
        /// </summary>
        CoreReserved,
        /// <summary>
        /// TextManager, ChatManager
        /// </summary>
        First,
        Second,
        /// <summary>
        /// BreedManager, EffectManager
        /// </summary>
        Third,
        /// <summary>
        /// ExperienceManager, InteractiveManager, ItemManager, CellTriggerManager, TinselManager
        /// </summary>
        Fourth,
        /// <summary>
        /// NpcManager
        /// </summary>
        Fifth,
        /// <summary>
        /// MonsterManager, GuildManager
        /// </summary>
        Sixth,
        /// <summary>
        /// World
        /// </summary>
        Seventh,
        /// <summary>
        /// IdProvider Synchronisation
        /// </summary>
        Eighth,
        Ninth,
        Tenth,
        Last,
    }
}