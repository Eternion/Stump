// /*************************************************************************
//  *
//  *  Copyright (C) 2010 - 2011 Stump Team
//  *
//  *  This program is free software: you can redistribute it and/or modify
//  *  it under the terms of the GNU General Public License as published by
//  *  the Free Software Foundation, either version 3 of the License, or
//  *  (at your option) any later version.
//  *
//  *  This program is distributed in the hope that it will be useful,
//  *  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  *  GNU General Public License for more details.
//  *
//  *  You should have received a copy of the GNU General Public License
//  *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//  *
//  *************************************************************************/
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