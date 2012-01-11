using System;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using Stump.Tools.MonsterDataFinder.Data;

namespace Stump.Tools.MonsterDataFinder.Readers
{
    public class MonsterDataReader
    {
        private const string URI = "http://www.dofus.com/fr/apidofus/monster?id={0}&type=xml";

        public MonsterDataReader()
        {
            
        }

        public Monster Request(int monsterId)
        {
            var serializer = new DataContractJsonSerializer(typeof(MonsterJSON));
            var request = WebRequest.Create(string.Format(URI, monsterId)) as HttpWebRequest;

            if (request == null)
                throw new Exception("request == null");

            request.Timeout = 60 * 1000;

            var rep = request.GetResponse();
            var stream = rep.GetResponseStream();

            var buffer = new byte[0x8000];
            var len = 0;
            int y;
            while (( y = stream.Read(buffer, len, 0x400) ) != 0)
            {
                len += y;
            }

            var data = Encoding.UTF8.GetString(buffer, 0, len);

            if (!data.Contains("monster"))
                return null;

            data = data.Substring(data.IndexOf("{"));

            using (var dataStream = new MemoryStream(Encoding.UTF8.GetBytes(data)))
            {
                var jsonObj = serializer.ReadObject(dataStream) as MonsterJSON;

                if (jsonObj == null)
                    return null;

                if (jsonObj.monster.id == 0)
                    return null;

                return jsonObj.monster;
            }
        }
    }
}