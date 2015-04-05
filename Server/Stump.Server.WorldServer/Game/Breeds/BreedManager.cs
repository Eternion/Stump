using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.Attributes;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Database;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer.Database.Breeds;

namespace Stump.Server.WorldServer.Game.Breeds
{
    public class BreedManager : DataManager<BreedManager>
    {
        /// <summary>
        /// List of available breeds
        /// </summary>
        [Variable]
        public readonly static List<PlayableBreedEnum> AvailableBreeds = new List<PlayableBreedEnum>
            {
                PlayableBreedEnum.Feca,
                PlayableBreedEnum.Osamodas,
                PlayableBreedEnum.Enutrof,
                PlayableBreedEnum.Sram,
                PlayableBreedEnum.Xelor,
                PlayableBreedEnum.Ecaflip,
                PlayableBreedEnum.Eniripsa,
                PlayableBreedEnum.Iop,
                PlayableBreedEnum.Cra,
                PlayableBreedEnum.Sadida,
                PlayableBreedEnum.Sacrieur,
                PlayableBreedEnum.Pandawa,
                PlayableBreedEnum.Roublard,
                PlayableBreedEnum.Zobal,
                PlayableBreedEnum.Steamer
            };

        public uint AvailableBreedsFlags
        {
            get
            {
                return (uint)AvailableBreeds.Aggregate(0, (current, breedEnum) => current | ( 1 << ((int)breedEnum - 1) ));
            }
        }

        private readonly Dictionary<int, Breed> m_breeds = new Dictionary<int, Breed>();
        private Dictionary<int, Head> m_heads = new Dictionary<int, Head>();

        [Initialization(InitializationPass.Third)]
        public override void Initialize()
        {
            base.Initialize();
            foreach (var breed in Database.Query<Breed, BreedItem, BreedSpell, Breed>(new BreedRelator().Map, BreedRelator.FetchQuery))
            {
                m_breeds.Add(breed.Id, breed);
            }
            m_heads = Database.Query<Head>(HeadRelator.FetchQuery).ToDictionary(x => x.Id);
        }

        public Breed GetBreed(PlayableBreedEnum breed)
        {
            return GetBreed((int)breed);
        }

        /// <summary>
        /// Get the breed associated to the given id
        /// </summary>
        /// <param name="id"></param>
        public Breed GetBreed(int id)
        {
            Breed breed;
            m_breeds.TryGetValue(id, out breed);

            return breed;
        }

        public Head GetHead(int id)
        {
            Head head;
            m_heads.TryGetValue(id, out head);

            return head;
        }

        public bool IsBreedAvailable(int id)
        {
            return AvailableBreeds.Contains((PlayableBreedEnum)id);
        }

        /// <summary>
        /// Add a breed instance to the database
        /// </summary>
        /// <param name="breed">Breed instance to add</param>
        /// <param name="defineId">When set to true the breed id will be auto generated</param>
        public void AddBreed(Breed breed, bool defineId = false)
        {
            if(defineId)
            {
                var id = m_breeds.Keys.Max() + 1;
                breed.Id = id;
            }

            if (m_breeds.ContainsKey(breed.Id))
                throw new Exception(string.Format("Breed with id {0} already exists", breed.Id));

            m_breeds.Add(breed.Id, breed);

            Database.Insert(breed);
        }

        /// <summary>
        /// Remove a breed from the database
        /// </summary>
        /// <param name="breed"></param>
        public void RemoveBreed(Breed breed)
        {
            RemoveBreed(breed.Id);
        }

        /// <summary>
        /// Remove a breed from the database by his id
        /// </summary>
        /// <param name="id"></param>
        public void RemoveBreed(int id)
        {
            if (!m_breeds.ContainsKey(id))
                throw new Exception(string.Format("Breed with id {0} does not exist", id));

            // it's safer to delete the breed in the dictionary first next in the database
            var breed = m_breeds[id];
            m_breeds.Remove(id);

            Database.Delete(breed);
        }
    }
}
