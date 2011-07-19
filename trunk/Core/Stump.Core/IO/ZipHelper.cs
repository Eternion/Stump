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

        private static void CopyStream(Stream input, Stream output)
        {
            var buffer = new byte[1024];
            int len;

            while ((len = input.Read(buffer, 0, 1024)) > 0)
            {
                output.Write(buffer, 0, len);
            }

            output.Flush();
        }
    }
}