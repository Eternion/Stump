using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using HtmlAgilityPack;

namespace DBSynchroniser.Http
{
    public static class MountsExplorer
    {
        public const string MOUNT_URL = "http://www.dofus.com/fr/linker/ride?l=fr";

        public static MountWebInfo GetMountWebInfo(int mountId)
        {
            var link = $"{MOUNT_URL}&id={mountId}";
            var resultat = new HtmlDocument();

            try
            {
                var http = new HttpClient();
                var task = http.GetByteArrayAsync(link);
                task.Wait();
                var response = task.Result;
                var source = Encoding.GetEncoding("utf-8").GetString(response, 0, response.Length - 1);
                source = WebUtility.HtmlDecode(source);
                resultat.LoadHtml(source);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"HTTP ERROR while exploring mount link ${link}). Exception: {ex.Message}");
                return null;
            }

            var statsDiv = resultat.DocumentNode.Descendants()
                .Where(x => x.Name == "div" && x.Attributes["class"]?.Value == "ak-title"
                            && x.ParentNode.Name == "div" && x.ParentNode.Attributes["class"]?.Value == "ak-content");

            return new MountWebInfo(mountId, statsDiv.Select(x => ParseStat(x)).ToArray());
        }

        private static MountWebStat ParseStat(HtmlNode nodeStat)
        {
            var nodeStatInfo = nodeStat.Descendants().FirstOrDefault(x => x.Name == "span" && x.Attributes["class"]?.Value == "ak-title-info");

            if (nodeStatInfo == null) //Effect
            {
                var statStr = nodeStat.InnerHtml.Trim().Split(new string[] { "  " }, StringSplitOptions.None);
                if (statStr.Count() <= 1)
                {
                    statStr = nodeStat.InnerHtml.Trim().Split('%');
                    statStr[0] += "%";
                }

                return new MountWebStat(statStr[1].Trim(), statStr[0].Trim());
            }
            else //Stat
            {
                var statStr = nodeStat.InnerHtml.Trim().Split(':');
                return new MountWebStat(statStr[0].Trim(), nodeStatInfo.InnerText.Trim());
            }
        }
    }
}