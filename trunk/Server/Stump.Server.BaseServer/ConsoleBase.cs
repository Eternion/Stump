
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NLog;
using Stump.Core.Attributes;
using Stump.Core.Threading;

namespace Stump.Server.BaseServer
{
    public class ConsoleBase
    {
        /// <summary>
        /// Define the interval between two condition checks
        /// when server is asking something to the user by the console
        /// </summary>
        [Variable(true)]
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

        protected ConsoleBase()
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
                int pad = (Console.BufferWidth + line.Length) / 2;

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

        public bool AskAndWait(string request, int delay)
        {
            lock (m_conditionWaiter)
            {
                try
                {
                    AskingSomething = true;

                    logger.Warn(request + Environment.NewLine + "[CANCEL IN " + delay + " SECONDS] (y/n)");

                    // Wait that user enter any characters
                    if (ConditionWaiter.WaitFor(() => !EnteringCommand && Console.KeyAvailable, delay*1000, AskWaiterInterval))
                    {
                        // wait 'enter'
                        var response = Console.ReadLine().ToLower();

                        AskingSomething = false;
                        return response == "y" || response == "yes";
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
}