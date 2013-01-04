#region License GNU GPL
// ParameterizableRecord.cs
// 
// Copyright (C) 2012 - BehaviorIsManaged
// 
// This program is free software; you can redistribute it and/or modify it 
// under the terms of the GNU General Public License as published by the Free Software Foundation;
// either version 2 of the License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; 
// without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 
// See the GNU General Public License for more details. 
// You should have received a copy of the GNU General Public License along with this program; 
// if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
#endregion

using System;
using System.Text;
using Stump.Core.Extensions;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;

namespace Stump.Server.BaseServer.Database
{
    public abstract class ParameterizableRecord
    {
        public ParameterizableRecord()
        {
            ParameterSeparatorChar = '|';
        }

        [Ignore]
        public char ParameterSeparatorChar
        {
            get;
            set;
        }

        [NullString]
        public string Parameter0
        {
            get;
            set;
        }

        [NullString]
        public string Parameter1
        {
            get;
            set;
        }

        [NullString]
        public string Parameter2
        {
            get;
            set;
        }

        [NullString]
        public string Parameter3
        {
            get;
            set;
        }

        [NullString]
        public string Parameter4
        {
            get;
            set;
        }

        /// <summary>
        /// Comma separated value
        /// </summary>
        [NullString]
        public string AdditionalParameters
        {
            get;
            set;
        }

        public T GetParameter<T>(uint parameter, bool defaultIfEmpty = false)
            where T : IConvertible
        {
            if (parameter <= 4)
            {
                switch (parameter)
                {
                    case 0:
                        return GetParameterInteral<T>(parameter, Parameter0, defaultIfEmpty);
                    case 1:
                        return GetParameterInteral<T>(parameter, Parameter1, defaultIfEmpty);
                    case 2:
                        return GetParameterInteral<T>(parameter, Parameter2, defaultIfEmpty);
                    case 3:
                        return GetParameterInteral<T>(parameter, Parameter3, defaultIfEmpty);
                    case 4:
                        return GetParameterInteral<T>(parameter, Parameter4, defaultIfEmpty);
                }
            }

            if (string.IsNullOrEmpty(AdditionalParameters))
            {
                if (defaultIfEmpty)
                    return default(T);

                throw new Exception(string.Format("Parameter {0} is empty, cannot be converted to {1}", parameter, typeof(T)));
            }

            var split = AdditionalParameters.Split(ParameterSeparatorChar);

            if (split.Length <= parameter - 5)
            {
                if (defaultIfEmpty)
                    return default(T);

                throw new Exception(string.Format("Parameter {0} is empty, cannot be converted to {1}", parameter, typeof(T)));
            }

            return (T)Convert.ChangeType(split[parameter - 5], typeof(T));
        }

        private T GetParameterInteral<T>(uint parameterNum, string parameterStr, bool defaultIfEmpty)
        {
            if (string.IsNullOrEmpty(parameterStr))
            {
                if (defaultIfEmpty)
                    return default(T);

                throw new Exception(string.Format("Parameter {0} is empty, cannot be converted to {1}", parameterNum, typeof(T)));
            }

            return (T)Convert.ChangeType(parameterStr, typeof(T));
        }

        public void SetParameter<T>(uint parameter, T value)
            where T : IConvertible
        {
            var valueStr = (string)Convert.ChangeType(value, typeof(string));
            if (parameter <= 4)
            {
                switch (parameter)
                {
                    case 0:
                        Parameter0 = valueStr;
                        break;
                    case 1:
                        Parameter1 = valueStr;
                        break;
                    case 2:
                        Parameter2 = valueStr;
                        break;
                    case 3:
                        Parameter3 = valueStr;
                        break;
                    case 4:
                        Parameter4 = valueStr;
                        break;
                }
            }
            else
            {
                var occurences = AdditionalParameters.CountOccurences(ParameterSeparatorChar);
                if (parameter - 5 > occurences)
                {
                    var builder = new StringBuilder();
                    builder.Append(AdditionalParameters);
                    for (int i = 0; i < (parameter - 5) - occurences; i++)
                    {
                        builder.Append(ParameterSeparatorChar);
                    }
                    builder.Append(valueStr);
                    AdditionalParameters = builder.ToString();
                }
                else
                {
                    int index = 0;
                    int currentParameter = 5;
                    while (parameter != currentParameter)
                    {
                        if (index == -1)
                            throw new Exception(string.Format("Cannot find parameter {0} in '{1}', something went wrong", parameter, AdditionalParameters));

                        index = AdditionalParameters.IndexOf(ParameterSeparatorChar, index);
                        currentParameter++;
                    }

                    var builder = new StringBuilder();
                    builder.Append(AdditionalParameters);
                    int nextIndex = AdditionalParameters.IndexOf(ParameterSeparatorChar, index);
                    builder.Remove(index + 1, ( nextIndex == -1 ? AdditionalParameters.Length - ( index + 1 ) : nextIndex - ( index + 1 ) ));
                    builder.Insert(index + 1, valueStr);
                    AdditionalParameters = builder.ToString();
                }
            }
        }

    }
}