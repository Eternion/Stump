namespace Stump.Server.BaseServer.Initialization
{
    public enum InitializationPass : byte
    {
        Any,
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
        /// ExperienceManager, InteractiveManager, ItemManager, CellTriggerManager
        /// </summary>
        Fourth,
        /// <summary>
        /// NpcManager
        /// </summary>
        Fifth,
        /// <summary>
        /// MonsterManager
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