using System.Collections.Generic;
using System.Linq;
using Stump.Core.IO;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;
using Stump.Server.WorldServer.Database.Items.Templates;
using Stump.Server.WorldServer.Game.Items;

namespace Stump.Server.WorldServer.Database.Jobs
{
    public static class SecretRecipeRelator
    {
        public static string FetchQuery = "SELECT * FROM recipes_secret";
    }

    [TableName("recipes_secret")]
    [D2OIgnore]
    public class SecretRecipeRecord : RecipeRecord
    {
      
    }
}