using System.Diagnostics.Contracts;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;

namespace Stump.Core.IO
{
    public static class FormatterExtensions
    {
        public static byte[] ToBinary(this object obj)
        {
            var formatter = new BinaryFormatter
                (null, new StreamingContext(StreamingContextStates.All));
            using (var stream = new MemoryStream())
            {
                formatter.AssemblyFormat = FormatterAssemblyStyle.Simple;
                formatter.Serialize(stream, obj);

                return stream.ToArray();
            }
        }

        public static T ToObject<T>(this byte[] bytes)
        {
            var formatter = new BinaryFormatter
                (null, new StreamingContext(StreamingContextStates.All));
            using (var stream = new MemoryStream(bytes))
            {
                formatter.AssemblyFormat = FormatterAssemblyStyle.Simple;

                return (T)formatter.Deserialize(stream);
            }
        }

        public static T ToObject<T>(this byte[] bytes, SerializationBinder binder)
        {
            var formatter = new BinaryFormatter
                (null, new StreamingContext(StreamingContextStates.All));
            using (var stream = new MemoryStream(bytes))
            {
                formatter.AssemblyFormat = FormatterAssemblyStyle.Simple;
                formatter.Binder = binder;

                return (T)formatter.Deserialize(stream);
            }
        }
    }
}