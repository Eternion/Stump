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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using ProtoBuf;
using Stump.Server.BaseServer.Data.MapTool;
using Stump.Tools.UtilityBot.Commands;

namespace Stump.Tools.UtilityBot
{
    internal class Program
    {
        public static Bot BotSingleton;

        private static void Main(string[] args)
        {
            BotSingleton = new Bot();

             List<Map> list;
            var e = new Stopwatch();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            e.Start();
            list = Serializer.Deserialize<List<Map>>(new StreamReader(@"C:\Users\Jérémy\Documents\Visual Studio 2010\Projects\StumpGit\trunk\Run\content\maps\maps.dat").BaseStream);
            e.Stop();
            Console.WriteLine(e.ElapsedMilliseconds);
            GenerateMapsCommand.Process();
            while (true)
                Thread.Sleep(10);
        }
    }
}