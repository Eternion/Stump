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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ProtoBuf;
using Stump.BaseCore.Framework.Attributes;
using Stump.Server.DataProvider.Core;
using Stump.Server.DataProvider.Data.Mount;
using Stump.Server.DataProvider.Data.Recipe;

namespace Stump.Server.DataProvider.Data.SuperAreas
{
    //public class SuperAreaTemplateManager : DataManager<int,SuperAreaTemplate>
    //{
    //    /// <summary>
    //    ///   Name of SuperArea templates file
    //    /// </summary>
    //    [Variable]
    //    public static string SuperAreaFile = "SuperAreaTemplates.xml";

    //    protected override SuperAreaTemplate InternalGetOne(int id)
    //    {
    //        using (var sr = new StreamReader(Settings.StaticPath + SuperAreaFile))
    //        {
    //            var superAreas = Serializer.Deserialize<List<SuperAreaTemplate>>(sr.BaseStream);

    //            return superAreas[id];
    //        }
    //    }

    //    protected override Dictionary<int, SuperAreaTemplate> InternalGetAll()
    //    {
    //        using (var sr = new StreamReader(Settings.StaticPath + SuperAreaFile))
    //        {
    //            return Serializer.Deserialize<List<SuperAreaTemplate>>(sr.BaseStream).ToDictionary(s => s.Id);
    //        }
    //    }
    //}
}