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
using System;

namespace Stump.Server.BaseServer.Commands
{
    public interface ICommandParameter : ICloneable
    {
        string Name
        {
            get;
        }

        string ShortName
        {
            get;
        }

        string Description
        {
            get;
        }

        bool IsOptional
        {
            get;
        }

        object Value
        {
            get;
        }

        object DefaultValue
        {
            get;
        }

        Type ValueType
        {
            get;
        }

        string StringValue
        {
            get;
            set;
        }

        bool IsRightName(string name);
        bool IsRightName(string name, bool useCase);
        void SetStringValue(string str);
        bool SetDefaultValue();
        string GetUsage();
    }

    public interface ICommandParameter<out T> : ICommandParameter
    {
        Converter<string, T> Converter
        {
            get;
        }

        new T DefaultValue
        {
            get;
        }

        new T Value
        {
            get;
        }

        T GetValue();
    }
}