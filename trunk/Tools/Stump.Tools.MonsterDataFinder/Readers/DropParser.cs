using System.Collections.Generic;

namespace Stump.Tools.MonsterDataFinder.Readers
{
    public class DropParser
    {
        private const string IDENTIFIER = "DofusLinker.Item(this,";
        private const string IDENTIFIER2 = "http://staticns.ankama.com/dofus/www/img/encyclopedia/monster/droppeur.gif";

        public Drop[] Parse(string dropString)
        {
            var drops = new List<Drop>();

            while (dropString.Contains(IDENTIFIER))
            {
                var drop = new Drop();

                dropString = dropString.Substring(dropString.IndexOf(IDENTIFIER) + IDENTIFIER.Length);
                drop.Id = int.Parse(dropString.Substring(0, dropString.IndexOf(")")));

                dropString = dropString.Substring(dropString.IndexOf(">") + 1);
                drop.Name = dropString.Substring(0, dropString.IndexOf("<"));

                dropString = dropString.Substring(dropString.IndexOf("(") + 1);
                drop.Percent = double.Parse(dropString.Substring(0, dropString.IndexOf("%")).Replace('.', ','));

                var ppLine = dropString.IndexOf(IDENTIFIER2);
                var next = dropString.IndexOf(IDENTIFIER);

                while (ppLine != -1 && ( ppLine < next || next == -1 ))
                {
                    dropString = dropString.Substring(dropString.IndexOf(IDENTIFIER2) + IDENTIFIER2.Length);
                    drop.Threshold += 100;
                    ppLine = dropString.IndexOf(IDENTIFIER2);
                    next = dropString.IndexOf(IDENTIFIER);
                }
                drops.Add(drop);
            }

            return drops.ToArray();
        }
    }
}