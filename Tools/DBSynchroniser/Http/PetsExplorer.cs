using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using Stump.Server.WorldServer.Database.Items.Pets;

namespace DBSynchroniser.Http
{
    public static class PetsExplorer
    {
        public const int MIN_PETS_COUNT = 200;
        public const string DOFUS_URL = "http://www.dofus.com/";

        public static string[] FetchPetsLinks()
        {
            var resultat = new HtmlDocument();

            try
            {
                var http = new HttpClient();
                var query = http.GetByteArrayAsync("http://www.dofus.com/fr/mmorpg/encyclopedie/familiers?size=" + MIN_PETS_COUNT);
                query.Wait();
                var response = query.Result;
                var source = Encoding.GetEncoding("utf-8").GetString(response, 0, response.Length - 1);
                source = WebUtility.HtmlDecode(source);
                resultat.LoadHtml(source);
            }
            catch (Exception ex)
            {
                Console.WriteLine("HTTP ERROR while exploring pets list). Exception: {0}", ex.Message);
                return new string[0];
            }

            var petsLinks = resultat.DocumentNode.Descendants()
                .Where(x => x.Name == "a"
                            && x.Attributes["href"] != null
                            && x.Attributes["href"].Value.StartsWith("/fr/mmorpg/encyclopedie/familiers/")
                            && x.ParentNode.Name == "span"
                            && x.ParentNode.Attributes["class"] != null
                            && x.ParentNode.Attributes["class"].Value.Contains("ak-linker")).Select(x => DOFUS_URL + x.Attributes["href"].Value).Distinct().ToArray();

            return petsLinks;
        }

        public static PetWebInfo GetPetWebInfo(string link)
        {
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
                Console.WriteLine($"HTTP ERROR while exploring pet link ${link}). Exception: {ex.Message}");
                return null;
            }

            var id = int.Parse(Regex.Match(link, @"familiers/(\d+)-").Groups[1].Value);


            var foodsDiv = resultat.DocumentNode.Descendants()
                .Where(x => x.Name == "div" && x.Attributes["class"]?.Value == "ak-list-element"
                            && x.ParentNode.Name == "div" && x.ParentNode.Attributes["class"]?.Value == "ak-container ak-content-list ak-food-effects-list ");

            var ghostNode = resultat.DocumentNode
                .Descendants().FirstOrDefault(x => x.Name == "a" && (x.Attributes["href"]?.Value.Contains("fantome") ?? false));

            int? ghost = null;
            if (ghostNode != null)
                ghost = int.Parse(Regex.Match(ghostNode.Attributes["href"].Value, @"(\d+)-").Groups[1].Value);


            var foods = (from div in foodsDiv
                from a in div.Descendants().Where(x => x.Name == "a")
                select ParseFood(div, a)).ToArray();

            return new PetWebInfo(id, foods, ghost);
        }

        private static PetWebFood ParseFood(HtmlNode divNode, HtmlNode aNode)
        {
            var bonusStrs = divNode.Descendants().Where(x => x.Name == "strong").Select(x => x.InnerText).First().Split(','); // + 1 Agilité

            var bonusQuantities = bonusStrs.Select(x => int.Parse(Regex.Match(x, @"(\d+)").Groups[1].Value)).ToArray();
            var bonusEffects = bonusStrs.Select(x => new string(x.ToCharArray().Where(c => !char.IsDigit(c)).ToArray()).Trim()).ToArray();

            FoodTypeEnum type;
            if (aNode.Attributes["href"].Value.Contains("monstres"))
                type = FoodTypeEnum.MONSTER;
            else if (aNode.Attributes["href"].Value.Contains("type_id"))
                type = FoodTypeEnum.ITEMTYPE;
            else
                type = FoodTypeEnum.ITEM;

            var id = int.Parse(Regex.Match(aNode.Attributes["href"].Value, @"(\d+)").Groups[1].Value);

            var spanNode = aNode.ParentNode;

            int quantity;
            if (spanNode.PreviousSibling == null || !int.TryParse(Regex.Match(spanNode.PreviousSibling.InnerText, @"(\d+)").Groups[1].Value, out quantity))
                quantity = 1;

            return new PetWebFood(bonusEffects, bonusQuantities, type, id, quantity);
        }
    }
}