using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace UplauncherReplacer
{
    class Program
    {
        static void Main(string[] args)
        {
            var pid = int.Parse(args[0]);
            var temp = args[1];
            var replace = args[2];


            try
            {
                var process = Process.GetProcessById(pid);

                process.WaitForExit(10*1000);
            }
            catch
            {
                Console.WriteLine("Process not found");
            }

            Thread.Sleep(500);

            try
            {
                File.Copy(temp, replace, true);
                File.Delete(temp);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Could not replace uplauncher.exe ! Download it and replace it manually !!");
                File.WriteAllText("error.txt", ex.ToString());
                Console.Read();
            }

            Process.Start(replace);
        }
    }
}
