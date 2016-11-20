using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stump.Core.Extensions;
using Stump.Core.IO;
using Stump.DofusProtocol.Messages;
using Stump.DofusProtocol.Types;

namespace ProtocolTester
{
    class Program
    {
        static void Main(string[] args)
        {
            ProtocolTypeManager.Initialize();

            foreach (var type in typeof(Message).Assembly.GetTypes().Where(x => !x.IsAbstract && typeof(Message).IsAssignableFrom(x)))
            {
                TestType(type);
            }

        }

        public static void TestType(Type type)
        {
            foreach (var value in GetRandomValues(type))
            {
                if (value.GetType() != type)
                    continue;

                var writer = new BigEndianWriter();
                type.GetMethod("Serialize").Invoke(value, new object[] { writer });

                writer.BaseStream.Position = 0;
                var reader = new BigEndianReader(writer.BaseStream);
                type.GetMethod("Deserialize").Invoke(Activator.CreateInstance(type), new object[] { reader });

                if (reader.BytesAvailable > 0)
                {
                    Console.WriteLine("Error with type " + type);
                    return;
                }
            }
        }

        public static IEnumerable<object> GetRandomValues(Type type)
        {
            if (type.IsPrimitive)
            {
                var rand = new Random();
                if (type == typeof (int) || type == typeof(double) ||type == typeof(long) ||  type == typeof(float) ||
                   type == typeof (short) || type == typeof(long) ||type == typeof(byte))
                    yield return Convert.ChangeType(unchecked((sbyte)(rand.Next() % 127)), type);

                else if (type == typeof (string))
                    yield return rand.RandomString(12);

                else
                    yield return Activator.CreateInstance(type);
            }

            else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            {
                // empty array
                yield return Activator.CreateInstance(typeof(List<>).MakeGenericType(type.GetGenericArguments()));

                // array with 2 values
                foreach (var value in GetRandomValues(type.GetGenericArguments()[0]))
                {
                    var list = (IList)Activator.CreateInstance(typeof (List<>).MakeGenericType(type.GetGenericArguments()));
                    list.Add(value);
                    list.Add(value);
                    yield return list;
                }
            }

            else
            {
                foreach(var currentType in GetDerivedType(type))
                {
                    var ctor = currentType.GetConstructors().FirstOrDefault(x => x.GetParameters().Length > 0);

                    if (ctor == null)
                        continue;

                    foreach (var parameters in CartesianProduct(ctor.GetParameters().Select(x => GetRandomValues(x.ParameterType))))
                        yield return ctor.Invoke(parameters.ToArray());
                }
            }
        }

        public static IEnumerable<IEnumerable<T>> CartesianProduct<T>(IEnumerable<IEnumerable<T>> sequences)
        {
            IEnumerable<IEnumerable<T>> result = new[] {Enumerable.Empty<T>()};
            foreach (var sequence in sequences)
            {
                var localSequence = sequence;
                result = result.SelectMany(
                    _ => localSequence,
                    (seq, item) => seq.Concat(new[] {item})
                    );
            }
            return result;
        }

        public static IEnumerable<Type> GetDerivedType(Type baseType)
        {
            return baseType.Assembly.GetTypes().Where(x => !x.IsAbstract && x != typeof(object) && baseType.IsAssignableFrom(x));
        } 
    }
}
