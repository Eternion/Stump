
using System;
using Stump.Database.WorldServer;
using Stump.Database.WorldServer.Character;
using Stump.DofusProtocol.Classes.Custom;

namespace Stump.Server.WorldServer.Helpers
{
    public static class CharacterManager
    {
        private const string Vowels = "aeiouy";
        private const string Consonants = "bcdfghjklmnpqrstvwxz";

        public static ExtendedLook GetStuffedCharacterLook(CharacterRecord character)
        {
            var baseLook = new ExtendedLook(character.BaseLook);

            //TODO ADD ITEMS

            return baseLook;
        }

        public static string GetRandomName()
        {
            string name;

            do
            {
                var rand = new Random();
                int namelen = rand.Next(5, 10);
                name = string.Empty;

                name += char.ToUpper(RandomConsonant(rand));

                for (int i = 0; i < namelen - 1; i++)
                {
                    name += ((i & 1) != 1) ? RandomConsonant(rand) : RandomVowel(rand);
                }
            } while (CharacterRecord.IsNameExists(name));

            return name;
        }

        private static char RandomVowel(Random rand)
        {
            return Vowels[rand.Next(0, Vowels.Length - 1)];
        }

        private static char RandomConsonant(Random rand)
        {
            return Consonants[rand.Next(0, Consonants.Length - 1)];
        }

    }
}