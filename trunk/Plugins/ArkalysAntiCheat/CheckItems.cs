using System.Linq;
using NLog;
using Stump.ORM;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer;
using Stump.Server.WorldServer.Database.Characters;
using Stump.Server.WorldServer.Database.Items;
using Stump.Server.WorldServer.Game;
using Stump.Server.WorldServer.Game.Accounts;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace ArkalysAntiCheat
{
    public class CheckItems
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        [Initialization(InitializationPass.Database)]
        public void Initialize()
        {
            WorldServer.Instance.IOTaskPool.AddMessage(
                () =>
                {
                    var characters = World.Instance.Database.Fetch<CharacterRecord>(string.Format(CharacterRelator.FetchQuery));
                    
                    foreach (var character in characters)
                    {
                        if (character.Kamas >= 150000)
                            World.Instance.Database.Delete(character);
                        else
                        {
                            var items = World.Instance.Database.Fetch<PlayerItemRecord>(string.Format(PlayerItemRelator.FetchByOwner, character.Id));
                            var orbeCount = items.Where(item => item.Template.Id == 20000).Aggregate<PlayerItemRecord, long>(0, (current, item) => current + item.Stack);

                            if (orbeCount >= 25000)
                                World.Instance.Database.Delete(character);
                        }
                    }
                });
        }
    }
}
