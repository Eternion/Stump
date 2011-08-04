using System.IO;
using zlib;

namespace Stump.Core.IO
{
    public class ZipHelper
    {
        public static void Compress(Stream input, Stream output)
        {
            var zoutput = new ZOutputStream(output, zlibConst.Z_DEFAULT_STRATEGY);
            var inputReader = new BinaryReader(input);

            zoutput.Write(inputReader.ReadBytes((int)input.Length), 0, (int)input.Length);
            zoutput.Flush();
        }

        public static void Uncompress(Stream input, Stream output)
        {
            var zoutput = new ZOutputStream(output);
            var inputReader = new BinaryReader(input);


            zoutput.Write(inputReader.ReadBytes((int) input.Length), 0, (int) input.Length);
            zoutput.Flush();
        }
    }
}