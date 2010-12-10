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
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NLog;
using Stump.BaseCore.Framework.Attributes;
using Stump.BaseCore.Framework.Utils;

namespace Stump.Server.BaseServer
{
    public class ConsoleBase
    {
        /// <summary>
        /// Define the interval between two condition checks
        /// when server is asking something to the user by the console
        /// </summary>
        [Variable]
        public static int AskWaiterInterval = 20;

        private Logger logger = LogManager.GetCurrentClassLogger();

        public static readonly string[] AsciiLogo = new[]
            {
                "  _____  _______  _     _   __   __   _____   ",
                " (_____)(__ _ __)(_)   (_) (__)_(__) (_____)  ",
                "(_)___     (_)   (_)   (_)(_) (_) (_)(_)__(_) ",
                "  (___)_   (_)   (_)   (_)(_) (_) (_)(_____)  ",
                "  ____(_)  (_)   (_)___(_)(_)     (_)(_)      ",
                " (_____)   (_)    (_____) (_)     (_)(_)      ",
            };

        public static readonly ConsoleColor[] LogoColors = new[]
            {
                ConsoleColor.DarkCyan,
                ConsoleColor.DarkRed, 
                ConsoleColor.DarkGray, 
                ConsoleColor.DarkGreen, 
                ConsoleColor.DarkYellow,
                ConsoleColor.Green,
                ConsoleColor.Red,
                ConsoleColor.White,
            };

        protected string Cmd = "";
        protected readonly ConditionWaiter m_conditionWaiter;

        public ConsoleBase()
        {
            m_conditionWaiter = new ConditionWaiter(() => !AskingSomething && Console.KeyAvailable, Timeout.Infinite, 20);
        }

        public bool EnteringCommand
        {
            get;
            set;
        }

        public bool AskingSomething
        {
            get;
            set;
        }

        public static void SetTitle(string str)
        {
            Console.Title = str;
        }

        public static void DrawAsciiLogo()
        {
            ConsoleColor color = Console.ForegroundColor;
            Console.ForegroundColor = LogoColors.ElementAt(new Random().Next(LogoColors.Count()));

            foreach (string line in AsciiLogo)
            {
                int pad = (Console.BufferWidth + line.Length)/2;

                Console.WriteLine(line.PadLeft(pad));
            }

            Console.ForegroundColor = color;
        }

        protected virtual void Process()
        {
        }

        public void Start()
        {
            Task.Factory.StartNew(Process);
        }

        public bool AskForSomething(string request, int delay)
        {
            try
            {
                AskingSomething = true;

                logger.Warn(request + " [CANCEL IN " + delay + " SECONDS] (y/n)");

                // Wait that user enter any characters ;)
                if (ConditionWaiter.WaitFor(() => !EnteringCommand && Console.KeyAvailable, delay * 1000, AskWaiterInterval))
                {
                    // wait 'enter'
                    var response = (char)Console.In.Peek();

                    AskingSomething = false;
                    return response == 'y';
                }

                AskingSomething = false;
                return false;
            }
            finally
            {
                AskingSomething = false;
            }
        }
    }
}