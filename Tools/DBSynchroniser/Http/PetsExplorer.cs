using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace DBSynchroniser.Http
{
    public static class PetsExplorer
    {
        public const int MIN_PETS_COUNT = 200;
        public const string DOFUS_URL = "http://www.dofus.com/";

        public static async Task<IEnumerable<string>> EnumeratePetsLinks()
        {
            var resultat = new HtmlDocument();

            try
            {
                var http = new HttpClient();
                var response = await http.GetByteArrayAsync("http://www.dofus.com/fr/mmorpg/encyclopedie/familiers?size=" + MIN_PETS_COUNT);
                var source = Encoding.GetEncoding("utf-8").GetString(response, 0, response.Length - 1);
                source = WebUtility.HtmlDecode(source);
                resultat.LoadHtml(source);
            }
            catch (Exception ex)
            {
                Console.WriteLine("HTTP ERROR while exploring pets list). Exception: {0}", ex.Message);
                return Enumerable.Empty<string>();
            }

            var petsLinks = resultat.DocumentNode.Descendants()
                .Where(x => x.Name == "a"
                            && x.Attributes["href"] != null
                            && x.Attributes["href"].Value.StartsWith("/fr/mmorpg/encyclopedie/familiers/")
                            && x.ParentNode.Name == "span"
                            && x.ParentNode.Attributes["class"] != null
                            && x.ParentNode.Attributes["class"].Value.Contains("ak-linker")).Select(x => DOFUS_URL + x.Attributes["href"].Value).ToList();

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

            var foods = (from div in foodsDiv
                from a in div.Descendants().Where(x => x.Name == "a")
                select ParseFood(div, a)).ToArray();

            return new PetWebInfo(id, foods);
        }

        private static PetWebFood ParseFood(HtmlNode divNode, HtmlNode aNode)
        {
            var bonusStr = divNode.Descendants().Where(x => x.Name == "strong").Select(x => x.InnerText).First(); // + 1 Agilité

            var bonusQuantity = int.Parse(Regex.Match(bonusStr, @"(\d+)").Groups[1].Value);

            FoodType type;
            if (aNode.Attributes["href"].Value.Contains("monstres"))
                type = FoodType.Monster;
            else if (aNode.Attributes["href"].Value.Contains("type_id"))
                type = FoodType.ItemType;
            else
                type = FoodType.Item;

            var id = int.Parse(Regex.Match(aNode.Attributes["href"].Value, @"(\d+)").Groups[1].Value);

            var spanNode = aNode.ParentNode;

            int quantity = spanNode.PreviousSibling == null ? 1 : int.Parse(Regex.Match(spanNode.PreviousSibling.InnerText, @"(\d+)").Groups[1].Value);

            return new PetWebFood(bonusStr, bonusQuantity, type, id, quantity);
        }
    }
}