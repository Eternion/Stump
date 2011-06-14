
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

            while (true)
                Thread.Sleep(10);
        }
    }
}