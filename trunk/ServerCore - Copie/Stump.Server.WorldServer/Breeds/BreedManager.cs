
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Stump.Database.Data.Breeds;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Initializing;

namespace Stump.Server.WorldServer.Breeds
{
    public static class BreedManager
    {
        /// <summary>
        ///   List containing all breeds.
        /// </summary>
        private static Dictionary<int, BreedRecord> _breeds = new Dictionary<int, BreedRecord>();

        /// <summary>
        ///   Load breeds data from database.
        ///   Called once on World Initialization process.
        /// </summary>
        [StageStep(Stages.Two, "Loaded Breeds")]
        public static void LoadBreedsData()
        {
            _breeds = BreedRecord.FindAll().ToDictionary(entry => entry.Id);
        }

        #region BreedEnum Getter

        public static BreedRecord GetBreed(PlayableBreedEnum breed)
        {
            return _breeds[(int)breed];
        }

        public static BreedRecord GetBreed(int breed)
        {
            return _breeds[breed];
        }

        public static uint BreedsToFlag(IEnumerable<PlayableBreedEnum> breeds)
        {
            return (uint)breeds.Aggregate(0, (current, breedEnum) => current | (1 << (int)breedEnum));
        }

        #endregion
    }
}