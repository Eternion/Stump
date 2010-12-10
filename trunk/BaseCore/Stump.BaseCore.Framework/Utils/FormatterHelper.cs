using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;

namespace Stump.BaseCore.Framework.Utils
{
    public static class FormatterHelper
    {
        public static byte[] SerializableObjectToBytes(object obj)
        {
            var formatter = new BinaryFormatter
                (null, new StreamingContext(StreamingContextStates.All));
            var stream = new MemoryStream();

            try
            {
                formatter.AssemblyFormat = FormatterAssemblyStyle.Simple;
                formatter.Serialize(stream, obj);

                byte[] buffer = stream.GetBuffer();
                stream.Close();

                return buffer;
            }
            catch
            {
                stream.Close();

                return null;
            }
        }

        public static T UnserializeBytesToObject<T>(byte[] bytes)
        {
            var formatter = new BinaryFormatter
                (null, new StreamingContext(StreamingContextStates.All));
            var stream = new MemoryStream(bytes);

            try
            {
                formatter.AssemblyFormat = FormatterAssemblyStyle.Simple;
                T result = (T)formatter.Deserialize(stream);

                stream.Close();

                return result;
            }
            catch
            {
                stream.Close();

                return default(T);
            }
        }

        public static T UnserializeBytesToObject<T>(byte[] bytes, SerializationBinder binder)
        {
            var formatter = new BinaryFormatter
                (null, new StreamingContext(StreamingContextStates.All));
            var stream = new MemoryStream(bytes);

            try
            {
                formatter.AssemblyFormat = FormatterAssemblyStyle.Simple;
                formatter.Binder = binder;
                T result = (T)formatter.Deserialize(stream);

                stream.Close();

                return result;
            }
            catch
            {
                stream.Close();

                return default(T);
            }
        }
    }
}